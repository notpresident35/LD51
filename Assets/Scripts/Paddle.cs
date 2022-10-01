using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class Paddle : MonoBehaviour
{
	private Rigidbody2D body;
	private InputHandler inputHandler;

	//controls
	KeyCode upKey;
	KeyCode downKey;

	[SerializeField] private int teamID;
	private Vector2 velocity;
	[SerializeField] private int facing;
	[SerializeField] private float moveSpd;

	private void Start() {
		//get references
		body = GetComponent<Rigidbody2D>();
		inputHandler = GetComponent<InputHandler>();

		//get initial input config
		upKey = inputHandler.GetKeycodeForInput("P1Up");
		downKey = inputHandler.GetKeycodeForInput("P1Down");
	}

	private void Update() {
		float vspd = moveSpd * ((Input.GetKey(upKey) ? 1 : 0) - (Input.GetKey(downKey) ? 1 : 0));

		velocity = new Vector2(0, vspd);
	}

	private void FixedUpdate() {
		body.velocity = velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "ball") {
			float hitAngle;
			float hitStrength = 8;

			hitAngle = Mathf.Atan2(0,facing);

			collision.gameObject.GetComponent<Ball>().ballHit(false, hitAngle, hitStrength);
			print("YEEEEEEET");
		}
	}
}