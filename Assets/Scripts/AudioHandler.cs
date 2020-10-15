using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [Header("Game Music")] 
    public AudioSource [] levelMusic;

    public float fadeTime = 2f;
    public float songVolume = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var song in levelMusic)
        {
            song.volume = 0;
        }
    }

    public void PlaySong(int level)
    {
        if (!levelMusic[level - 1].isPlaying)
        {
            levelMusic[level-1].Play();
            StartCoroutine(StartFade(levelMusic[level-1], fadeTime, songVolume));
        }
    }
    
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public void StopSong(int level)
    {
        levelMusic[level-1].Stop();
    }

    public void FadeOutMusic(int level)
    {
        StartCoroutine(StartFade(levelMusic[level-1], fadeTime, 0f));
    }

    public void FadeInMusic(int level)
    {
        StartCoroutine(StartFade(levelMusic[level-1], fadeTime, songVolume));
    }
}
