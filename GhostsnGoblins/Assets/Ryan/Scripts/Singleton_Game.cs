using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private GameObject m_HUDPrefab = null;
    [SerializeField] private int m_insertedMoney = 0;
    [SerializeField] private int m_requiredPenceToStartGame = 100;
    [SerializeField] private int m_requiredPenceToSpawnPlayer2 = 100;
    [SerializeField] private int m_requiredPenceToBuyLife = 100;
    [SerializeField] private bool m_canStartGame = false;
    [SerializeField] private bool m_spawnedPlayer2 = false;
    [Space]
    [SerializeField] private int m_playerLives = 3;
    [Space]
    [SerializeField] private int m_score = 0;
    [SerializeField] private int[] m_highScores = null;
    [SerializeField] private GameObject m_scorePopupPrefab = null;
    [Space]
    [SerializeField] layerColObject[] layerColAry = null;
    [Space]
    [SerializeField] private Vector2 m_lastCheckPoint = new Vector2(0, 0);
    [SerializeField] private bool m_showLevelDoorItem = false;
    [SerializeField] private string m_previousLevelName = "Level1_heaven";

    private Dictionary<int, GameObject> m_registeredPlayers = new Dictionary<int, GameObject>();
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

    public void AddScore(int argScore, Vector2 argScorePopupPosition, float argScorePopupDisplayTime = 1.5f)
    {
        AddScore(argScore);

        GameObject scorePopupObject = System_Spawn.instance.GetObjectFromPool(m_scorePopupPrefab);
        StartCoroutine(scorePopupObject.GetComponent<ScorePopup>().ActivatePopup(argScorePopupPosition, argScore, argScorePopupDisplayTime));
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

    public void InsertMoney(int argMoney)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        m_insertedMoney += argMoney;

        if ("MenuScene" == currentScene.name) // is mainmennu
        {
            if (m_insertedMoney >= m_requiredPenceToStartGame) // Start game button
            {
                m_canStartGame = true;
                m_insertedMoney = 0;
            }
        }
        else if("death" == currentScene.name) // is death scene
        {
            if(m_insertedMoney >= m_requiredPenceToBuyLife) // load previous scene
            {
                m_insertedMoney = 0;
                m_playerLives += 1;
                System_Spawn.instance.DisableAllSpawns();
                LoadPreviousScene();
            }
        }
        else if(!m_spawnedPlayer2)
        {
            if (m_insertedMoney >= m_requiredPenceToSpawnPlayer2) // Spawn Player 2
            {
                SpawnPlayer2();
                m_insertedMoney = 0;
            }
        }
    }

    private void SpawnPlayer2()
    {
        GameObject player2 = GetPlayer(1);
        player2.GetComponent<ISpawn>().OnSpawn();
        player2.transform.position = GetPlayer(0).transform.position;
        player2.SetActive(true);

        m_spawnedPlayer2 = true;
    }

    public void ReSpawnPlayerAtCheckpoint(int argPlayerID)
    {
        if (1 == argPlayerID && !m_spawnedPlayer2)
            return;

        GameObject player2 = GetPlayer(argPlayerID);
        if(player2.GetComponent<PlayerController>().GetHealth() <= 0)
            player2.GetComponent<ISpawn>().OnSpawn();

        player2.transform.position = m_lastCheckPoint;
        player2.SetActive(true);
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
            System_Spawn.instance.DisableAllSpawns();
            m_previousLevelName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("death");
        }
    }

    public void CheckPlayersAlive()
    {
        GameObject player1 = GetPlayer(0);
        GameObject player2 = GetPlayer(1);

        if (!player1.activeSelf && (m_spawnedPlayer2 ? !player2.activeSelf : true))
        {
            player1.SetActive(true);
            player1.transform.position = m_lastCheckPoint;
            player1.GetComponent<PlayerController>().OnSpawn();
        }

        if(m_spawnedPlayer2 && !player2.activeSelf && !player1.activeSelf)
        { 
            player2.SetActive(true);
            player2.transform.position = m_lastCheckPoint;
            player2.GetComponent<PlayerController>().OnSpawn();
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

    public GameObject GetHUD()
    {
        return m_HUDPrefab;
    }

    public bool GetCanStartGame()
    {
        return m_canStartGame;
    }

    public void ResetGame()
    {
        m_score = 0;
        m_playerLives = 0;
        m_insertedMoney = 0;
        m_canStartGame = false;
        m_spawnedPlayer2 = false;
        m_showLevelDoorItem = false;
        m_previousLevelName = "Level1_heaven";
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

    public void RegisterPlayer(int argID, GameObject argPlayer)
    {
        m_registeredPlayers.Add(argID - 1, argPlayer);
    }


    public GameObject GetPlayer(int argID)
    {
        if(m_registeredPlayers.Count < argID)
        {
            Debug.LogError("Cannot return player as ID is greater than registered Players (" + m_registeredPlayers.Count + ") given ID=" + argID);
            return null;
        }

        return m_registeredPlayers[argID];
    }

    public void SetShowLevelDoorItem(bool argShowLevelDoorItem)
    {
        m_showLevelDoorItem = argShowLevelDoorItem;
    }

    public bool GetShowLevelDoorItem()
    {
        return m_showLevelDoorItem;
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(m_previousLevelName);
    }
    public void SetPreviousScene(string argPreviousLevelName)
    {
        m_previousLevelName = argPreviousLevelName;
    }
}
