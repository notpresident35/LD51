using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField] SoundEffect buttonSound;

    public void Start()
    {
        Resume();
    }

    public void Update()
    {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            if (!GameState.Paused) {
                Pause ();
            } else {
                Resume ();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        TimeControl.Pause();
        GameState.Paused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        TimeControl.Unpause();
        GameState.Paused = false;
        AudioManager.PlaySound(buttonSound.clip,buttonSound.volume);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
