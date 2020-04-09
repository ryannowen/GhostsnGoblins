
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    [SerializeField] private Text[] m_highScores = new Text[3];

    void UpdateHighScores()
    {
        for (int i = 0; i < m_highScores.Length; i++)
        {
            Singleton_Game.SHighScore highScore = Singleton_Game.m_instance.GetHighScore(i);

            if (highScore.m_score <= 0)
                m_highScores[i].text = "";
            else
                m_highScores[i].text = highScore.m_initials + ":   " + highScore.m_score.ToString();
        }
    }

    private void OnEnable()
    {
        UpdateHighScores();
    }
}
