using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour {

    private GameObject ballExplosionVFXPrefab;

    //private ObjectPool<GameObject> particlePool = new ObjectPool<GameObject>();

    private void Start() {
        ballExplosionVFXPrefab = Resources.Load (Statics.EffectsFilePathPrefix + "BallExplosionVFX") as GameObject;
    }

    public void createBallExplosion(Vector3 coords) {
        Instantiate(ballExplosionVFXPrefab, coords, Quaternion.identity);
    }

    private void OnEnable () {
        SingletonManager.Instance.GetComponentInChildren<EventSystem> ().OnBallExplode.AddListener (createBallExplosion);
    }

    private void OnDisable () {
        SingletonManager.Instance.GetComponentInChildren<EventSystem> ().OnBallExplode.RemoveListener (createBallExplosion);
    }
}
