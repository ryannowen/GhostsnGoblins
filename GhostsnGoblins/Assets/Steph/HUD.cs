using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Copper healthBar.fill.color = new Color(239,15,30,255); 
    // Silver healthBar.fill.color = new Color(192,192,192,255); 
    // Gold healthBar.fill.color = new Color(255,215,0,255); 

    [SerializeField] GameObject[] m_healthBars = null;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(int argPlayerID, int argHealth)
    {
        if(argPlayerID > m_healthBars.Length)
        {
            Debug.LogError("Cannot update HUD healthbar health because given PlayerID is too large");
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

        if(PlayerController.ArmourType.None == argArmourType)
            return;

        m_healthBars[argPlayerID].GetComponent<Slider>().maxValue = (int)argArmourType;

    }
}
