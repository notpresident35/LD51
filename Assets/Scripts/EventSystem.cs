using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystem : MonoBehaviour {

    // State events
    [HideInInspector] public UnityEvent<int, Vector3> OnGoalHit; // Passes team who was scored on and location of ball that scored
    [HideInInspector] public UnityEvent<Vector3> OnPaddleHit; // Passes location of ball that was hit
    [HideInInspector] public UnityEvent OnSettingsSaved;

    // VFX Handovers
    [HideInInspector] public UnityEvent<Vector3> OnBallExplode; // Passes location of ball that exploded
}
