using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider MasterVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider MusicVolumeSlider;
    public Toggle DedicatedChargeToggleP1;
    public Toggle DedicatedChargeToggleP2;
    public Slider AIDifficultySlider;

    public void Start () {
        Load ();
    }

    public void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            if (!GameState.Paused) {
                Save ();
                gameObject.SetActive (false);
            }
        }
    }


    public void Load () {
        MasterVolumeSlider.value = PlayerPrefHandler.GetFloat (Statics.AudioMasterVolumePPD);
        SFXVolumeSlider.value = PlayerPrefHandler.GetFloat (Statics.AudioSFXVolumePPD);
        MusicVolumeSlider.value = PlayerPrefHandler.GetFloat (Statics.AudioMusicVolumePPD);
        AIDifficultySlider.value = PlayerPrefHandler.GetFloat (Statics.AIDifficultyPPD);

        DedicatedChargeToggleP1.isOn = PlayerPrefHandler.GetBool (Statics.DedicatedChargeP1PPD);
        DedicatedChargeToggleP2.isOn = PlayerPrefHandler.GetBool (Statics.DedicatedChargeP2PPD);
    }

    public void Save () {
        PlayerPrefHandler.SetFloat (Statics.AudioMasterVolumePPD, MasterVolumeSlider.value);
        PlayerPrefHandler.SetFloat (Statics.AudioSFXVolumePPD, SFXVolumeSlider.value);
        PlayerPrefHandler.SetFloat (Statics.AudioMusicVolumePPD, MusicVolumeSlider.value);
        PlayerPrefHandler.SetFloat (Statics.AIDifficultyPPD, AIDifficultySlider.value);

        PlayerPrefHandler.SetBool (Statics.DedicatedChargeP1PPD, DedicatedChargeToggleP1.isOn);
        PlayerPrefHandler.SetBool (Statics.DedicatedChargeP2PPD, DedicatedChargeToggleP2.isOn);

        if (SingletonManager.EventSystemInstance) {
            SingletonManager.EventSystemInstance.OnSettingsSaved.Invoke();
        }
    }
}
