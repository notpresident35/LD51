using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour {

    private PostProcessProfile postProcessingProfile;

    private void OnEnable() {
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener(goalJuice);
    }

    private void OnDisable() {
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener(goalJuice);
    }

    private void Start() {
        postProcessingProfile = GetComponent<PostProcessVolume>().profile;
    }

    private void goalJuice(int team, Vector3 loc) {
        
    }

}
