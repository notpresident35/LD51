using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    private static float timeScale = 1.0f;
    // this is overriden if GameState is paused
    private static bool timePaused = false;
    
    public static void Pause()
    {
        timePaused = true;
    }

    public static void Unpause()
    {
        timePaused = false;
    }

    public static void SetTimeScale(float newTimeScale)
    {
        timeScale = newTimeScale;
    }

    private void Update () {
        if (timePaused) {
            Time.timeScale = 0.0f;
            return;
        }

        float currentTimeScale = timeScale * (1 - JuiceManager.TimeFreezeJuice);

        Time.timeScale = currentTimeScale;
    }
}
