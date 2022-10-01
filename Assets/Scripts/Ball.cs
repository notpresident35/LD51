using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//vvv REMOVE LATER WE DONT NEED IT HERE vvv
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
	//test event ig??
	UnityEvent<bool, float, float> ballHit_test = new UnityEvent<bool, float, float>();

	private Rigidbody2D body;

	private Vector2 velocity;
	[SerializeField] private float angle;
	[SerializeField] private float moveSpd;

	private void Start() {
		//get references
		body = GetComponent<Rigidbody2D>();

		//test event
		ballHit_test.AddListener(onBallHit);

		//hit ball in random direction at start
		ballHit_test.Invoke(false, Random.Range(0, 2 * Mathf.PI), moveSpd);
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			ballHit_test.Invoke(false, Mathf.Atan2(-transform.position.y, -transform.position.x), moveSpd + .1f);
		}
	}

	private void FixedUpdate() {
		velocity = body.velocity;

		velocity = new Vector2(moveSpd * Mathf.Cos(angle), moveSpd * Mathf.Sin(angle));

		body.velocity = velocity;
	}


	private void onBallHit(bool isCurveBall, float newAngle, float newSpeed) {
		moveSpd = newSpeed;
		angle = newAngle;
	}
}
