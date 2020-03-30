using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUD : MonoBehaviour
{
    [System.Serializable]
    class Healthbar
    {
        public string inspectorName = "";
        public Color healthbarColour = new Color();
    }

    // Copper healthBar.fill.color = new Color(239,15,30,255); 
    // Silver healthBar.fill.color = new Color(192,192,192,255); 
    // Gold healthBar.fill.color = new Color(255,215,0,255); 

    [SerializeField] private GameObject m_gameUI = null;
    [SerializeField] private GameObject m_shopUI = null;
    [Space]
    [SerializeField] private TextMeshProUGUI m_scoreText = null;
    [SerializeField] private TextMeshProUGUI m_highScoreText = null;
    [SerializeField] private timer m_HUDTimer = null;
    [Space]
    [SerializeField] private GameObject[] m_healthBarsGameObjects = null;
    [SerializeField] private Healthbar[] m_healthbars = null;
    [SerializeField] private Image[] m_healthbarImages = null;
    [SerializeField] private Sprite[] m_armourSprites = null;

    [SerializeField] private int[] m_currentArmours = null;
    [SerializeField] private float[] m_healthbarTargets = null;

    void Start()
    {
        m_currentArmours = new int[m_healthBarsGameObjects.Length];
        m_healthbarTargets = new float[m_healthBarsGameObjects.Length];

        if (null != m_highScoreText)
            m_highScoreText.text = "High Score: " + Singleton_Game.m_instance.GetHighScore(0);
    }

    void Update()
    {
        for(int i = 0; i < m_healthbarTargets.Length; i++)
        {
            Slider slider = m_healthBarsGameObjects[i].GetComponent<Slider>();
            slider.value = Mathf.Lerp(slider.value, m_healthbarTargets[i], 0.1f);
        }

        if(null != m_scoreText)
            m_scoreText.text = "Score: " + Singleton_Game.m_instance.GetScore();


        if(Input.GetKeyDown(KeyCode.L))
        {
            Singleton_Game.m_instance.InsertMoney(20);
            Debug.Log("Down");
        }

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetArmourValue(int argPlayerID, int argArmourValue)
    {
        if(argPlayerID > m_healthBarsGameObjects.Length)
        {
            Debug.LogError("Cannot update HUD healthbar health because given PlayerID is too large");
            return;
        }

        if(null == m_healthBarsGameObjects[argPlayerID])
        {
            Debug.LogWarning("HUD player health bar was null, cannot set armour value");
            return;
        }

        if (argArmourValue == m_currentArmours[argPlayerID])
            m_healthbarTargets[argPlayerID] = 3;
        else
            m_healthbarTargets[argPlayerID] = 3.0f / m_currentArmours[argPlayerID] * argArmourValue;
    }

    public void SetArmourType(int argPlayerID, PlayerController.ArmourType argArmourType)
    {
        if (argPlayerID > m_healthBarsGameObjects.Length)
        {
            Debug.LogError("Cannot update HUD healthbar health because given PlayerID is too large");
            return;
        }

        if (null == m_healthBarsGameObjects[argPlayerID])
        {
            Debug.LogWarning("HUD player health bar was null, cannot set armour type");
            return;
        }

        m_currentArmours[argPlayerID] = (PlayerController.ArmourType.None == argArmourType) ? 3 : (int)argArmourType;

        Image healthBarFill = m_healthBarsGameObjects[argPlayerID]/*.transform.Find("barMask").*/.transform.Find("colourFill").GetComponent<Image>();

        if(null != healthBarFill)
            healthBarFill.color = m_healthbars[(int)argArmourType].healthbarColour;

        if ((int)argArmourType > m_armourSprites.Length)
        {
            Debug.LogWarning("Cannot change armour image because not enough armour sprites are given" + (int)argArmourType);
            return;
        }

        m_healthbarImages[argPlayerID].sprite = m_armourSprites[(int)argArmourType];
    }

    public void SetPlayerHUDActive(int argPlayerID, bool argActiveState)
    {
        if (argPlayerID > m_healthBarsGameObjects.Length)
        {
            Debug.LogWarning("Cannot set active state of player HUD because the player ID excedes the number of healthbar gameobjects");
            return;
        }
        m_healthBarsGameObjects[argPlayerID].SetActive(argActiveState);


        if (argPlayerID > m_healthbarImages.Length)
        {
            Debug.LogWarning("Cannot set active state of player HUD because the player ID excedes the number of healthbar images");
            return;
        }
        m_healthbarImages[argPlayerID].gameObject.SetActive(argActiveState);
    }

    public void ToggleUI()
    {
        if (null != m_gameUI)
            m_gameUI.SetActive(!m_gameUI.activeSelf);

        if (null != m_shopUI)
            m_shopUI.SetActive(!m_shopUI.activeSelf);
    }

    public void ShowHUD()
    {
        if (null != m_gameUI)
            m_gameUI.SetActive(true);

        if (null != m_shopUI)
            m_shopUI.SetActive(false);
    }

    public void ShowItemShop()
    {
        if (null != m_gameUI)
            m_gameUI.SetActive(false);

        if (null != m_shopUI)
            m_shopUI.SetActive(true);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Enables timer if not on death scene or Main Menu
        m_HUDTimer.enabled = (1 != scene.buildIndex && 0 != scene.buildIndex);

        if (m_HUDTimer.enabled)
            m_HUDTimer.ResetTimer();

    }
}
