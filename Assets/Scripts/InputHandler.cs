using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerPrefHandler;

[Serializable]
public class InputHandler : MonoBehaviour
{
    [Tooltip ("This should be stored/read as a list of inputs")]
    [SerializeField]
    private static List<StringPlayerPrefData> InputKeycodePPDs;
}
