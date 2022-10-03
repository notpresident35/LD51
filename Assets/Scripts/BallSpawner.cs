using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public Ball BallPrefab;

    private ObjectPool<Ball> BallPit;

    private void Awake () {
        BallPit = new ObjectPool<Ball> (BallPrefab);
    }

    public void SpawnBalls () {
        foreach (Vector3 pos in GameState.CurrentMode.BallDefaultPositions) {
            Ball newBall = BallPit.Pop();
            newBall.transform.position = pos;
            float ballAngle = Random.Range(-Mathf.Deg2Rad * GameState.CurrentMode.ballStartAngleSpread, Mathf.Deg2Rad * GameState.CurrentMode.ballStartAngleSpread);
            if (GameState.CurrentMode.ballStartsWithRandomDirection) {
                GameState.CurrentMode.ballStartsGoingLeft = Random.Range(0.0f, 1.0f) > 0.5f;
            }
            if (GameState.CurrentMode.ballStartsGoingLeft) {
                ballAngle = -(ballAngle + Mathf.PI);
            }
            newBall.ballHit(0, ballAngle);
        }
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnRoundRestart.AddListener (SpawnBalls);
        SingletonManager.EventSystemInstance.OnGameRestart.AddListener (SpawnBalls);
    }

    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnRoundRestart.RemoveListener (SpawnBalls);
        SingletonManager.EventSystemInstance.OnGameRestart.RemoveListener (SpawnBalls);
    }
}
