using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerPrefHandler;
/*
[Serializable]
public class InputKeycodePlayerPrefData : PlayerPrefData<KeyCode> {
    public string HandlerNamePrefix;
    public InputKeycodePlayerPrefData (string name, KeyCode defaultVal) : base (name, defaultVal) {
    }
}*/

[Serializable]
public class InputHandler : MonoBehaviour
{
    //[SerializeField]
    //private string InputHandlerName;
    [Tooltip ("This should be stored/read as a list of inputs")]
    [SerializeField]
    private List<KeycodePlayerPrefData> InputKeycodePPDs;

    public KeyCode GetKeycodeForInput (string inputName) {
        foreach (KeycodePlayerPrefData p in InputKeycodePPDs) {
            if (p.Name == inputName) {
                return GetKeycode (p);
            }
        }
        Debug.LogError ("Failed to get keycode!");
        return KeyCode.Home;
    }
}