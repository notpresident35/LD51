using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour {

    private GameObject ballExplosionVFXPrefab;

    //private ObjectPool<GameObject> particlePool = new ObjectPool<GameObject>();

    private void Start() {
        ballExplosionVFXPrefab = Resources.Load (Statics.VFXFilePathPrefix + "BallExplosionVFX") as GameObject;
    }

    public void createBallExplosion(Vector3 coords) {
        Instantiate(ballExplosionVFXPrefab, coords, Quaternion.identity);
    }

    private void OnEnable () {
        SingletonManager.EventSystemInstance.OnBallExplode.AddListener (createBallExplosion);
    }

    private void OnDisable () {
        SingletonManager.EventSystemInstance.OnBallExplode.RemoveListener (createBallExplosion);
    }
}
