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
            // TODO: uncomment
            /*GameObject newBall = BallPit.Pop ();
            newBall.transform.position = pos;*/
        }
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnRoundRestart.AddListener (SpawnBalls);
    }

    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnRoundRestart.AddListener (SpawnBalls);
    }
}
