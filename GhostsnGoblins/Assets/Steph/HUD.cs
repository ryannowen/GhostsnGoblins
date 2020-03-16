using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private TextMeshProUGUI m_scoreText = null;
    [SerializeField] private TextMeshProUGUI m_highScoreText = null;

    [SerializeField] private GameObject[] m_healthBarsGameObjects = null;
    [SerializeField] private Image[] m_healthbarImages = null;
    [SerializeField] private Sprite[] m_armourSprites = null;

    [SerializeField] private Healthbar[] m_healthbars = null;

    void Start()
    {
        if (null != m_highScoreText)
            m_highScoreText.text = "High Score: " + Singleton_Game.m_instance.GetHighScore(0);
    }

    void Update()
    {
        if(null != m_scoreText)
            m_scoreText.text = "Score: " + Singleton_Game.m_instance.GetScore();
    }

    public void SetArmourValue(int argPlayerID, int argHealth)
    {
        if(argPlayerID > m_healthBarsGameObjects.Length)
        {
            Debug.LogError("Cannot update HUD healthbar health because given PlayerID is too large");
            return;
        }

        if(null != m_healthBarsGameObjects[argPlayerID])
        {
            Debug.LogWarning("HUD player health bar was null, cannot set armour value");
            return;
        }

        m_healthBarsGameObjects[argPlayerID].GetComponent<Slider>().minValue = argHealth;
    }

    public void SetArmourType(int argPlayerID, PlayerController.ArmourType argArmourType)
    {
        if (argPlayerID > m_healthBarsGameObjects.Length)
        {
            Debug.LogError("Cannot update HUD healthbar health because given PlayerID is too large");
            return;
        }
        
        if (PlayerController.ArmourType.None == argArmourType)
            return;

        if (null != m_healthBarsGameObjects[argPlayerID])
        {
            Debug.LogWarning("HUD player health bar was null, cannot set armour type");
            return;
        }

        m_healthBarsGameObjects[argPlayerID].GetComponent<Slider>().maxValue = (int)argArmourType;
        Image healthBarFill = m_healthBarsGameObjects[argPlayerID].transform.Find("fill").GetComponent<Image>();

        if(null != healthBarFill)
            healthBarFill.color = m_healthbars[(int)argArmourType].healthbarColour;

        if ((int)argArmourType > m_armourSprites.Length)
        {
            Debug.LogWarning("Cannot change armour image because not enough armour sprites are given");
            return;
        }

        if((int)argArmourType > m_healthbarImages.Length)
        {
            Debug.LogWarning("Cannot change armour image because not enough healthbar images are given");
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
}
