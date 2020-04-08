using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI m_playText = null;

    public void playGame ()
    {
        if(Singleton_Game.m_instance.GetCanStartGame())
            SceneManager.LoadScene(2);
    }

    public void quitGame ()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(null == m_playText)
        {
            Debug.LogError("Cannot update play game text because reference is null");
            return;
        }


        if (Singleton_Game.m_instance.GetCanStartGame() && m_playText.text != "PLAY")
            m_playText.text = "PLAY";
    }
}
