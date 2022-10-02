using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	private Rigidbody2D body;
	private float explosionDelayTime = 0.02f;

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

		EventSystem.Instance.goalHit.AddListener(explodeBall);

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

	public void explodeBall(int team, Vector3 endPos) {

		float dist = Vector3.Distance(endPos, transform.position);
		float waitTime = dist * explosionDelayTime;

		Destroy(gameObject, waitTime);

    }

    private void OnDestroy() {
		EventSystem.Instance.ballExplode.Invoke(transform.position);
    }

}
