using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleAI : Paddle
{
    public float CurveballRerollTime = 2;
    public float DashingDistance = 9;
    public float StopZone = 0.3f;
    public AnimationCurve DashCooldownByDifficulty;
    public AnimationCurve ChargeCooldownByDifficulty;
    public AnimationCurve MoveSpeedModifierByDifficulty;
    public AnimationCurve CurveballFrequencyByDifficulty;

    float lastRolledForCurveTime;
    float initialMoveSpeed;
    float difficulty;
    float dashCooldownTimer;
    float chargeCooldownTimer;
    int curveDirCache;
    Transform ball;

    protected override void Awake () {
        base.Awake ();
        initialMoveSpeed = moveSpeed;
    }

    protected override void Config () {
        difficulty = PlayerPrefHandler.GetFloat (Statics.AIDifficultyPPD);
        moveSpeed = initialMoveSpeed * MoveSpeedModifierByDifficulty.Evaluate(difficulty);
    }

    protected override float HandleMovementInput () {
        if (Mathf.Abs (ball.position.y - transform.position.y) < StopZone) {
            return 0;
        }

        return Mathf.Sign (ball.position.y - transform.position.y);
    }

    protected override bool HandleDashInput () {
        if (dashCooldownTimer > Mathf.Epsilon) {
            dashCooldownTimer -= Time.deltaTime;
            return false;
        }

        if (Mathf.Abs (ball.position.y - transform.position.y) > DashingDistance) {
            dashCooldownTimer = DashCooldownByDifficulty.Evaluate (difficulty);
            return true;
        }

        return false;
    }

    protected override bool HandleChargingInput () {
        if (chargeCooldownTimer > Mathf.Epsilon) {
            chargeCooldownTimer -= Time.deltaTime;
            return false;
        }

        return true;
    }

    protected override void HandleHitBall (Collision2D ballCollision) {
        base.HandleHitBall (ballCollision);
        if (chargeCooldownTimer <= Mathf.Epsilon) {
            chargeCooldownTimer = ChargeCooldownByDifficulty.Evaluate (difficulty);
        }
    }


    protected override int HandleCurveDir () {
        if (Time.time - lastRolledForCurveTime > CurveballRerollTime) {
            lastRolledForCurveTime = Time.time;
            curveDirCache = Random.Range(0.0f, 1.0f) < CurveballFrequencyByDifficulty.Evaluate(difficulty) ? (Random.Range (0, 1) * 2) - 1 : 0;
        }
        return curveDirCache;
    }

    // Note: Must be rewritten to support multiple balls
    void Reset () {
        ball = FindObjectOfType<Ball> ().transform;
        chargeCooldownTimer = ChargeCooldownByDifficulty.Evaluate (difficulty);
    }

    protected override void OnEnable () {
        base.OnEnable ();
        SingletonManager.EventSystemInstance.OnRoundBegin.AddListener (Reset);
        SingletonManager.EventSystemInstance.OnSettingsSaved.AddListener (Config);
    }

    protected override void OnDisable () {
        base.OnDisable ();
        SingletonManager.EventSystemInstance.OnRoundBegin.RemoveListener (Reset);
        SingletonManager.EventSystemInstance.OnSettingsSaved.RemoveListener (Config);
    }
}
