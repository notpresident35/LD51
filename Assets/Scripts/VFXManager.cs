using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour {

    public GameObject ballExplosionPS;

    public static VFXManager Instance;

    private void Start() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        EventSystem.Instance.goalHit.AddListener(createBallExplosion);
    }

    private void createBallExplosion(int team, Vector3 coords) {
        Instantiate(ballExplosionPS, coords, Quaternion.identity);
    }

}
