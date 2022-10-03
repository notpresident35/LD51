using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
	[SerializeField] private int teamID;
	[SerializeField] private bool facingRight;
	[SerializeField] private bool canMoveDuringCharge;
	[SerializeField] private float minYPos;
	[SerializeField] private float maxYPos;
	[SerializeField] protected float moveSpeed;
	[SerializeField] private float inputSmoothFactor;
	[SerializeField] private float strongHitStrength;
	[SerializeField] private float curveHitStrength;
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

	protected virtual void Config() {
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

    protected virtual void Awake () {
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

        paddleVisuals.SetupCurveIndicators (facingRight);
    }


	private void Update () {
        if (!GameState.IsBallActive) {
            yInputDir = 0;
            paddleVisuals.SetCharge (0);
			chargeShotTimer = 0;
            curveBallInputTimer = 0;
            chargeShotActiveBuffer = 0;
			dashTimer = 0;
            dashInterpolationTimer = 0;
            return;
        }

		// Movement
		Move(HandleMovementInput ());
        // Charging
        Charge(HandleChargingInput ());
        // Dashing
        Dash(HandleDashInput ());

		ApplyMovement ();

        // Timers
        curveBallInputTimer = Mathf.Clamp01(curveBallInputTimer - Time.deltaTime);
		chargeShotActiveBuffer = Mathf.Clamp01(chargeShotActiveBuffer - Time.deltaTime);
		dashTimer = Mathf.Clamp01 (dashTimer - Time.deltaTime);
		dashInterpolationTimer = Mathf.Clamp01 (dashInterpolationTimer - (dashFalloff * Time.deltaTime));

        // DEBUG NONO ZONE LINE UNTIL WE GET ART
        //Debug.DrawRay(transform.TransformPoint(new Vector2(0, paddleHeight * (nonoZoneSize - .5f))), Vector3.right * (facingRight ? 1 : -1), Color.blue);
    }

    protected virtual float HandleMovementInput () {
        yInputDir = 0;
        if (Input.GetKey (upKey)) {
            yInputDir++;
        }
        if (Input.GetKey (downKey)) {
            yInputDir--;
        }

		return yInputDir;
    }

	void Move (float yInputDir) {
        yMoveDir = Mathf.Lerp (yMoveDir, yInputDir, inputSmoothFactor);
    }

    protected virtual bool HandleDashInput () {
		return Input.GetKeyDown (dashKey);
	}

	void Dash (bool dashInput) {
        if (dashInput && dashTimer <= Mathf.Epsilon) {
            velocity.y = yMoveDir * dashSpeed;
            dashInterpolationTimer = 1;
            dashTimer = dashCooldown;

            AudioManager.PlaySound (dash.clip, dash.volume);
        }
    }

	void ApplyMovement () {
        // Merge movement and dash
        velocity.y = Mathf.Lerp (velocity.y, moveSpeed * yMoveDir, 1 - dashInterpolationTimer);

		// Clamp paddle to bounds
        transform.position = new Vector2 (transform.position.x, Mathf.Clamp (transform.position.y, minYPos, maxYPos));
    }

    protected virtual bool HandleChargingInput () {
        bool chargeInput;
        if (useDedicatedCharge) {
            chargeInput = Input.GetKey (chargeKey);
        } else {
            chargeInput = Input.GetKey (upKey) && Input.GetKey (downKey);
        }

		return chargeInput;
    }

	void Charge (bool charging) {
        if (charging) {
            if (!canMoveDuringCharge) {
                yMoveDir = 0;
            }

            paddleVisuals.SetCharge (chargeShotTimer);

            if (chargeShotTimer <= Mathf.Epsilon) {
                AudioManager.PlaySound (chargeUp.clip, chargeUp.volume);
            }

            //full charge
            if (chargeShotTimer >= 1) {
                if (chargeShotActiveBuffer <= Mathf.Epsilon) {
                    AudioManager.PlaySound (chargeComplete.clip, chargeComplete.volume);
                }
                chargeShotActiveBuffer = chargeShotInputBuffer;
            }

            chargeShotTimer = Mathf.Clamp01 (chargeShotTimer + Time.deltaTime / chargeTime);
        } else {
            chargeShotTimer = 0;
            paddleVisuals.SetCharge (0);
        }
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

		HandleHitBall (collision);
	}

	protected virtual void HandleHitBall (Collision2D ballCollision) {

        // Find y position of the ball relative to the paddle, from 0-1 (0 is bottom of paddle, 1 is top, regardless of flipping)
        float hitHeight = Mathf.Clamp01 (0.5f + (transform.InverseTransformPoint (ballCollision.GetContact (0).point).y / paddleHeight));

        // Find angle ball should be hit at
        float hitAngle = Mathf.Deg2Rad * angleSpread * (hitHeight - nonoZoneSize);
        // Flip if facing right
        if (!facingRight) { hitAngle = -(hitAngle + Mathf.PI); }

        // If can hit charged shot, prefer to do so
        if (chargeShotActiveBuffer >= Mathf.Epsilon) {
            AudioManager.PlaySound (paddleHitHard.clip, paddleHitHard.volume);
            SingletonManager.EventSystemInstance.OnPaddleHit.Invoke (true, ballCollision.transform.position);

            StartCoroutine (WaitForCurveInput (ballCollision.gameObject.GetComponent<Ball> (), hitAngle));
        } else {
            ballCollision.gameObject.GetComponent<Ball> ().ballHit (0, hitAngle);
            SingletonManager.EventSystemInstance.OnPaddleHit.Invoke (false, ballCollision.transform.position);
            AudioManager.PlaySound (paddleHit.clip, paddleHit.volume);
        }

        chargeShotActiveBuffer = 0;
        chargeShotTimer = 0;
    }

    protected virtual int HandleCurveDir () {
        int curveDir = 0;

        if (Input.GetKeyDown (upKey)) {
            curveDir = -1;
        } else if (Input.GetKeyDown (downKey)) {
            curveDir = 1;
        }

        curveDir *= facingRight ? 1 : -1;

        return curveDir;
    }

    IEnumerator WaitForCurveInput (Ball ball, float hitAngle) {
		int curveDir = 0;

        paddleVisuals.ShowCurveIndicators ();

        for (float i = 0; i < curveBallInputDetectionTime; i += Time.unscaledDeltaTime) {
            int newDir = HandleCurveDir ();
            if (newDir != 0) {
                curveDir = newDir;
                i = curveBallInputDetectionTime;
                JuiceManager.TimeFreezeJuice = 0;
            }
            yield return null;
		}

		if (curveDir != 0) {
			AudioManager.PlaySound (paddleHitCurveball.clip, paddleHitCurveball.volume);
            ball.ballHit (curveDir, hitAngle, curveHitStrength);
		} else {
            AudioManager.PlaySound (paddleHitHard.clip, paddleHitHard.volume);
            ball.ballHit (curveDir, hitAngle, strongHitStrength);
        }

		paddleVisuals.HideCurveIndicators ();
    }

	protected virtual void OnEnable () {
        SingletonManager.TeamManagerInstance.RegisterPaddle (this, teamID - 1);
        SingletonManager.EventSystemInstance.OnSettingsSaved.AddListener (Config);
    }

    protected virtual void OnDisable () {
		if (SingletonManager.Instance) {
			SingletonManager.TeamManagerInstance.DeregisterPaddle (this, teamID - 1);
            SingletonManager.EventSystemInstance.OnSettingsSaved.RemoveListener (Config);
        }
	}
}