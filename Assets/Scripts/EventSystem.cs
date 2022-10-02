using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystem : MonoBehaviour {

    // State events
    [Tooltip("Passes team that scored and location of ball that scored")]
    [HideInInspector] public UnityEvent<int, Vector3> OnGoalHit;
    [Tooltip("Passes location of ball that was hit")]
    [HideInInspector] public UnityEvent<Vector3> OnPaddleHit;
    [HideInInspector] public UnityEvent OnSettingsSaved;
    [Tooltip("Called when the round resets (note that this is *not* when play begins)")]
    [HideInInspector] public UnityEvent OnRoundRestart;
    [Tooltip ("Called when the game resets (note that this is *not* when play begins)")]
    [HideInInspector] public UnityEvent OnGameRestart;

    // VFX Handovers
    [HideInInspector] public UnityEvent<Vector3> OnBallExplode; // Passes location of ball that exploded
}
