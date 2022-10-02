using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void ScoreHandler (int teamID, Vector3 pos) {
        TeamManager teamManager = SingletonManager.Instance.GetComponentInChildren<TeamManager> ();
        teamManager.ScorePoint (teamID);
        if (teamManager.Teams [teamID].Score >= Statics.LosingScore) {
            // Other team wins!

        }
    }

    private void OnEnable () {
        SingletonManager.Instance.GetComponentInChildren<EventSystem> ().OnGoalHit.AddListener (ScoreHandler);
    }

    private void OnDisable () {
        SingletonManager.Instance.GetComponentInChildren<EventSystem> ().OnGoalHit.RemoveListener (ScoreHandler);
    }
}
