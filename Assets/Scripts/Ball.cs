using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	private Rigidbody2D rb;
	private float explosionDelayTime = 0.02f;

	[SerializeField] private float angle;
	[SerializeField] private float initialSpeed;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float speedMultiplier = 1;
	[SerializeField] private float smoothFactor;

	// TEMP FOR TESTING - REPLACE WITH REGULAR STARTING STATE LATER
	[SerializeField] private float initialRotation;
	[SerializeField] private bool useRandomInitialRotation;

	private void Awake () {
		rb = GetComponent<Rigidbody2D> ();
	}

	private void Start() {
		moveSpeed = initialSpeed;

		// Hit ball at start
		ballHit(false, useRandomInitialRotation ? Random.Range (0, 2 * Mathf.PI) : initialRotation, moveSpeed);
	}

	private void FixedUpdate() {
		moveSpeed = Mathf.Lerp(moveSpeed, initialSpeed * speedMultiplier, smoothFactor);
		Vector2 velocity = new Vector2(moveSpeed * Mathf.Cos(angle), moveSpeed * Mathf.Sin(angle));

		rb.velocity = velocity;
	}


	public void ballHit(bool isCurveBall, float newAngle, float hitStrength = 0) {
		moveSpeed = (initialSpeed * speedMultiplier) + hitStrength;
		angle = newAngle;
	}

	public void explodeBall(int team, Vector3 endPos) {
		float dist = Vector3.Distance (endPos, transform.position);
		float waitTime = dist * explosionDelayTime;

		StartCoroutine(DeathEffects(waitTime));
	}

	IEnumerator DeathEffects (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		SingletonManager.Instance.GetComponentInChildren<EventSystem> ().OnBallExplode.Invoke (transform.position);
		Destroy (gameObject);
	}

	void OnEnable () {
		SingletonManager.Instance.GetComponentInChildren<EventSystem> ().OnGoalHit.AddListener (explodeBall);
	}

	private void OnDisable() {
		if (SingletonManager.Instance) {
			SingletonManager.Instance.GetComponentInChildren<EventSystem> ().OnGoalHit.RemoveListener (explodeBall);
		}
	}
}
