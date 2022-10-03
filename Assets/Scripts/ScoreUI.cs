using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public List<Text> ScoreTexts;

    void Refresh (int teamID, Vector3 pos) {
        for (int i = 0; i < ScoreTexts.Count; i++) {
            ScoreTexts [i].text = SingletonManager.TeamManagerInstance.Teams [i].Score.ToString ();
        }
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener(Refresh);
    }

    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener (Refresh);
    }
}
