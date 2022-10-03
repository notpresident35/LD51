using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenButtonManager : MonoBehaviour
{
    public void PlayPressed()
    {
        SceneLoader.LoadMainScene();
    }

    public void SettingsPressed()
    {
        Debug.Log("Settings button pressed in menu.");
    }

    public void CreditsPressed()
    {
        Debug.Log("Credits button pressed in menu.");
    }
}
