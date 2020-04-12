using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class optionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer m_master = null;
    [SerializeField] private Slider m_masterSlider = null;
    private void Start()
    {
        m_masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0);
    }
    public void ChangeGameVolume(float argVolume)
    {
        m_master.SetFloat("MasterVolume", argVolume);
        PlayerPrefs.SetFloat("MasterVolume", argVolume);
    }
}
