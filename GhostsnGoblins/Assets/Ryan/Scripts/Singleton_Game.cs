﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class layerColObject
{
    public int obj1;
    public int obj2;
    public bool ignoreLayerCollision;
}

public class Singleton_Game : MonoBehaviour
{
    public static Singleton_Game m_instance;

    [SerializeField] private int m_playerLives = 3;
    [SerializeField] private int m_score = 0;
    [SerializeField] private int[] m_highScores = new int[3];
    [SerializeField] layerColObject[] layerColAry = null;
    [SerializeField] private Vector2 m_lastCheckPoint = new Vector2(0, 0);

    private void Awake()
    {
        if (null == m_instance && this != m_instance)
        {
            m_instance = this;

            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        setLayerCollisions();
    }

    // Start is called before the first frame update
    void Start() {
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

    private void setLayerCollisions()
    {
        foreach (layerColObject lObj in layerColAry)
        {
            Physics2D.IgnoreLayerCollision(lObj.obj1, lObj.obj2, lObj.ignoreLayerCollision);
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

    public int GetPlayerLives()
    {
        return m_playerLives;
    }

    public void SetPlayerLives(int argPlayerLives)
    {
        m_playerLives = argPlayerLives;
    }

    public void AddPlayerLives(int argPlayerLives)
    {
        m_playerLives += argPlayerLives;

        if (m_playerLives <= 0)
        {
            Debug.Log("RAN OUT OF LIVES");
        }
    }

    public void SetCheckPoint(Vector2 argCheckPointLocation)
    {
        m_lastCheckPoint = argCheckPointLocation;
    }

    public void MoveToCheckPoint(GameObject argPlayer)
    {
        argPlayer.transform.position = m_lastCheckPoint;
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
