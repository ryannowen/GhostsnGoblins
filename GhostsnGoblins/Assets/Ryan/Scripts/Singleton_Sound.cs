using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton_Sound : MonoBehaviour
{
    public static Singleton_Sound m_instance;

    [SerializeField] private Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();

    [SerializeField] private GameObject audioSourcePrefab = null;

    [SerializeField] private AudioClip[] createdSounds = null;

    void Awake()
    {
        if (null == m_instance && this != m_instance)
            m_instance = this;
    }

    void Start() {
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
        audioSource.PlayOneShot(GetAudioClip(argAudioName), argVolume);

        return audioSource;
    }
}
