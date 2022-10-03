using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScreenWinnerAnnouncer : MonoBehaviour
{
    // "{WinningTeamName} Wins!" will display on the game over screen.
    public static string WinningTeamName = "team_name";

    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = WinningTeamName + " Wins!";
    }
}
