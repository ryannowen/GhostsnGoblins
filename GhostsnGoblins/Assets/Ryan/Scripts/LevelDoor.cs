using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    [SerializeField] private string m_customSceneName = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerController>().HasKey())
            {
                Singleton_Sound.m_instance.PlayAudioClip("LevelFinished");

                if (m_customSceneName == "") // Next Scene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                else // Custom Scene
                    SceneManager.LoadScene(m_customSceneName);
            }
        }
    }
}
