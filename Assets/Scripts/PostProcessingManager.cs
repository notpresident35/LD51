using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour {

    private PostProcessProfile postProcessingProfile;
    private Bloom bloomComp;
    private ChromaticAberration chromAbComp;
    private LensDistortion lensDistortComp;

    private const float defaultBloomIntensity = 15f;
    private const float defaultChromAbIntensity = 0.264f;
    private const float defaultLensDistortIntensity = -12f;

    [SerializeField] private float goalBloomIntensity = 35f;
    [SerializeField] private float goalChromAbIntensity = 1f;
    [SerializeField] private float goalLensDistortIntensity = -40f;
    [SerializeField] private float paddleBloomIntensity = 20f;
    [SerializeField] private float paddleChromAbIntensity = 0.5f;
    [SerializeField] private float paddleLensDistortIntensity = -20f;

    [SerializeField] private AnimationCurve juiceEffectCurve;
    [SerializeField] private float juiceEffectSpeed = 0.5f;    

    private void OnEnable() {
        SingletonManager.EventSystemInstance.OnGoalHit.AddListener(goalJuice);
        SingletonManager.EventSystemInstance.OnPaddleHit.AddListener(paddleJuice);
    }

    private void OnDisable() {
        SingletonManager.EventSystemInstance.OnGoalHit.RemoveListener(goalJuice);
        SingletonManager.EventSystemInstance.OnPaddleHit.RemoveListener(paddleJuice);
    }

    private void Start() {
        postProcessingProfile = GetComponent<PostProcessVolume>().profile;
        if (postProcessingProfile.TryGetSettings<Bloom>(out var bloom)) {
            bloomComp = bloom;
            bloomComp.intensity.overrideState = true;
        }
        if (postProcessingProfile.TryGetSettings<ChromaticAberration>(out var chromAb)) {
            chromAbComp = chromAb;
            chromAbComp.intensity.overrideState = true;
        }
        if (postProcessingProfile.TryGetSettings<LensDistortion>(out var lensDistort)) {
            lensDistortComp = lensDistort;
            lensDistortComp.intensity.overrideState = true;
        }
    }

    private void goalJuice(int team, Vector3 loc) {
        StartCoroutine(LerpEffects(goalBloomIntensity,goalChromAbIntensity,goalLensDistortIntensity));
    }

    private void paddleJuice(Vector3 unused) {
        StartCoroutine(LerpEffects(paddleBloomIntensity, paddleChromAbIntensity, paddleLensDistortIntensity));
    }

    IEnumerator LerpEffects (float bloomIntensity, float chromAbIntensity, float lensDistortIntensity) {
        for (float i = 0; i < 1; i += Time.deltaTime * juiceEffectSpeed) {
            bloomComp.intensity.value = Mathf.Lerp(defaultBloomIntensity, bloomIntensity, juiceEffectCurve.Evaluate(i));
            chromAbComp.intensity.value = Mathf.Lerp(defaultChromAbIntensity, chromAbIntensity, juiceEffectCurve.Evaluate (i));
            lensDistortComp.intensity.value = Mathf.Lerp(defaultLensDistortIntensity, lensDistortIntensity, juiceEffectCurve.Evaluate (i));
            yield return null;
        }

        bloomComp.intensity.value = defaultBloomIntensity;
        chromAbComp.intensity.value = defaultChromAbIntensity;
        lensDistortComp.intensity.value = defaultLensDistortIntensity;

    }
}
