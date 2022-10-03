using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * In a hasty attempt to get anything done, I'm reusing this script for game over screen.
 * Sorry about the name.
 *  - Rose
 */

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
