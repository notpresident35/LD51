using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystem : MonoBehaviour {

    // State events
    public UnityEvent<int, Vector3> OnGoalHit;
    public UnityEvent<Vector3> OnPaddleHit;
    public UnityEvent OnSettingsSaved;

    // VFX Handovers
    public UnityEvent<Vector3> OnBallExplode;

    public static EventSystem Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }
}
