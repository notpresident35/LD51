using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class Paddle : MonoBehaviour
{
	[SerializeField] private int teamID;
	[SerializeField] private bool facingRight;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float strongHitStrength = 6;
	[SerializeField][Range(0, 1)] private float nonoZoneSize;
	[SerializeField][Range(0, 80)] private float angleSpread;
	[SerializeField] private float chargeTime;
	private Vector2 velocity;
	private float paddleHeight;
	private float chargeShotTimer;
	private float dashTimer;
	private float curveBallTimer;

	// TEMP FOR TESTING
	[SerializeField] bool mustFullyCharge;

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private InputHandler inputHandler;
	// Control keycode cache
	KeyCode upKey;
	KeyCode downKey;
	KeyCode chargeKey;

	private void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D>();
		inputHandler = GetComponent<InputHandler> ();
	}

	private void Start() {
		// Get initial input config
		upKey = inputHandler.GetKeycodeForInput($"P{teamID}Up");
		downKey = inputHandler.GetKeycodeForInput($"P{teamID}Down");
		chargeKey = inputHandler.GetKeycodeForInput($"P{teamID}Charge");

		chargeShotTimer = 0;

		//nono zone config?
		if ((facingRight && nonoZoneSize > .5f) || (!facingRight && nonoZoneSize < .5f)) {
			nonoZoneSize = 1-nonoZoneSize;
		}
		paddleHeight = 2 * boxCollider.bounds.extents.y;

		SingletonManager.Instance.GetComponentInChildren<TeamManager> ().RegisterPaddle (this, teamID - 1);
	}


	private void Update () {
		float yMoveDir = (Input.GetKey (upKey) ? 1 : 0) - (Input.GetKey (downKey) ? 1 : 0);

		// DEBUG NONO ZONE LINE UNTIL WE GET ART
		Debug.DrawRay(transform.TransformPoint(new Vector2(0, paddleHeight * (nonoZoneSize - .5f))), Vector3.right * (facingRight ? 1 : -1), Color.blue);

		//start charging
		if (Input.GetKeyDown(chargeKey)) {
			chargeShotTimer = 0;
		}
		
		if (!Input.GetKey(chargeKey)) {
			velocity = new Vector2(0, moveSpeed * yMoveDir);
		} else {
			// DEBUG LINE TO SHOW YOURE CHARGING
			Debug.DrawRay(transform.TransformPoint(Vector3.zero), Vector2.up, Color.green);
		}

		chargeShotTimer += Time.deltaTime;
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
				if (chargeShotTimer > chargeTime && Input.GetKey(chargeKey)) {
					hitStrength = strongHitStrength;
					print("yeeted");
				}
			} else if (Input.GetKey(chargeKey)) {
				hitStrength = Mathf.Min(chargeShotTimer / chargeTime, 1) * strongHitStrength;
				print($"{hitStrength}, {chargeShotTimer}, {chargeTime}");
			}
			chargeShotTimer = 0;

			collision.gameObject.GetComponent<Ball>().ballHit(false, hitAngle, hitStrength);
		}
	}

	void OnDestroy() {
		if (SingletonManager.Instance) {
			SingletonManager.Instance.GetComponentInChildren<TeamManager> ().DeregisterPaddle (this, teamID - 1);
		}
	}
}