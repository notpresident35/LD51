using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour {

    public GameObject ballExplosionPS;

    public static VFXManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

    }

    private void Start() {
        EventSystem.Instance.OnBallExplode.AddListener(createBallExplosion);
    }

    public void createBallExplosion(Vector3 coords) {
        Instantiate(ballExplosionPS, coords, Quaternion.identity);
    }

}
