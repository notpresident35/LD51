using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float WidthCompression;
    public float MinZoomSize;
    public Vector2 EdgePadding;
    public float ShakeAmplitude;
    public float ShakeFrequency;

    [SerializeField]
    private float smoothFactor; 
    // sorry for the magic number, hopefully hazel never reads this code
    // i'm also incredibly sorry about this variable name
    private float smoothFactorReasonabilityConstant = 0.006f;
    [SerializeField]
    private float horizontalWiggleFactor;
    
    // the actual amplitude: this gets lerped with shakeAmplitude
    private float intermediateShakeAmplitude;
    [SerializeField]
    private float perlinNoisePosition = 0.0f;

    private Bounds lastBounds;

    Camera cam;

    private void Awake() {
        cam = GetComponentInChildren<Camera> ();
    }

    void LateUpdate()
    {
        perlinNoisePosition += ShakeFrequency;
        if (perlinNoisePosition > 10000) perlinNoisePosition = 0;
        FocusOnBounds(GetTargetedBounds());
    }

    float GetNoiseFactor(float offset)
    {
        return 1.0f + GameState.ShakeJuice * ShakeAmplitude * Mathf.PerlinNoise(perlinNoisePosition + offset, 0.0f);
    }

    float GetNoiseAdder(float offset)
    {
        return GameState.ShakeJuice * ShakeAmplitude * (-0.5f + Mathf.PerlinNoise(perlinNoisePosition + offset, 0.0f));
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
    
    
    void FocusOnBounds (Bounds bounds) {
        bounds.Expand (new Vector2 (-bounds.extents.x * cam.aspect * WidthCompression, 0));
        bounds.Expand (EdgePadding);
        // lerpity lerpity
        Vector3 newCenter = Vector3.Lerp(lastBounds.center, bounds.center, smoothFactorReasonabilityConstant / smoothFactor);
        Vector3 newSize = Vector3.Lerp(lastBounds.size, bounds.size, smoothFactorReasonabilityConstant / smoothFactor);

        lastBounds = new Bounds(newCenter, newSize);
        
        newSize *= GetNoiseFactor(0.0f);
        newCenter.x += horizontalWiggleFactor * GetNoiseAdder(100.0f);

        bounds = new Bounds(newCenter, newSize);

        // Position camera
        transform.position = bounds.center;

        // Size to focus on bounds
        cam.orthographicSize = Mathf.Max (bounds.extents.x, bounds.extents.y);
    }
}
