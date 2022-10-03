using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public Transform grid1;
    public Transform grid2;

    public float grid1dist;
    public float grid2dist;

    private void Update () {
        grid1.position = cam.position / grid1dist;
        grid2.position = cam.position / grid2dist;
    }
}
