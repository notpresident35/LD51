using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystem : MonoBehaviour {

    // State events
    public UnityEvent<int, Vector3> OnGoalHit; // Passes team who was scored on and location of ball that scored
    public UnityEvent<Vector3> OnPaddleHit; // Passes location of ball that was hit
    public UnityEvent OnSettingsSaved;

    // VFX Handovers
    public UnityEvent<Vector3> OnBallExplode; // Passes location of ball that exploded
}
