using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance;
    public static EventSystem EventSystemInstance;
    public static TeamManager TeamManagerInstance;

    private void Awake () {
        if (Instance == null) {
            DontDestroyOnLoad (gameObject);
            Instance = this;
        } else {
            Destroy (gameObject);
            return;
        }

        EventSystemInstance = Instance.GetComponentInChildren<EventSystem> ();
        TeamManagerInstance = Instance.GetComponentInChildren<TeamManager> ();
    }
}
