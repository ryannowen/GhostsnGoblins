using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton_Sound : MonoBehaviour
{
    public static Singleton_Sound m_instance;

    [SerializeField] private Dictionary<string, AudioClip> m_audioClips = null;

    void Awake()
    {
        if (null == m_instance && this != m_instance)
            m_instance = this;
    }

    public void CreateAudioClip(string argAudioName, AudioClip argAudioClip)
    {
        if(m_audioClips.ContainsKey(argAudioName))
        {
            Debug.LogWarning("Cannot add audioclip due to one already existing with same name");
            return;
        }

        m_audioClips.Add(argAudioName, argAudioClip);
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
}
