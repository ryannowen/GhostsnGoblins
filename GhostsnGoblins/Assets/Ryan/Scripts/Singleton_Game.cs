using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[System.Serializable]
public class layerColObject
{
    public int obj1;
    public int obj2;
    public bool ignoreLayerCollision;
}

public class Singleton_Game : MonoBehaviour
{
    public enum EGameStat
    {
        eDeaths,
        EKills,
        EPickups,
        eLivesGained
    }

    [System.Serializable]
    public struct SHighScore
    {
        public string m_initials;
        public int m_score;

        public SHighScore(string argInitials = "ERR", int argScore = 0)
        {
            m_initials = argInitials;
            m_score = argScore;
        }
    };

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
    [SerializeField] private int m_requiredScoreForLife = 10000;
    [SerializeField] private SHighScore[] m_highScores = null;
    [SerializeField] private bool m_isNewHighScore = false;
    [SerializeField] private GameObject m_scorePopupPrefab = null;
    [Space]
    [SerializeField] private Vector2 m_lastCheckPoint = new Vector2(0, 0);
    [SerializeField] private bool m_showLevelDoorItem = false;
    [SerializeField] private string m_previousLevelName = "Level1_heaven";
    [SerializeField] private int m_livesScore = 0;
    [Space]
    [SerializeField] private AudioMixer m_masterMixer = null;

    [SerializeField] Dictionary<EGameStat, int> m_gameStats = new Dictionary<EGameStat, int>();
    private AudioSource mainAudioSource;

    private GameObject m_camera;

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

        mainAudioSource = this.GetComponent<AudioSource>();

        if (mainAudioSource == null) {
            print("Couldn't find AudioSource component!");
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start() 
    {
        LoadGame();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        switch (scene.buildIndex) {
            case 0:
                mainAudioSource.clip = Singleton_Sound.m_instance.GetAudioClip("MainMenu");
                mainAudioSource.loop = true;
                mainAudioSource.volume = 1;
                mainAudioSource.Play();
                break;

            case 1:
                mainAudioSource.clip = Singleton_Sound.m_instance.GetAudioClip("GameOver");
                mainAudioSource.loop = false;
                mainAudioSource.volume = 0.5f;
                mainAudioSource.Play();
                break;

            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
                mainAudioSource.clip = Singleton_Sound.m_instance.GetAudioClip("FullLevelBGM");
                mainAudioSource.loop = true;
                mainAudioSource.volume = 0.1f;
                mainAudioSource.Play();
                break;

            case 9:
                mainAudioSource.clip = Singleton_Sound.m_instance.GetAudioClip("Victory");
                mainAudioSource.loop = false;
                mainAudioSource.volume = 1;
                mainAudioSource.Play();
                break;

            case 10:
                mainAudioSource.clip = Singleton_Sound.m_instance.GetAudioClip("BossBattle");
                mainAudioSource.loop = true;
                mainAudioSource.volume = 1;
                mainAudioSource.Play();
                break;

            default:
                mainAudioSource.clip = null;
                mainAudioSource.loop = true;
                mainAudioSource.volume = 0;
                mainAudioSource.Stop();
                break;
        }
    }

    public void AddScore(int argScore)
    {
        m_score += argScore;
        m_livesScore += argScore;

        CheckHighScore();

        if (m_playerLives < 3) 
        {
            if (m_livesScore >= m_requiredScoreForLife) 
            {
                m_livesScore = 0;
                AddPlayerLives(1);
                AddGameStat(EGameStat.eLivesGained, 1);

                Singleton_Sound.m_instance.PlayAudioClip("1Up", 0.8f);
            }
        }
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
            if(m_score > m_highScores[i].m_score)
            {
                m_isNewHighScore = true;
                break;
            }
        }
    }

    public void SubmitHighScore(string argInitials)
    {
        string tempInitials = argInitials;
        int tempScore = m_score;

        for (int i = 0; i < m_highScores.Length; i++)
        {
            if (tempScore > m_highScores[i].m_score)
            {
                string currentInitials = m_highScores[i].m_initials;
                int currentScore = m_highScores[i].m_score;

                m_highScores[i].m_score = tempScore;
                m_highScores[i].m_initials = tempInitials;
                PlayerPrefs.SetInt("m_highScores_" + i, tempScore);
                PlayerPrefs.SetString("m_highScoreInitials_" + i, tempInitials);

                tempScore = currentScore;
                tempInitials = currentInitials;
            }
        }

        SaveGame();
        ResetGame();
    }

