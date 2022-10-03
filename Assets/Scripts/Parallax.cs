using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Vector3 grid2Offset = new Vector3 (0.2f, 0.5f, 0);
    public Transform cam;
    public Camera curCam;
    public Transform grid1;
    public Transform grid2;

    public float grid1parallaxing;
    public float grid2parallaxing;
    public float camBaselineZoom;

    private void Update () {
        grid1.localPosition = cam.position * -grid1parallaxing;
        grid2.localPosition = cam.position * -grid2parallaxing + grid2Offset;
    }
}
