using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public static List<CameraTracker> Trackers = new List<CameraTracker>();

    private void OnEnable () {
        Trackers.Add (this);
    }

    private void OnDisable () {
        Trackers.Remove (this);
    }
}
