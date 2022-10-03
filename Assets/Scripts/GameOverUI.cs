using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject UI;
    public TMP_Text teamNameText;

    private void Update () {
        UI.SetActive (GameState.IsGameComplete);
        teamNameText.text = "Player 1 Wins";
    }
}
