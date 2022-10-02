using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject BallPrefab;

    static ObjectPool<Ball> BallPit;

    private void Awake () {
        // TODO: don't do this lol
        //BallPit = new ObjectPool (BallPrefab);
    }

    public void SpawnBalls () {
        foreach (Vector3 pos in GameState.CurrentMode.BallDefaultPositions) {
            // TODO: uncomment
            /*GameObject newBall = BallPit.Pop();
            newBall.transform.position = pos;
            float ballAngle = Random.Range(-Mathf.Deg2Rad * GameState.CurrentMode.ballStartAngleSpread, Mathf.Deg2Rad * GameState.CurrentMode.ballStartAngleSpread);
            if (GameState.CurrentMode.ballStartsWithRandomDirection) {
                GameState.CurrentMode.ballStartsGoingLeft = Random.Range(0.0f, 1.0f) > 0.5f;
            }
            if (GameState.CurrentMode.ballStartsGoingLeft) {
                ballAngle = -(ballAngle + Mathf.PI);
            }
            newBall.GetComponent<Ball>().ballHit(false, ballAngle);*/
        }
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnRoundRestart.AddListener (SpawnBalls);
    }

    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnRoundRestart.AddListener (SpawnBalls);
    }
}