    public int GetScore()
    {
        return m_score;
    }
    
    public SHighScore GetHighScore(int argHighScore)
    {
        if (null == m_highScores)
            return new SHighScore();

        if (argHighScore > m_highScores.Length)
            return new SHighScore();

        return m_highScores[argHighScore];
    }

    public bool GetIsNewHighScore()
    {
        return m_isNewHighScore;
    }

    public void InsertMoney(int argMoney)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        m_insertedMoney += argMoney;
        Singleton_Sound.m_instance.PlayAudioClip("CoinUp", 0.1f);

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
                GetPlayer(0).SetActive(true);
                GetPlayer(0).transform.position = m_lastCheckPoint;
                GetPlayer(0).GetComponent<ISpawn>().OnSpawn();
                if (m_spawnedPlayer2)
                {
                    GetPlayer(1).SetActive(true);
                    GetPlayer(1).transform.position = m_lastCheckPoint;
                    GetPlayer(1).GetComponent<ISpawn>().OnSpawn();
                }
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

        if (m_playerLives <= 0)
            return;

        if (1 == argPlayerID && !m_spawnedPlayer2)
            return;

        GameObject player = GetPlayer(argPlayerID);
        if (player.GetComponent<PlayerController>().GetHealth() <= 0)
            player.GetComponent<ISpawn>().OnSpawn();

        player.transform.position = m_lastCheckPoint;
        player.SetActive(true);
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

        m_HUDPrefab.GetComponent<HUD>().SetPlayerLives(m_playerLives);

        if (m_playerLives <= 0)
        {
            if (!GetPlayer(0).activeSelf && (m_spawnedPlayer2 ? !GetPlayer(1).activeSelf : true))
            {
                GoToDeathScene();
            }
        }
    }

    public void CheckPlayersAlive()
    {
        if (m_playerLives > 0)
        {
            GameObject player1 = GetPlayer(0);
            GameObject player2 = GetPlayer(1);

            if (!player1.activeSelf && !player2.activeSelf)
            {
                System_Spawn.instance.ResetRegisteredIReactors();
                System_Spawn.instance.ResetRegisteredISpawners();
                System_Spawn.instance.DisableAllSpawnType(System_Spawn.ESpawnType.eEnemy);
                System_Spawn.instance.ActivateRegisteredSpawners();
            }

            if (!player1.activeSelf && (m_spawnedPlayer2 ? !player2.activeSelf : true))
            {
                ReSpawnPlayerAtCheckpoint(0);
            }

            if (m_spawnedPlayer2 && !player2.activeSelf && !player1.activeSelf)
            {
                ReSpawnPlayerAtCheckpoint(1);
            }
        }
    }

    private void GoToDeathScene()
    {
        System_Spawn.instance.DisableAllSpawns();

        GetPlayer(0).SetActive(true);

        if (m_spawnedPlayer2)
            GetPlayer(1).SetActive(true);

        m_previousLevelName = SceneManager.GetActiveScene().name;
        AddGameStat(EGameStat.eDeaths, 1);

        SceneManager.LoadScene("death");
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
        m_playerLives = 3;
        m_insertedMoney = 0;
        m_canStartGame = false;
        m_spawnedPlayer2 = false;
        m_showLevelDoorItem = false;
        m_previousLevelName = "Level1_heaven";
        m_isNewHighScore = false;
    }

    public void LoadGame()
    {
        for (int i = 0; i < m_highScores.Length; i++)
        {
            m_highScores[i].m_initials = PlayerPrefs.GetString("m_highScoreInitials_" + i);
            m_highScores[i].m_score = PlayerPrefs.GetInt("m_highScores_" + i);
        }

        m_masterMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
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

    public void AddGameStat(EGameStat argGameStat, int argAmount)
    {
        if (m_gameStats.ContainsKey(argGameStat))
        {
            m_gameStats[argGameStat] += argAmount;
        }
        else
        {
            m_gameStats.Add(argGameStat, argAmount);
        }
    }

    public int GetGameStat(EGameStat argGameStat)
    {
        if (m_gameStats.ContainsKey(argGameStat))
            return m_gameStats[argGameStat];
        else
            return 0;
    }


    public GameObject GetCamera()
    {
        return m_camera;
    }

    public void SetCamera(GameObject argCamera)
    {
        m_camera = argCamera;
    }
}
