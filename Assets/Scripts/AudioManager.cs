using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static GameObject oneMusicSourceObject;
    private static GameObject twoMusicSourceObject;
    private static AudioSource oneMusicSource;
    private static AudioSource twoMusicSource;
    private static GameObject sfxObject;
    private static AudioSource sfxSource;

    public List<AudioClip> musicClips;
    private int currentMusicIndex = 0;
    private int nextMusicIndex = 1;
    private bool isFading = false;
    private bool changingSong = false;
    [SerializeField] private float maxVolume = 0.2f;
    [SerializeField] private float fadeTime = 5;

    // fileName is the name of a file in the SFX directory
    public static void PlaySound(AudioClip clip, float volume = 1)
    {
        //AudioClip clip = (AudioClip)Resources.Load(Statics.AudioFilePathPrefix + fileName);
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.Play();
    }

    private void OnEnable() {
        sfxObject = new GameObject("SFX Object");
        sfxSource = sfxObject.AddComponent<AudioSource>();
        sfxSource.rolloffMode = AudioRolloffMode.Linear;
    }

    private void Start() {
        PlayMusic();
    }

    public void PlayMusic() {

        oneMusicSourceObject = new GameObject("Current Music");
        twoMusicSourceObject = new GameObject("Next Music");
        oneMusicSource = oneMusicSourceObject.AddComponent<AudioSource>();
        twoMusicSource = twoMusicSourceObject.AddComponent<AudioSource>();
        oneMusicSource.playOnAwake = false;
        twoMusicSource.playOnAwake = false;
        oneMusicSource.loop = false;
        twoMusicSource.loop = false;
        // oneMusicSource.outputAudioMixerGroup = ;
        // twoMusicSource.outputAudioMixerGroup = ;
        oneMusicSource.clip = musicClips[currentMusicIndex];
        twoMusicSource.clip = musicClips[nextMusicIndex];
        oneMusicSource.volume = 0f;
        twoMusicSource.volume = 0f;
        oneMusicSource.Play();

    }

    private void Update() {

        if (oneMusicSource.isPlaying == true && (oneMusicSource.clip.length - oneMusicSource.time) <= fadeTime) {

            if (isFading == false) {
                isFading = true;
                StartCoroutine(Fade(oneMusicSource, fadeTime, 0f));
            }
            if ((oneMusicSource.clip.length - oneMusicSource.time) <= 0.5f && changingSong == false) {
                changingSong = true;
                oneMusicSource.volume = 0f;
                oneMusicSource.Stop();
                currentMusicIndex = nextMusicIndex;
                if (nextMusicIndex + 1 >= musicClips.Count) {
                    nextMusicIndex = 0;
                } else {
                    nextMusicIndex += 1;
                }
                twoMusicSource.Play();
                oneMusicSource.clip = musicClips[nextMusicIndex];
                changingSong = false;
            }

        } else if (twoMusicSource.isPlaying == true && (twoMusicSource.clip.length - twoMusicSource.time) <= fadeTime) {

            if (isFading == false) {
                isFading = true;
                StartCoroutine(Fade(twoMusicSource, fadeTime, 0f));
            }
            if ((twoMusicSource.clip.length - twoMusicSource.time) <= 0.5f && changingSong == false) {
                changingSong = true;
                twoMusicSource.volume = 0f;
                twoMusicSource.Stop();
                currentMusicIndex = nextMusicIndex;
                if (nextMusicIndex + 1 >= musicClips.Count) {
                    nextMusicIndex = 0;
                } else {
                    nextMusicIndex += 1;
                }
                oneMusicSource.Play();
                twoMusicSource.clip = musicClips[nextMusicIndex];
                changingSong = false;
            }

        } else if (oneMusicSource.isPlaying == true && oneMusicSource.time < fadeTime) {

            if (isFading == false) {
                isFading = true;
                StartCoroutine(Fade(oneMusicSource, fadeTime, maxVolume));
            }

        } else if (twoMusicSource.isPlaying == true && twoMusicSource.time < fadeTime) {

            if (isFading == false) {
                isFading = true;
                StartCoroutine(Fade(twoMusicSource, fadeTime, maxVolume));
            }

        }

    }

    IEnumerator Fade(AudioSource audio, float duration, float targetVol) {
        float currentTime = 0f;
        float startVol = audio.volume;
        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVol, targetVol, currentTime / duration);
            yield return null;

        }
        isFading = false;
        yield break;
    }

}
