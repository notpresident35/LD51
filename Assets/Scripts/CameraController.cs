using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float WidthCompression;
    public float MinZoomSize;
    public Vector2 EdgePadding;

    Camera cam;

    private void Awake () {
        cam = GetComponentInChildren<Camera> ();
    }

    void Update()
    {
        FocusOnBounds (GetTargetedBounds ());
    }

    Bounds GetTargetedBounds () {
        // Focus on center position with minimum zoom
        Vector3 centerPos = Vector3.zero;
        foreach (CameraTracker trackedObj in CameraTracker.Trackers) {
            centerPos += trackedObj.transform.position;
        }
        centerPos /= CameraTracker.Trackers.Count;

        // Find bounds around camera trackers, with minimum camera size
        Bounds bounds = new Bounds(centerPos, Vector3.one * MinZoomSize);
        foreach (CameraTracker trackedObj in CameraTracker.Trackers) {
            bounds.Encapsulate (trackedObj.transform.position);
        }
        return bounds;
    }

    void FocusOnBounds (Bounds bounds) {
        bounds.Expand (new Vector2 (-bounds.extents.x * cam.aspect * WidthCompression, 0));
        bounds.Expand (EdgePadding);

        // Position camera
        transform.position = bounds.center;

        // Size to focus on bounds
        cam.orthographicSize = Mathf.Max (bounds.extents.x, bounds.extents.y);
    }
}
