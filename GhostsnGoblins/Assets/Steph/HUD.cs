using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [System.Serializable]
    class HealthbarColours
    {
        public string inspectorName = "";
        public Color healthbarColour = new Color();
    }
    
    // Copper healthBar.fill.color = new Color(239,15,30,255); 
    // Silver healthBar.fill.color = new Color(192,192,192,255); 
    // Gold healthBar.fill.color = new Color(255,215,0,255); 

    [SerializeField] private TextMeshProUGUI m_scoreText = null;
    [SerializeField] private TextMeshProUGUI m_highScoreText = null;

    [SerializeField] private GameObject[] m_healthBars = null;

    [SerializeField] private HealthbarColours[] healthbarColours = null;

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
        if(argPlayerID > m_healthBars.Length)
        {
            Debug.LogError("Cannot update HUD healthbar health because given PlayerID is too large");
            return;
        }

        if(null != m_healthBars[argPlayerID])
        {
            Debug.LogWarning("HUD player health bar was null, cannot set armour value");
            return;
        }

        m_healthBars[argPlayerID].GetComponent<Slider>().minValue = argHealth;
    }

    public void SetArmourType(int argPlayerID, PlayerController.ArmourType argArmourType)
    {
        if (argPlayerID > m_healthBars.Length)
        {
            Debug.LogError("Cannot update HUD healthbar health because given PlayerID is too large");
            return;
        }
        
        if (PlayerController.ArmourType.None == argArmourType)
            return;

        if (null != m_healthBars[argPlayerID])
        {
            Debug.LogWarning("HUD player health bar was null, cannot set armour type");
            return;
        }

        m_healthBars[argPlayerID].GetComponent<Slider>().maxValue = (int)argArmourType;

        Image healthBarFill = m_healthBars[argPlayerID].transform.Find("fill").GetComponent<Image>();
        healthBarFill.color = healthbarColours[(int)argArmourType].healthbarColour;
    }
}
