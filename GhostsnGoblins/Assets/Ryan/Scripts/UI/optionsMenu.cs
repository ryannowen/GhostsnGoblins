using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class optionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer m_master = null;

    public void ChangeGameVolume(float argVolume)
    {
        m_master.SetFloat("MasterVolume", argVolume);
    }
}
