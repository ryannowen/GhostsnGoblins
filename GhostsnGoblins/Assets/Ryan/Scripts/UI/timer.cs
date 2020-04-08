using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour
{
    public Text timeTxt;
    public int duration = 10;
    public float remainingTime;
    public bool isCountingDown = false;

    private void Start()
    {
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
