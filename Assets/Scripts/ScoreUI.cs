using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private List<TMP_Text> ScoreTexts;

    private void Awake () {
        ScoreTexts = GetComponentsInChildren<TMP_Text> ().ToList();        
    }

    void Refresh (int teamID, Vector3 pos) {
        for (int i = 0; i < ScoreTexts.Count; i++) {
            ScoreTexts [i].text = SingletonManager.TeamManagerInstance.Teams [i].Score.ToString ();
        }
    }

    void EnableUI (int teamID, Vector3 pos) {
        for (int i = 0; i < ScoreTexts.Count; i++) {
            ScoreTexts [i].gameObject.SetActive (true);
        }
    }
    
    void DisableUI () {
        for (int i = 0; i < ScoreTexts.Count; i++) {
            ScoreTexts [i].gameObject.SetActive (false);
        }
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener(Refresh);
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener(EnableUI);
        SingletonManager.EventSystemInstance.OnRoundBegin.AddListener(DisableUI);
    }

    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener (Refresh);
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener (EnableUI);
        SingletonManager.EventSystemInstance.OnRoundBegin.RemoveListener (DisableUI);
    }
}
