using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton_Sound : MonoBehaviour
{
    public static Singleton_Sound m_instance;

    [SerializeField] private Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();

    [SerializeField] private GameObject audioSourcePrefab = null;

    [SerializeField] private AudioClip[] createdSounds = null;

    private AudioSource mainAudioSource = null;

    void Awake()
    {
        if (null == m_instance && this != m_instance)
            m_instance = this;

        mainAudioSource = this.GetComponent<AudioSource>();

        if (mainAudioSource == null) {
            print("Couldn't find AudioSource component!");
        }

        foreach (AudioClip aClip in createdSounds) {
            Singleton_Sound.m_instance.CreateAudioClip(aClip, false);
        }
    }

    public void CreateAudioClip(AudioClip argAudioClip, bool shouldShowWarnings)
    {
        if(m_audioClips.ContainsKey(argAudioClip.name))
        {
            if (shouldShowWarnings)
            {
                Debug.LogWarning("Cannot add audioclip due to one already existing with same name");
            }
            
            return;
        }

        m_audioClips.Add(argAudioClip.name, argAudioClip);
    }

    public AudioClip GetAudioClip(string argAudioName)
    {
        if(!m_audioClips.ContainsKey(argAudioName))
        {
            Debug.LogWarning("Audioclip not created, cannot return instance");
            return null;
        }

        return m_audioClips[argAudioName];
    }

    public AudioSource PlayAudioClip(string argAudioName, float argVolume = 1.0f)
    {
        AudioSource audioSource = System_Spawn.instance.GetObjectFromPool(audioSourcePrefab, true).GetComponent<AudioSource>();
        audioSource.clip = GetAudioClip(argAudioName);
        audioSource.volume = argVolume;
        audioSource.Play();

        return audioSource;
    }
    public void fadeOutSound(float fadingSpeed) {
        StartCoroutine(fadeOutSoundIE(fadingSpeed));
    }

    private IEnumerator fadeOutSoundIE(float fadeSpeed) {
        while (mainAudioSource.volume > 0) {
            if (mainAudioSource.isPlaying) {
                mainAudioSource.volume -= 0.001f * fadeSpeed;

                if (mainAudioSource.volume < 0) {
                    mainAudioSource.volume = 0;
                }
                yield return null;
            }
        }

        mainAudioSource.Stop();
    }

    public void fadeInSound(float fadingSpeed, float volumeDest) {
        mainAudioSource.volume = 0;
        mainAudioSource.Play();

        StartCoroutine(fadeInSoundIE(fadingSpeed, volumeDest));
    }

    private IEnumerator fadeInSoundIE(float fadeSpeed, float vDest) {
        while (mainAudioSource.volume < vDest) {
            mainAudioSource.volume += 0.001f * fadeSpeed;

            if (mainAudioSource.volume > vDest) {
                mainAudioSource.volume = vDest;
            }
            yield return null;
        }
    }

    public void transitionToDifferentSound(string argAudioName, float fadingSpeedOut, float fadingSpeedIn, float volumeDest) {
        fadeOutSound(fadingSpeedOut);
        mainAudioSource.clip = GetAudioClip(argAudioName);
        fadeInSound(fadingSpeedIn, volumeDest);
    }
}
