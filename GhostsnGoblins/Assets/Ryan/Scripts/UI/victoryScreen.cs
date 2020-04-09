using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class victoryScreen : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI m_scoreText = null;
    [SerializeField] private TMPro.TextMeshProUGUI m_newHighScoreText = null;
    [SerializeField] private TMPro.TMP_InputField m_initialsBox = null;
    // Start is called before the first frame update
    void Start()
    {
        if(null != m_scoreText)
        {
            m_scoreText.text = "You achieved a score of: " + Singleton_Game.m_instance.GetScore();
        }

        bool isNewHighScore = Singleton_Game.m_instance.GetIsNewHighScore();
        if (null != m_initialsBox)
        {
            m_initialsBox.gameObject.SetActive(isNewHighScore);
        }

        if (null != m_newHighScoreText)
        {
            m_newHighScoreText.gameObject.SetActive(isNewHighScore);
        }
    }

    public void SubmitHighScore()
    {
        if (null != m_initialsBox)
            Singleton_Game.m_instance.SubmitHighScore(m_initialsBox.text);
        else
            Debug.LogWarning("Cannot submit high score because initials box is null");

        SceneManager.LoadScene("MenuScene");
    }
}
