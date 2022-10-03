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
	[SerializeField] private float speedAccPerHit;
    [SerializeField] private float speedSmoothFactor;
	[SerializeField] private float curveBallInitialAngle;
	[SerializeField] private float curveSpeedInitial;
	[SerializeField] private float curveFalloff;
	private float angleBias;
	private float curveDir;
	private float curveSpeed;

	//sfx
	[SerializeField] SoundEffect explode;

	// TEMP FOR TESTING - REPLACE WITH REGULAR STARTING STATE LATER
	[SerializeField] private float initialRotation;
	[SerializeField] private bool useRandomInitialRotation;

	private void Awake () {
        rb = GetComponent<Rigidbody2D> ();
    }

    private void Start() {
		moveSpeed = initialSpeed;
		curveDir = 0;

		curveBallInitialAngle *= Mathf.Deg2Rad;
		curveSpeedInitial *= Mathf.Deg2Rad;

		// Hit ball at start
		ballHit(0, useRandomInitialRotation ? Random.Range (0, 2 * Mathf.PI) : initialRotation, moveSpeed);
    }

	private void FixedUpdate() {
        if (!GameState.IsBallActive) {
            return;
        }
		moveSpeed = Mathf.Lerp(moveSpeed, initialSpeed * speedMultiplier, speedSmoothFactor * Time.deltaTime);

		if (curveDir != 0) {
			curveSpeed = Mathf.Lerp(curveSpeed, 0, curveFalloff * Time.deltaTime);
			angleBias += curveDir * curveSpeed * Time.deltaTime;
		}
		
		Vector2 velocity = new Vector2(moveSpeed * Mathf.Cos(angle + angleBias), moveSpeed * Mathf.Sin(angle + angleBias));

		rb.velocity = velocity;
	}


	public void ballHit(int _curveDir, float newAngle, float hitStrength = 0) {
		moveSpeed = (initialSpeed * speedMultiplier) + hitStrength;
		angle = newAngle;

		speedMultiplier += speedAccPerHit;

		if (_curveDir != 0) {
			curveDir = _curveDir;
			curveSpeed = curveSpeedInitial;
			angleBias = -_curveDir * curveBallInitialAngle;
		} else {
			curveDir = 0;
			curveSpeed = 0;
			angleBias = 0;
		}
	}

	public void explodeBall(int team, Vector3 endPos) {
		float dist = Vector3.Distance (endPos, transform.position);
		float waitTime = dist * explosionDelayTime;

		StartCoroutine(DeathEffects(waitTime));
    }

	IEnumerator DeathEffects (float waitTime) {
		yield return new WaitForSeconds (waitTime);
        SingletonManager.EventSystemInstance.OnBallExplode.Invoke (transform.position);
		AudioManager.PlaySound(explode.clip, explode.volume);
        Destroy (gameObject);
    }

	void OnEnable () {
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener (explodeBall);
    }

    private void OnDisable() {
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener (explodeBall);
    }
}
