using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimerUI : MonoBehaviour
{
    public TMP_Text text;
    public float CountdownLength = 1.5f;
    [SerializeField] SoundEffect tickSound;
    [SerializeField] SoundEffect dingSound;

    void StartCountDown() {
        StartCoroutine (Countdown ());
    }

    IEnumerator Countdown () {
        text.gameObject.SetActive (true);
        for (int i = 2; i >= 0; i--) {
            text.text = (i + 1).ToString ();
            AudioManager.PlaySound (tickSound.clip, tickSound.volume);
            yield return new WaitForSeconds(CountdownLength / 4);
        }
        text.text = "Go!";
        AudioManager.PlaySound (dingSound.clip, dingSound.volume);
        yield return new WaitForSeconds (CountdownLength / 4);
        text.gameObject.SetActive(false);
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnRoundRestart.AddListener (StartCountDown);
    }

    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnRoundRestart.RemoveListener (StartCountDown);
    }
}
