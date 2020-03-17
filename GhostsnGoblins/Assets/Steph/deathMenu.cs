using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathMenu : MonoBehaviour
{
    private void Start()
    {
        Singleton_Sound.m_instance.PlayAudioClip("GameOver");
    }

    public void continueGame()
    {
        SceneManager.LoadScene(2);
    }

    public void quitGame()
    {
        SceneManager.LoadScene(0);
    }
}
