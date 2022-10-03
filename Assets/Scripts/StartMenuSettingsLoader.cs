using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuSettingsLoader : MonoBehaviour
{
    public AudioSource source;

    private void Awake () {
        source.volume = PlayerPrefHandler.GetFloat (Statics.AudioMusicVolumePPD) * PlayerPrefHandler.GetFloat (Statics.AudioMasterVolumePPD);
    }
}
