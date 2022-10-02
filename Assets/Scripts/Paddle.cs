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
	private Vector2 velocity;
	private float paddleHeight;

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private InputHandler inputHandler;
	// Control keycode cache
	KeyCode upKey;
	KeyCode downKey;

	private void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D>();
		inputHandler = GetComponent<InputHandler> ();
	}

	private void Start() {
		// Get initial input config
		upKey = inputHandler.GetKeycodeForInput($"P{teamID}Up");
		downKey = inputHandler.GetKeycodeForInput($"P{teamID}Down");

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

		velocity = new Vector2(0, moveSpeed * yMoveDir);
	}

	private void FixedUpdate() {
		rb.velocity = velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "ball") {
			Debug.DrawRay(collision.GetContact(0).point, Vector2.right, Color.red, 1);

			float hitHeight = 0.5f + (transform.InverseTransformPoint(collision.GetContact(0).point).y / paddleHeight);
			print(hitHeight);

			float hitAngle = Mathf.Deg2Rad * angleSpread * (hitHeight - nonoZoneSize);
			if (!facingRight) { hitAngle = -(hitAngle + Mathf.PI); }
			collision.gameObject.GetComponent<Ball>().ballHit(false, hitAngle, strongHitStrength);
		}
	}

	void OnDestroy() {
		if (SingletonManager.Instance) {
			SingletonManager.Instance.GetComponentInChildren<TeamManager> ().DeregisterPaddle (this, teamID - 1);
		}
	}
}