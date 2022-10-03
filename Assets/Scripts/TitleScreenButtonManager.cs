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
    public GameObject credits;
    public GameObject main;
    public GameObject settings;

    public void PlayPressed()
    {
        AudioManager.PlaySound(buttonSound.clip, buttonSound.volume);
        SceneLoader.LoadMainScene();
    }

    public void PlayAIPressed () {
        AudioManager.PlaySound (buttonSound.clip, buttonSound.volume);
        SceneLoader.LoadMainAIScene ();
    }

    public void SettingsPressed()
    {
        AudioManager.PlaySound(buttonSound.clip, buttonSound.volume);
        main.SetActive(false);
        settings.SetActive(true);
    }

    public void CreditsPressed()
    {
        AudioManager.PlaySound(buttonSound.clip, buttonSound.volume);
        main.SetActive(false);
        credits.SetActive(true);
        
    }

    public void BackPressed() {

        AudioManager.PlaySound(buttonSound.clip, buttonSound.volume);
        credits.SetActive(false);
        main.SetActive(true);

    }

}
