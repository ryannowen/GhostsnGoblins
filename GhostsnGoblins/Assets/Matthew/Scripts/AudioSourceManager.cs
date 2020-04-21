using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour, ISpawn
{

    public AudioSource m_AudioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ISpawn
    public void OnSpawn()
    {
        
        if (m_AudioSource = null)
            m_AudioSource = this.gameObject.GetComponent<AudioSource>();

        if (m_AudioSource != null)
        {
            m_AudioSource.volume = 1.0f;
            m_AudioSource.loop = false;
            m_AudioSource.clip = null;
        }

    }

    public void OnDeSpawn()
    {



    }

}
