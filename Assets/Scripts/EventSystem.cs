using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystem : MonoBehaviour {

    // State events
    [Tooltip("Passes team that scored and location of ball that scored")]
    [HideInInspector] public UnityEvent<int, Vector3> OnGoalHit = new UnityEvent<int, Vector3> ();
    [Tooltip("Passes location of ball that was hit")]
    [HideInInspector] public UnityEvent<bool> OnPaddleHit = new UnityEvent<bool> ();
    [HideInInspector] public UnityEvent OnSettingsSaved;
    [Tooltip ("Called when the round begins")]
    [HideInInspector] public UnityEvent OnRoundBegin = new UnityEvent ();
    [Tooltip("Called when the round resets")]
    [HideInInspector] public UnityEvent OnRoundRestart = new UnityEvent ();
    [Tooltip ("Called when the game resets")]
    [HideInInspector] public UnityEvent OnGameRestart = new UnityEvent ();

    // VFX Handovers
    [HideInInspector] public UnityEvent<Vector3> OnBallExplode = new UnityEvent<Vector3> (); // Passes location of ball that exploded
}
