using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayAfterTime : MonoBehaviour, ISpawn
{

    [SerializeField] private float m_TimeToDecay = 2f;
    [SerializeField] private bool m_ShouldDecay = false;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {

        timer = m_TimeToDecay;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (m_ShouldDecay)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = m_TimeToDecay;
                m_ShouldDecay = false;
                gameObject.SetActive(false);
            }
        }

    }

    // ISpawn

    public void OnSpawn()
    {

        timer = m_TimeToDecay;
        m_ShouldDecay = true;

    }

    public void OnDeSpawn()
    {

    }

}
