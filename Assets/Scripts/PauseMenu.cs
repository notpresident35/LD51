using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    public void Start()
    {
        Resume();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Pause();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        TimeControl.Pause();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        TimeControl.Unpause();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
