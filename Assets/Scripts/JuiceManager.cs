using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceManager : MonoBehaviour
{
    public static float ShakeJuice;

    public static void AddShake (float shake) {
        ShakeJuice = Mathf.Clamp01(ShakeJuice + shake);
    }

    private void Update () {
        ShakeJuice = Mathf.Clamp01 (ShakeJuice -= Time.deltaTime);
    }
}
