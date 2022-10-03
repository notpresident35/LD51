using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class Paddle : MonoBehaviour
{
	[SerializeField] private int teamID;
	[SerializeField] private bool facingRight;
	[SerializeField] private float minYPos;
	[SerializeField] private float maxYPos;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float inputSmoothFactor;
	[SerializeField] private float strongHitStrength;
	[SerializeField][Range(0, 1)] private float nonoZoneSize;
	[SerializeField][Range(0, 80)] private float angleSpread;
	[SerializeField] private float chargeTime;
	[SerializeField] private float chargeShotInputBuffer;
	[SerializeField] private float dashSpeed;
	[SerializeField] private float dashCooldown;
	[SerializeField] private float dashFalloff;
	[SerializeField] private float curveBallInputDetectionTime;
	[SerializeField] private PaddleVisuals paddleVisuals;

	private Vector2 velocity;
	private int yInputDir;
	private float yMoveDir;

	private float chargeShotActiveBuffer;
	private float chargeShotTimer;
	private float dashInterpolationTimer;
	private float dashTimer;
	private float curveBallInputTimer;

	private float paddleHeight;

	private bool useDedicatedCharge;

	//sfx
	[SerializeField] SoundEffect paddleHit;
	[SerializeField] SoundEffect paddleHitHard;
	[SerializeField] SoundEffect paddleHitCurveball;
	[SerializeField] SoundEffect dash;
	[SerializeField] SoundEffect chargeUp;
	[SerializeField] SoundEffect chargeComplete;

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

		if (teamID == 1) {
			useDedicatedCharge = PlayerPrefHandler.GetBool(Statics.DedicatedChargeP1PPD);
		} else if (teamID == 2) {
			useDedicatedCharge = PlayerPrefHandler.GetBool(Statics.DedicatedChargeP2PPD);
		}
	}

	private void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D>();
		inputHandler = GetComponent<InputHandler> ();
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
		yInputDir = 0;
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
		bool chargeInput;
        if (useDedicatedCharge) {
            chargeInput = Input.GetKey (chargeKey);
		} else {
			chargeInput = Input.GetKey(upKey) && Input.GetKey(downKey);
		}

		// Charging behavior
		if (chargeInput) {
			yMoveDir = 0;

			paddleVisuals.SetCharge(chargeShotTimer);

			if (chargeShotTimer <= Mathf.Epsilon) {
				AudioManager.PlaySound(chargeUp.clip, chargeUp.volume);
			}

			//full charge
			if (chargeShotTimer >= 1) {
				if (chargeShotActiveBuffer <= Mathf.Epsilon) {
					AudioManager.PlaySound(chargeComplete.clip, chargeComplete.volume);
				}
                chargeShotActiveBuffer = chargeShotInputBuffer;
			}

			chargeShotTimer = Mathf.Clamp01 (chargeShotTimer + Time.deltaTime / chargeTime);

        } else {
            chargeShotTimer = 0;
			paddleVisuals.SetCharge(0);
		}

        // Dashing
        bool dashInputDown = Input.GetKeyDown (dashKey);
        if (dashInputDown && dashTimer <= Mathf.Epsilon) {
			velocity.y = yMoveDir * dashSpeed;
			dashInterpolationTimer = 1;
			dashTimer = dashCooldown;

			AudioManager.PlaySound(dash.clip, dash.volume);
		}
		
		// Merge movement and dash
		velocity.y = Mathf.Lerp(velocity.y, moveSpeed * yMoveDir, 1 - dashInterpolationTimer);

		transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, minYPos, maxYPos));

		// Timers
		curveBallInputTimer = Mathf.Clamp01(curveBallInputTimer - Time.deltaTime);
		chargeShotActiveBuffer = Mathf.Clamp01(chargeShotActiveBuffer - Time.deltaTime);
		dashTimer = Mathf.Clamp01 (dashTimer - Time.deltaTime);
		dashInterpolationTimer = Mathf.Clamp01 (dashInterpolationTimer - (dashFalloff * Time.deltaTime));
	}

	private void FixedUpdate() {
        if (!GameState.IsBallActive) {
			rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag != "ball") {
			return;
		}

        // Find y position of the ball relative to the paddle, from 0-1 (0 is bottom of paddle, 1 is top, regardless of flipping)
        float hitHeight = Mathf.Clamp01(0.5f + (transform.InverseTransformPoint(collision.GetContact(0).point).y / paddleHeight));

		// Find angle ball should be hit at
		float hitAngle = Mathf.Deg2Rad * angleSpread * (hitHeight - nonoZoneSize);
		// Flip if facing right
		if (!facingRight) { hitAngle = -(hitAngle + Mathf.PI); }

		// If can hit charged shot, prefer to do so
		if (chargeShotActiveBuffer >= Mathf.Epsilon) {
			AudioManager.PlaySound(paddleHitHard.clip, paddleHitHard.volume);
			SingletonManager.EventSystemInstance.OnPaddleHit.Invoke(true, collision.transform.position);

			StartCoroutine (WaitForCurveInput (collision, hitAngle));
		} else {
			collision.gameObject.GetComponent<Ball>().ballHit(0, hitAngle);
			SingletonManager.EventSystemInstance.OnPaddleHit.Invoke(false, collision.transform.position);
			AudioManager.PlaySound(paddleHit.clip, paddleHit.volume);
		}

		chargeShotActiveBuffer = 0;
        chargeShotTimer = 0;
	}

	IEnumerator WaitForCurveInput (Collision2D collision, float hitAngle) {
		int curveDir = 0;

		for (float i = 0; i < curveBallInputDetectionTime; i += Time.unscaledDeltaTime) {
            if (Input.GetKeyDown (upKey)) {
				curveDir = facingRight ? -1 : 1;
            } else if (Input.GetKeyDown (downKey)) {
				curveDir = facingRight ? 1 : -1;
            }
            yield return null;
		}

        collision.gameObject.GetComponent<Ball> ().ballHit (curveDir, hitAngle, strongHitStrength);

		if (curveDir != 0) {
            AudioManager.PlaySound (paddleHitCurveball.clip, paddleHitCurveball.volume);
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