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
    [SerializeField] SoundEffect buttonSound;

    public void PlayPressed()
    {
        AudioManager.PlaySound(buttonSound.clip, buttonSound.volume);
        SceneLoader.LoadMainScene();
    }

    public void SettingsPressed()
    {
        AudioManager.PlaySound(buttonSound.clip, buttonSound.volume);
        Debug.Log("Settings button pressed in menu.");
    }

    public void CreditsPressed()
    {
        AudioManager.PlaySound(buttonSound.clip, buttonSound.volume);
        Debug.Log("Credits button pressed in menu.");
    }
}
