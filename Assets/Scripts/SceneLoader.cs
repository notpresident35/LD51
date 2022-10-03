using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadMainScene()
    {
        SceneManager.LoadScene(Statics.MainSceneFileName);
    }

    public static void LoadGameOverScreen(int winningTeamID)
    {
        // I"M SO SORRY THAT I WROTE THIS LINE OF CODE OH GOD OH FRICK
        string winningTeamName = winningTeamID == 0 ? "Team 1" : "Team 2";
        Debug.Log("LoadGameOverScreen called with name: " + winningTeamName);
        GameOverScreenWinnerAnnouncer.WinningTeamName = winningTeamName;
        SceneManager.LoadScene(Statics.GameOverSceneFileName);
    }
}
