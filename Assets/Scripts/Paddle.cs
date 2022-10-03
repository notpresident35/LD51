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
	[SerializeField] private float curveBallInitialAngle;
	[SerializeField] private float initialCurve;
	[SerializeField] private float curveFalloff;
	private Vector2 velocity;
	private float yMoveDir;
	private float chargeShotTimer;
	private float dashInterpolation;
	private float dashTimer;
	private float curveBallInputTimer;
	private float angleBias;
	private float curveSpeed;

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
		dashInterpolation = 0;
		curveBallInputTimer = 0;

		yMoveDir = 0;

		//nono zone config?
		if ((facingRight && nonoZoneSize > .5f) || (!facingRight && nonoZoneSize < .5f)) {
			nonoZoneSize = 1-nonoZoneSize;
		}
		paddleHeight = 2 * boxCollider.bounds.extents.y;
	}


	private void Update () {
		if (useDedicatedCharge) {
			chargeInput = Input.GetKey(chargeKey);
			chargeInputDown = Input.GetKeyDown(chargeKey);
		} else {
			chargeInput = Input.GetKey(upKey) && Input.GetKey(downKey);

			chargeInputDown = (Input.GetKeyDown(upKey) && Input.GetKey(downKey)) || (Input.GetKeyDown(downKey) && Input.GetKey(upKey));
		}

		yMoveDir = Mathf.Lerp(yMoveDir, (Input.GetKey(upKey) ? 1 : 0) - (Input.GetKey(downKey) ? 1 : 0), inputSmoothFactor);

		if (chargeInput) {
			yMoveDir = 0;

			// DEBUG ANIM TO SHOW YOURE CHARGING
			sprite.color = new Color(1, (1 - Mathf.Min(chargeShotTimer / chargeTime, 1)), 1);
			if (mustFullyCharge && chargeShotTimer / chargeTime >= 1) {
				sprite.color = new Color(0, .5f, .8f);
			}
		} else {
			sprite.color = Color.white;
		}

			// DEBUG NONO ZONE LINE UNTIL WE GET ART
			Debug.DrawRay(transform.TransformPoint(new Vector2(0, paddleHeight * (nonoZoneSize - .5f))), Vector3.right * (facingRight ? 1 : -1), Color.blue);

		//start charging
		if (chargeInputDown) {
			chargeShotTimer = 0;
		}

		//dash
		if (Input.GetKeyDown(dashKey) && dashTimer <= Mathf.Epsilon) {
			velocity.y = yMoveDir * dashSpeed;
			dashInterpolation = 1;
			dashTimer = dashCooldown;
		}

		velocity.y = Mathf.Lerp(velocity.y, moveSpeed * yMoveDir, 1 - dashInterpolation);

		chargeShotTimer += Time.deltaTime;
		curveBallInputTimer += Time.deltaTime;
		dashTimer = Mathf.Max(0, dashTimer - Time.deltaTime);

		dashInterpolation = Mathf.Max(0, dashInterpolation - (dashFalloff * Time.deltaTime));
	}

	private void FixedUpdate() {
		rb.velocity = velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "ball") {
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

			collision.gameObject.GetComponent<Ball>().ballHit(false, hitAngle, hitStrength);
		}
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