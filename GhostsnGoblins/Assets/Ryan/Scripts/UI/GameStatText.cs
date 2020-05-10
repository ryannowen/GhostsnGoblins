using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatText : MonoBehaviour
{
    [SerializeField] private string m_text = "Unknown";
    [SerializeField] private Singleton_Game.EGameStat m_stat = Singleton_Game.EGameStat.EKills;
    [SerializeField] private TMPro.TextMeshProUGUI m_textGUI = null;

    // Start is called before the first frame update
    void Start()
    {
        if (null != m_textGUI)
            m_textGUI.text = m_text + ": " + Singleton_Game.m_instance.GetGameStat(m_stat);
    }
}
