using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void playGame ()
    {
        SceneManager.LoadScene(2);
        Singleton_Game.m_instance.gameObject.transform.Find("Prefab_HUD").GetComponent<HUD>().SetTimerActiveState(true);
    }

    public void quitGame ()
    {
        Application.Quit();
    }
}
