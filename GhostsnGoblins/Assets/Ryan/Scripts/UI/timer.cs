using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timeTxt;
    public int duration = 10;
    public float remainingTime;
    public bool isCountingDown = false;
    [SerializeField] private Color m_startColour = Color.green;
    [SerializeField] private Color m_endColour = Color.red;

    private void Start()
    {
        timeTxt.color = m_startColour;
        remainingTime = duration;
    }

    private void Update()
    {
        if (!isCountingDown)
            return;

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        remainingTime -= Time.deltaTime;
        timeTxt.text = ((int)remainingTime).ToString();

        timeTxt.color = Color.Lerp(m_startColour, m_endColour, (1 - (remainingTime / duration)));

        if (remainingTime < 0)
        {
            timeTxt.text = "0";

            if (sceneName == "death") // Loads Main Menu
            {
                Singleton_Game.m_instance.ResetGame();
                SceneManager.LoadScene("MenuScene");
            }
            else // Loads Death Scene
            {
                
                Singleton_Game.m_instance.SetPreviousScene(sceneName);
                SceneManager.LoadScene("death");
            }

            isCountingDown = false;
        }
    }

    public void ResetTimer()
    {
        remainingTime = duration;
        isCountingDown = true;
    }
}
