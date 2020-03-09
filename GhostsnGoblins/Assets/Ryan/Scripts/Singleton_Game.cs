using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton_Game : MonoBehaviour
{
    public static Singleton_Game m_instance;

    [SerializeField] private int m_score;
    [SerializeField] private int[] m_highScores = new int[3];
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
        LoadGame();
    }

    public void AddScore(int argScore)
    {
        m_score += argScore;
        CheckHighScore();
    }

    private void CheckHighScore()
    {
        for(int i = 0; i < m_highScores.Length; i++)
        {
            if(m_score > m_highScores[i])
            {
                m_highScores[i] = m_score;
                PlayerPrefs.SetInt("m_highScores_" + i, m_score);
                break;
            }
        }
    }

    public int GetScore()
    {
        return m_score;
    }
    
    public int GetHighScore(int argHighScore)
    {
        if (argHighScore > m_highScores.Length)
            return 0;

        return m_highScores[argHighScore];
    }

    public void LoadGame()
    {
        for (int i = 0; i < m_highScores.Length; i++)
        {
            m_highScores[i] = PlayerPrefs.GetInt("m_highScores_" + i);
        }
    }

    public void SaveGame()
    { 
            
    }
}
