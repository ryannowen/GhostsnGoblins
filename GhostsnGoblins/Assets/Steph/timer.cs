using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class timer : MonoBehaviour

{
    public Text timeTxt;
    public float duration = 10;
    public int remainingTime;
    public bool isCountingDown = false;

    public void Begin()
    {

        if (!isCountingDown)
        {
            isCountingDown = true;
            remainingTime = (int)duration;
            Invoke("_tick", 1f);
        }
    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        duration -= Time.deltaTime;
        remainingTime = (int)duration;
        timeTxt.text = remainingTime.ToString();
        if (remainingTime < 0)
            if (sceneName == "death")
            {
                timeTxt.text = ("0");
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.Log("You dead");
                timeTxt.text = ("0");
                SceneManager.LoadScene(1);
            }

    }
}
