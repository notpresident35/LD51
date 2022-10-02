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
