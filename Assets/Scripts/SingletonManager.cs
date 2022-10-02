using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance;

    private void Awake () {
        if (Instance == null) {
            DontDestroyOnLoad (gameObject);
            Instance = this;
        } else {
            Destroy (gameObject);
            return;
        }
    }
}
