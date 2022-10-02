using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float RestartTime = 2;
    public GameMode DefaultMode;

    // Note: Because GameManager is later in script execution order, it adds this as a listener after all other listeners
    // This means that it can rely on the score being updated when that happens in another script before this is called
    // This is quite precarious, but it gets the job done
    void HandleGoalScored (int teamID, Vector3 pos) {
        GameState.IsBallActive = false;
        if (SingletonManager.TeamManagerInstance.Teams [teamID].Score >= Statics.WinningScore) {
            // This team wins!
            StopGame (teamID);
        } else {
            GameState.IsBallActive = true;
            StartCoroutine (RestartRound ());
        }
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener (HandleGoalScored);
    }

    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener (HandleGoalScored);
    }

    IEnumerator RestartRound () {
        yield return new WaitForSeconds (RestartTime);
        GameState.IsBallActive = true;
        SingletonManager.EventSystemInstance.OnRoundRestart.Invoke ();
    }

    void StopGame (int winTeam) {
        GameState.IsGameComplete = true;
    }

    public void RestartGame () {
        GameState.IsGameComplete = false;
        SingletonManager.EventSystemInstance.OnGameRestart.Invoke ();
        StartCoroutine (RestartRound ());
    }
}
