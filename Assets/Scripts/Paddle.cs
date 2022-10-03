using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class Paddle : MonoBehaviour
{
	[SerializeField] private int teamID;
	[SerializeField] private bool facingRight;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float inputSmoothFactor;
	[SerializeField] private float strongHitStrength;
	[SerializeField][Range(0, 1)] private float nonoZoneSize;
	[SerializeField][Range(0, 80)] private float angleSpread;
	[SerializeField] private float chargeTime;
	[SerializeField] private float dashSpeed;
	[SerializeField] private float dashCooldown;
	[SerializeField] private float dashFalloff;
	[SerializeField] private float curveBallInputBuffer;
	[SerializeField] private PaddleVisuals paddleVisuals;
	private Vector2 velocity;
	private float yMoveDir;
	private float chargeShotTimer;
	private float dashInterpolationTimer;
	private float dashTimer;
	private float curveBallInputTimer;

	private float paddleHeight;

	private bool chargeInput;
	private bool chargeInputDown;

	// TEMP - MAKE AN OPTION IN SETTINGS LATER
	[SerializeField] bool useDedicatedCharge;
	// TEMP FOR TESTING
	[SerializeField] bool mustFullyCharge;
	// TEMP FOR UNTIL WE HAVE ANIMATIONS
	private SpriteRenderer sprite;

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private InputHandler inputHandler;
	// Control keycode cache
	KeyCode upKey;
	KeyCode downKey;
	KeyCode chargeKey;
	KeyCode dashKey;

	public void Config() {
		upKey = inputHandler.GetKeycodeForInput($"P{teamID}Up");
		downKey = inputHandler.GetKeycodeForInput($"P{teamID}Down");
		chargeKey = inputHandler.GetKeycodeForInput($"P{teamID}Charge");
		dashKey = inputHandler.GetKeycodeForInput($"P{teamID}Dash");
	}

	private void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D>();
		inputHandler = GetComponent<InputHandler> ();

		// TEMP FOR UNTIL WE HAVE ANIMATIONS
		sprite = GetComponentInChildren<SpriteRenderer>();
	}

	private void Start() {
		// Get initial input config
		Config();

		chargeShotTimer = 0;
		dashInterpolationTimer = 0;
		curveBallInputTimer = 0;

		yMoveDir = 0;

		//nono zone config?
		if ((facingRight && nonoZoneSize > .5f) || (!facingRight && nonoZoneSize < .5f)) {
			nonoZoneSize = 1-nonoZoneSize;
		}
		paddleHeight = 2 * boxCollider.bounds.extents.y;
	}


	private void Update () {
		// Move input
		float yInputDir = 0;
		if (Input.GetKey (upKey)) {
			yInputDir++;
        }
        if (Input.GetKey (downKey)) {
            yInputDir--;
        }

		// Movement
        yMoveDir = Mathf.Lerp(yMoveDir, yInputDir, inputSmoothFactor);

        // DEBUG NONO ZONE LINE UNTIL WE GET ART
        //Debug.DrawRay(transform.TransformPoint(new Vector2(0, paddleHeight * (nonoZoneSize - .5f))), Vector3.right * (facingRight ? 1 : -1), Color.blue);

        // Charge input
        if (useDedicatedCharge) {
            chargeInput = Input.GetKey (chargeKey);
        } else {
            chargeInput = Input.GetKey (upKey) && Input.GetKey (downKey);
        }

        // Charging behavior
        if (chargeInput) {
            yMoveDir = 0;
            paddleVisuals.SetCharge (1 - Mathf.Min (chargeShotTimer / chargeTime, 1));
        } else {
            chargeShotTimer = 0;
        }

        // Dashing
        bool dashInputDown = Input.GetKeyDown (dashKey);
        if (dashInputDown && dashTimer <= Mathf.Epsilon) {
			velocity.y = yMoveDir * dashSpeed;
			dashInterpolationTimer = 1;
			dashTimer = dashCooldown;
		}
		
		// Merge movement and dash
		velocity.y = Mathf.Lerp(velocity.y, moveSpeed * yMoveDir, 1 - dashInterpolationTimer);

		// Timers
		chargeShotTimer = Mathf.Clamp01 (chargeShotTimer + Time.deltaTime);
		curveBallInputTimer = Mathf.Clamp01 (curveBallInputTimer + Time.deltaTime);
		dashTimer = Mathf.Clamp01 (dashTimer - Time.deltaTime);
		dashInterpolationTimer = Mathf.Clamp01 (dashInterpolationTimer - (dashFalloff * Time.deltaTime));
	}

	private void FixedUpdate() {
		rb.velocity = velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag != "ball") {
			return;
		}

		float hitHeight = 0.5f + (transform.InverseTransformPoint(collision.GetContact(0).point).y / paddleHeight);

		float hitAngle = Mathf.Deg2Rad * angleSpread * (hitHeight - nonoZoneSize);
		if (!facingRight) { hitAngle = -(hitAngle + Mathf.PI); }
			
		float hitStrength = 0;
		if (mustFullyCharge) {
			if (chargeShotTimer > chargeTime && chargeInput) {
				hitStrength = strongHitStrength;
			}
		} else if (chargeInput) {
			hitStrength = Mathf.Min(chargeShotTimer / chargeTime, 1) * strongHitStrength;
		}
		chargeShotTimer = 0;

		collision.gameObject.GetComponent<Ball>().ballHit(0, hitAngle, hitStrength);
	}

	private void OnEnable () {
        SingletonManager.TeamManagerInstance.RegisterPaddle (this, teamID - 1);
		SingletonManager.EventSystemInstance.OnSettingsSaved.AddListener(Config);
	}

	private void OnDisable () {
		if (SingletonManager.Instance) {
			SingletonManager.TeamManagerInstance.DeregisterPaddle (this, teamID - 1);
			SingletonManager.EventSystemInstance.OnSettingsSaved.RemoveListener(Config);
		}
	}
}