using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider MasterVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider MusicVolumeSlider;

    public void Start () {
        Load ();
    }

    public void Load () {
        MasterVolumeSlider.value = PlayerPrefHandler.GetFloat (Statics.AudioMasterVolumePPD);
        SFXVolumeSlider.value = PlayerPrefHandler.GetFloat (Statics.AudioSFXVolumePPD);
        MusicVolumeSlider.value = PlayerPrefHandler.GetFloat (Statics.AudioMusicVolumePPD);

        // TODO: Player inputs!
    }

    public void Save () {
        PlayerPrefHandler.SetFloat (Statics.AudioMasterVolumePPD, MasterVolumeSlider.value);
        PlayerPrefHandler.SetFloat (Statics.AudioSFXVolumePPD, SFXVolumeSlider.value);
        PlayerPrefHandler.SetFloat (Statics.AudioMusicVolumePPD, MusicVolumeSlider.value);

        // TODO: Player inputs!
    }
}
