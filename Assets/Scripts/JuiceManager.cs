using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceManager : MonoBehaviour
{
    public static float ShakeJuice = 0;
    public static float TimeFreezeJuice = 0;
    public float shakeJuiceFalloff = 3.4f;
    public float timeFreezeJuiceFalloff = 4f;

    [Range(0,1)]
    public float goalJuice = 1f;
    [Range(0, 1)]
    public float paddleSoftJuice = 0.3f;
    [Range(0, 1)]
    public float paddleHardJuice = 0.5f;
    [Range(0, 1)]
    public float ballExplodeJuice = 0.5f;

    public static void AddShake (float shake) {
        ShakeJuice = Mathf.Clamp01 (ShakeJuice + shake);
    }

    public static void AddTimeFreeze (float freeze) {
        TimeFreezeJuice = Mathf.Clamp01 (TimeFreezeJuice + freeze);
    }

    private void Update () {
        ShakeJuice = Mathf.Clamp01 (ShakeJuice -= Time.deltaTime * shakeJuiceFalloff);
        TimeFreezeJuice = Mathf.Clamp01 (TimeFreezeJuice -= Time.deltaTime * timeFreezeJuiceFalloff);
    }

    private void OnEnable() {
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener(goalShake);
        SingletonManager.EventSystemInstance.OnPaddleHit.AddListener(paddleShake);
        //SingletonManager.EventSystemInstance.OnBallExplode.AddListener(ballExplodeShake);
    }

    private void OnDisable() {
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener(goalShake);
        SingletonManager.EventSystemInstance.OnPaddleHit.RemoveListener(paddleShake);
        //SingletonManager.EventSystemInstance.OnBallExplode.RemoveListener(ballExplodeShake);
    }

    private void goalShake(int team, Vector3 location) {
        AddShake(goalJuice);
    }

    private void paddleShake(bool isHard, Vector3 pos) {
        if (isHard == false) {
            AddShake(paddleSoftJuice);
        } else {
            AddShake(paddleHardJuice);
        }
    }
    /*
    private void ballExplodeShake(Vector3 location) {
        AddShake(ballExplodeJuice);
    }*/
    
}
