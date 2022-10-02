using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	private Rigidbody2D body;

	private Vector2 velocity;
	[SerializeField] private float angle;
	[SerializeField] private float startSpd;
	[SerializeField] private float moveSpd;
	[SerializeField] private float speedMultiplier;
	[SerializeField] private float smoothFactor;

	private void Start() {
		//get references
		body = GetComponent<Rigidbody2D>();

		//defaults
		speedMultiplier = 1;
		moveSpd = startSpd;

		//hit ball in random direction at start
		ballHit(false, Random.Range(0, 2 * Mathf.PI), moveSpd);
	}

	private void FixedUpdate() {
		velocity = body.velocity;

		moveSpd = Mathf.Lerp(moveSpd, startSpd * speedMultiplier, smoothFactor);
		velocity = new Vector2(moveSpd * Mathf.Cos(angle), moveSpd * Mathf.Sin(angle));

		body.velocity = velocity;
	}


	public void ballHit(bool isCurveBall, float newAngle, float hitStrength = 0) {
		moveSpd = (startSpd * speedMultiplier) + hitStrength;
		angle = newAngle;
	}
}
