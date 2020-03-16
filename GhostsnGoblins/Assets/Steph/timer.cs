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
    public bool timerActive = false;
    public void Start()
    {
        remainingTime = duration;
    }

    private void Update()
    {
        if (!timerActive)
            return;

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        remainingTime -= Time.deltaTime;
        timeTxt.text = ((int)remainingTime).ToString();

        if (remainingTime < 0)
        {
            if (sceneName == "death")
            {
                timeTxt.text = ("0");
                SceneManager.LoadScene(0);
                timerActive = false;
            }
            else
            {
                Debug.Log("You dead");
                timeTxt.text = ("0");
                SceneManager.LoadScene(1);
            }
        }
    }
}
