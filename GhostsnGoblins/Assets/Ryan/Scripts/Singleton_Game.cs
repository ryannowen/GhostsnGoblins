using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton_Game : MonoBehaviour
{
    public static Singleton_Game m_instance;

    [SerializeField] private int m_score;

    private void Awake()
    {
        if (null == m_instance && this != m_instance)
        {
            m_instance = this;

            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int argScore)
    {
        m_score += argScore;
        CheckHighScore();
    }

    private void CheckHighScore()
    {

    }

    public int GetScore()
    {
        return m_score;
    }
}
