using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class Paddle : MonoBehaviour
{
	[SerializeField] private int teamID;
	[SerializeField] private bool facingRight;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float hitStrength = 8;
    private Vector2 velocity;

    private Rigidbody2D rb;
    private InputHandler inputHandler;
    // Control keycode cache
    KeyCode upKey;
    KeyCode downKey;

	private void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		inputHandler = GetComponent<InputHandler> ();
	}

    private void Start() {
        // Get initial input config
        upKey = inputHandler.GetKeycodeForInput($"P{teamID}Up");
		downKey = inputHandler.GetKeycodeForInput($"P{teamID}Down");

        SingletonManager.Instance.GetComponentInChildren<TeamManager> ().RegisterPaddle (this, teamID);
    }


    private void Update () {
		float yMoveDir = (Input.GetKey (upKey) ? 1 : 0) - (Input.GetKey (downKey) ? 1 : 0);

		velocity = new Vector2(0, moveSpeed * yMoveDir);
	}

	private void FixedUpdate() {
		rb.velocity = velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "ball") {
			float hitAngle = Mathf.Atan2(0, facingRight ? 1 : -1);
			collision.gameObject.GetComponent<Ball>().ballHit(false, hitAngle, hitStrength);
		}
	}

	void OnDestroy() {
		if (SingletonManager.Instance) {
			SingletonManager.Instance.GetComponentInChildren<TeamManager> ().DeregisterPaddle (this, teamID);
		}
    }
}