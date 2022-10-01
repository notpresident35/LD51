using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystem : MonoBehaviour {

    public UnityEvent<int, Vector3> goalHit;
    public UnityEvent<Vector3> paddleHit;
    public UnityEvent settingsSet;

    public static EventSystem Instance;

    private void Start() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

}
