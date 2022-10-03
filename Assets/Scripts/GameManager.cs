using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float BeginWaitTime = 2;
    public float RestartWaitTime = 2;

    private void Awake () {
        // Load default game mode
        if (GameState.CurrentMode == null) {
            SetGameMode (Resources.Load (Statics.GameModeFilePathPrefix + "BaseGameMode") as GameMode);
        }
    }

    // Note: Because GameManager is later in script execution order, it adds this as a listener after all other listeners
    // This means that it can rely on the score being updated when that happens in another script before this is called
    // This is quite precarious, but it gets the job done
    void HandleGoalScored (int teamID, Vector3 pos) {
        GameState.IsBallActive = false;
        if (SingletonManager.TeamManagerInstance.Teams [teamID].Score >= Statics.WinningScore) {
            // This team wins!
            StopGame (teamID);
        } else {
            StartCoroutine (RestartRound ());
        }
    }

    private void Start () {
        StartCoroutine (StartRound ());
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener (HandleGoalScored);
    }

    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener (HandleGoalScored);
    }

    IEnumerator StartRound () {
        SingletonManager.EventSystemInstance.OnRoundRestart.Invoke ();
        yield return new WaitForSeconds (BeginWaitTime);
        SingletonManager.EventSystemInstance.OnRoundBegin.Invoke ();
        GameState.IsBallActive = true;
    }

    IEnumerator RestartRound () {
        yield return new WaitForSeconds (RestartWaitTime);
        SingletonManager.EventSystemInstance.OnRoundRestart.Invoke ();
        yield return new WaitForSeconds (BeginWaitTime);
        SingletonManager.EventSystemInstance.OnRoundBegin.Invoke ();
        GameState.IsBallActive = true;
    }

    void StopGame (int winTeam) {
        GameState.IsGameComplete = true;
    }

    public void RestartGame () {
        GameState.IsGameComplete = false;
        SingletonManager.EventSystemInstance.OnGameRestart.Invoke ();
        StartCoroutine (RestartRound ());
    }

    public void SetGameMode (GameMode newMode) {
        GameState.CurrentMode = newMode;
    }
}
