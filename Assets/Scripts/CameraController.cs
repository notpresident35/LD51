using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float WidthCompression;
    public float MinZoomSize;
    public float InitialZoomSize;
    public Vector2 EdgePadding;

    [SerializeField]
    private float smoothFactor; 
    // sorry for the magic number, hopefully hazel never reads this code
    private float smoothFactorRationalizationConstant = 0.006f;

    private Bounds lastBounds;

    Camera cam;

    private void Awake() {
        cam = GetComponentInChildren<Camera> ();
    }

    private void Start () {
        lastBounds = new Bounds (Vector3.zero, Vector3.one * InitialZoomSize);
    }

    void LateUpdate()
    {
        FocusOnBounds(GetTargetedBounds());
    }

    Bounds GetTargetedBounds() {
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
    
    
    Vector3 Interpolate(Vector3 a, Vector3 b)
    {
        return Vector3.Lerp(a, b, smoothFactorRationalizationConstant / smoothFactor);
    }

    void FocusOnBounds (Bounds bounds) {
        bounds.Expand (new Vector2 (-bounds.extents.x * cam.aspect * WidthCompression, 0));
        bounds.Expand (EdgePadding);
        bounds = new Bounds(Interpolate(lastBounds.center, bounds.center), Interpolate(lastBounds.size, bounds.size));

        // Position camera
        transform.position = bounds.center;

        // Size to focus on bounds
        cam.orthographicSize = Mathf.Max (bounds.extents.x, bounds.extents.y);
        lastBounds = bounds;
    }
}
