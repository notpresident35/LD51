using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    float val = PlayerPrefHandler.GetFloat (Statics.AudioMasterVolumePPD);
}
