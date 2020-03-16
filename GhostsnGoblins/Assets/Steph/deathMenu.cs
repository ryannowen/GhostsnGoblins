using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathMenu : MonoBehaviour
{
    public void continueGame()
    {
        SceneManager.LoadScene(2);
    }

    public void quitGame()
    {
        SceneManager.LoadScene(0);
    }
}
