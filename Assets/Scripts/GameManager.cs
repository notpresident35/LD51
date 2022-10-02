using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float RestartTime = 2;

    void HandleGoalScored (int teamID, Vector3 pos) {
        TeamManager teamManager = SingletonManager.TeamManagerInstance;
        teamManager.ScorePoint (teamID);
        if (teamManager.Teams [teamID].Score >= Statics.WinningScore) {
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
    }

    void StopGame (int winTeam) {

    }

    void RestartGame () {
        SingletonManager.EventSystemInstance.OnGameRestart.Invoke ();
    }
}
