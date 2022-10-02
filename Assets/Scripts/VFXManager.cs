using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour {

    private GameObject ballExplosionVFXPrefab;

    //private ObjectPool<GameObject> particlePool = new ObjectPool<GameObject>();

    private void Start() {
        SingletonManager.Instance.GetComponentInChildren<EventSystem> ().OnBallExplode.AddListener(createBallExplosion);

        ballExplosionVFXPrefab = Resources.Load ("VFX/BallExplosionVFX") as GameObject;
    }

    public void createBallExplosion(Vector3 coords) {
        Instantiate(ballExplosionVFXPrefab, coords, Quaternion.identity);
    }

}
