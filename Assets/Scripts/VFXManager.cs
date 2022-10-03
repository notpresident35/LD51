using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour {

    private GameObject ballExplosionVFXPrefab;
    private GameObject paddleHitVFXPrefab;
    private GameObject paddleHitHardVFXPrefab;

    //private ObjectPool<GameObject> particlePool = new ObjectPool<GameObject>();

    private void Start() {
        ballExplosionVFXPrefab = Resources.Load (Statics.VFXFilePathPrefix + "BallExplosionVFX") as GameObject;
        paddleHitVFXPrefab = Resources.Load (Statics.VFXFilePathPrefix + "PaddleHitVFX") as GameObject;
        paddleHitHardVFXPrefab = Resources.Load (Statics.VFXFilePathPrefix + "PaddleHitHardVFX") as GameObject;
    }

    public void createBallExplosion(Vector3 coords) {
        Instantiate(ballExplosionVFXPrefab, coords, Quaternion.identity);
    }

    public void createPaddleHit (bool hardHit, Vector3 coords) {
        Instantiate (hardHit ? paddleHitHardVFXPrefab : paddleHitVFXPrefab, coords, Quaternion.identity);
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnPaddleHit.AddListener (createPaddleHit);
        SingletonManager.EventSystemInstance.OnBallExplode.AddListener (createBallExplosion);
    }

    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnPaddleHit.RemoveListener (createPaddleHit);
        SingletonManager.EventSystemInstance.OnBallExplode.RemoveListener (createBallExplosion);
    }
}
