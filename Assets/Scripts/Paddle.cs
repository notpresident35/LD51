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
	[SerializeField] private float dashSpeed;
	[SerializeField] private float dashCooldown;
	[SerializeField] private float dashFalloff;
	[SerializeField] private float curveBallInputBuffer;
	[SerializeField] private PaddleVisuals paddleVisuals;
	private Vector2 velocity;
	private int yInputDir;
	private float yMoveDir;
	private float chargeShotTimer;
	private bool isCharged;
	private float dashInterpolationTimer;
	private float dashTimer; 
	private float curveBallInputTimer;
	private bool curveBallShotDue;

	private Ball lastHitBall;
	private float storedHitAngle;

	private float paddleHeight;

	private bool chargeInput;

	// TEMP - MAKE AN OPTION IN SETTINGS LATER
	[SerializeField] bool useDedicatedCharge;
	private SpriteRenderer sprite;

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
	}

	private void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D>();
		inputHandler = GetComponent<InputHandler> ();

		lastHitBall = null;

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
        Debug.DrawRay(transform.TransformPoint(new Vector2(0, paddleHeight * (nonoZoneSize - .5f))), Vector3.right * (facingRight ? 1 : -1), Color.blue);

        // Charge input
        if (useDedicatedCharge) {
            chargeInput = Input.GetKey (chargeKey);
        } else {
            chargeInput = Input.GetKey (upKey) && Input.GetKey (downKey);
        }

        // Charging behavior
        if (chargeInput) {
            yMoveDir = 0;

			float chargeAmount = chargeShotTimer / chargeTime;
            paddleVisuals.SetCharge (Mathf.Min(chargeAmount, 1));

			if (chargeShotTimer < Mathf.Epsilon) {
				AudioManager.PlaySound(chargeUp.clip, chargeUp.volume);
			}

			//full charge
			if (chargeAmount >= 1) {
				if (!curveBallShotDue) {
					curveBallInputTimer = 0;
				}

				if (!isCharged) {
					isCharged = true;
					AudioManager.PlaySound(chargeComplete.clip, chargeComplete.volume);
				}
			}
			
			//charge timer
			chargeShotTimer += Time.deltaTime;
		} else {
			isCharged = false;
            chargeShotTimer = 0;
			paddleVisuals.SetCharge(0);
		}

		// Execute a curveball that's due, OR buffer one to be redeemed later
		if (yInputDir != 0 && !chargeInput && curveBallInputTimer <= curveBallInputBuffer) {
			if (!curveBallShotDue) {
				curveBallShotDue = true;
			} else if (lastHitBall != null) {
				// do a late curveball with the hopefully saved last hit ball??
				lastHitBall.ballHit(-yInputDir * (facingRight ? 1 : -1), storedHitAngle, strongHitStrength);

				AudioManager.PlaySound(paddleHitCurveball.clip, paddleHitCurveball.volume);

				lastHitBall = null;
				curveBallShotDue = false;
				curveBallInputTimer = curveBallInputBuffer + 1;
			}
		}
		//clear due curveball shot
		if (curveBallInputTimer > curveBallInputBuffer) {
			curveBallShotDue = false;
			lastHitBall = null;
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
		curveBallInputTimer = Mathf.Clamp01(curveBallInputTimer + Time.deltaTime);
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

		float hitHeight = 0.5f + (transform.InverseTransformPoint(collision.GetContact(0).point).y / paddleHeight);

		float hitAngle = Mathf.Deg2Rad * angleSpread * (hitHeight - nonoZoneSize);
		if (!facingRight) { hitAngle = -(hitAngle + Mathf.PI); }

		if (curveBallShotDue) {
			//do a previously buffered curveball
			collision.gameObject.GetComponent<Ball>().ballHit(-yInputDir * (facingRight ? 1 : -1), hitAngle, strongHitStrength);

			AudioManager.PlaySound(paddleHitCurveball.clip, paddleHitCurveball.volume);

			lastHitBall = null;
			curveBallShotDue = false;
		} else {
			if (chargeShotTimer > chargeTime && chargeInput) {
				collision.gameObject.GetComponent<Ball>().ballHit(0, hitAngle, strongHitStrength);

				AudioManager.PlaySound(paddleHitHard.clip, paddleHitHard.volume);
				SingletonManager.EventSystemInstance.OnPaddleHit.Invoke(true);

				//allow a late curveball shot to be redeemed later
				lastHitBall = collision.gameObject.GetComponent<Ball>();
				curveBallInputTimer = 0;
				storedHitAngle = hitAngle;
				curveBallShotDue = true;
			} else {
				collision.gameObject.GetComponent<Ball>().ballHit(0, hitAngle);

				SingletonManager.EventSystemInstance.OnPaddleHit.Invoke(false);
				AudioManager.PlaySound(paddleHit.clip, paddleHit.volume);
			}
		}
		chargeShotTimer = 0;
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