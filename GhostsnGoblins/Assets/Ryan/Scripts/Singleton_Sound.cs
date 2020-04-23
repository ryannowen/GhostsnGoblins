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
    private bool hasFadedOut = false;

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

    public void PlayAudioClipOneShot(string argAudioName, float argVolume = 1.0f) {
        AudioSource audioSource = System_Spawn.instance.GetObjectFromPool(audioSourcePrefab, true).GetComponent<AudioSource>();
        audioSource.clip = null;
        audioSource.PlayOneShot(GetAudioClip(argAudioName), argVolume);
    }

    public AudioSource PlayAudioClip(string argAudioName, float argVolume = 1.0f)
    {
        AudioSource audioSource = System_Spawn.instance.GetObjectFromPool(audioSourcePrefab, true).GetComponent<AudioSource>();
        audioSource.clip = GetAudioClip(argAudioName);
        audioSource.volume = argVolume;
        audioSource.Play();

        return audioSource;
    }

    public void fadeOutSound(float fadingTimeDelay, string argAudioName = "") {
        hasFadedOut = false;
        StartCoroutine(fadeOutSoundIE(fadingTimeDelay, argAudioName));
    }

    private IEnumerator fadeOutSoundIE(float fTimeDelay, string argAudioName) {
        while (mainAudioSource.volume > 0.005f) {
            if (mainAudioSource.isPlaying) {
                mainAudioSource.volume -= 0.02f;
            }

            if (mainAudioSource.volume <= 0.005f) {
                mainAudioSource.Stop();
                mainAudioSource.volume = 0.005f;

                if (argAudioName != "") {
                    mainAudioSource.clip = GetAudioClip(argAudioName);
                    mainAudioSource.Play(); 
                }

                hasFadedOut = true;
                yield break;
            }

            yield return new WaitForSeconds(fTimeDelay);
        }
    }

    public void fadeInSound(float fadingTimeDelay, float volumeDest) {
        StartCoroutine(fadeInSoundIE(fadingTimeDelay, volumeDest));
    }

    private IEnumerator fadeInSoundIE(float fTimeDelay, float volumeDest) {
        while (mainAudioSource.volume < volumeDest ) {
            if (hasFadedOut) {
                mainAudioSource.volume += 0.02f;

                if (mainAudioSource.volume > volumeDest) {
                    mainAudioSource.volume = volumeDest;
                    hasFadedOut = false;
                    yield break;
                }
            }

            yield return new WaitForSeconds(fTimeDelay);
        }
    }

    public void transitionToDifferentSound(string argAudioName, float fadingTimeDelayOut, float fadingTimeDelayIn, float volumeDest) {
        fadeOutSound(fadingTimeDelayOut, argAudioName);
        fadeInSound(fadingTimeDelayIn, volumeDest);
    }
}
