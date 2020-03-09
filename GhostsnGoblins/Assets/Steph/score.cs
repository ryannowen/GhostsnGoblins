
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    [SerializeField] private Text[] m_highScores = new Text[3];

    void UpdateHighScores()
    {
        for (int i = 0; i < m_highScores.Length; i++)
        {
            m_highScores[i].text = Singleton_Game.m_instance.GetHighScore(i).ToString();
        }
    }

    private void OnEnable()
    {
        UpdateHighScores();
    }
}
