using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    // fileName is the name of a file in the SFX directory
    public static void PlaySound(string fileName)
    {
        AudioClip clip = (AudioClip)Resources.Load(Statics.AudioFilePathPrefix + fileName);
        AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, 0));
    }
}
