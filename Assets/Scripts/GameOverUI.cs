using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject UI;
    public TMP_Text teamNameText;

    [SerializeField] SoundEffect buttonSound;

    public void playSound() {
        AudioManager.PlaySound(buttonSound.clip, buttonSound.volume);
	}

    private void Update () {
        UI.SetActive (GameState.IsGameComplete);
        teamNameText.text = $"Player {GameState.WinTeam} Wins!";
    }
}
