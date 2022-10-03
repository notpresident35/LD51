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
        Time.timeScale = 0.0f;
        //Time.fixedDeltaTime = 0.0f;
    }

    public static void Unpause()
    {
        timePaused = false;
        Time.timeScale = timeScale;
        //Time.fixedDeltaTime = ????;
    }

    public static void SetTimeScale(float newTimeScale)
    {
        timeScale = newTimeScale;
        // just reusing the code in unpause, it only runs if it's already unpaused anyway
        if (!timePaused) Unpause();
    }
}
