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
    public float paddleJuice = 0.5f;
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
        TimeFreezeJuice = Mathf.Clamp01 (ShakeJuice -= Time.deltaTime * timeFreezeJuiceFalloff);
    }

    private void OnEnable() {
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener(goalShake);
        SingletonManager.EventSystemInstance.OnPaddleHit.AddListener(paddleSoftShake);
        //SingletonManager.EventSystemInstance.OnGoalHit.AddListener(goalShake);
        SingletonManager.EventSystemInstance.OnBallExplode.AddListener(ballExplodeShake);
    }

    private void OnDisable() {
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener(goalShake);
        SingletonManager.EventSystemInstance.OnPaddleHit.RemoveListener(paddleSoftShake);
        //SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener(goalShake);
        SingletonManager.EventSystemInstance.OnBallExplode.RemoveListener(ballExplodeShake);
    }

    private void goalShake(int team, Vector3 location) {
        AddShake(goalJuice);
    }

    private void paddleSoftShake(Vector3 location) {
        AddShake(paddleJuice);
    }

    private void paddleHardShake() {

    }

    private void ballExplodeShake(Vector3 location) {
        AddShake(ballExplodeJuice);
    }
    
}
