using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	private Rigidbody2D body;

	private Vector2 velocity;
	[SerializeField] private float angle;
	[SerializeField] private float moveSpd;

	private void Start() {
		//get references
		body = GetComponent<Rigidbody2D>();

		//hit ball in random direction at start
		ballHit(false, Random.Range(0, 2 * Mathf.PI), moveSpd);
	}

	private void FixedUpdate() {
		velocity = body.velocity;

		velocity = new Vector2(moveSpd * Mathf.Cos(angle), moveSpd * Mathf.Sin(angle));

		body.velocity = velocity;
	}


	public void ballHit(bool isCurveBall, float newAngle, float newSpeed) {
		moveSpd = newSpeed;
		angle = newAngle;
	}
}
