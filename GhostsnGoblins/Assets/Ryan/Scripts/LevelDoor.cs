using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    [SerializeField] private string m_customSceneName = "";
    [Space]
    [SerializeField] private string m_requiredEquippedItemName = "";
    [SerializeField] private SpriteRenderer m_requiredItemRenderer = null;
    [SerializeField] private bool m_requireKey = false;
    [SerializeField] private bool m_doorOpen = false;

    private void Start()
    {
        m_requiredItemRenderer.gameObject.SetActive(Singleton_Game.m_instance.GetShowLevelDoorItem());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerC = collision.GetComponent<PlayerController>();

            if (m_requireKey ? playerC.HasKey() : true || m_doorOpen)
            {
                Singleton_Sound.m_instance.PlayAudioClip("LevelFinished");
                System_Spawn.instance.DisableAllSpawns();

                if (m_requireKey)
                    playerC.SetHasKey(false);

                if ("" == m_requiredEquippedItemName) // No equipped item required
                {
                    LoadNextScene();
                }
                else // Item required
                {
                    if (playerC.GetEquippedItem().name.Contains(m_requiredEquippedItemName)) // Player has item
                    {
                        Singleton_Game.m_instance.SetShowLevelDoorItem(false);
                        LoadNextScene();
                    }
                    else // Player doesn't have item
                    {
                        System_Spawn.instance.ClearSpawners();

                        Singleton_Game.m_instance.SetShowLevelDoorItem(true);
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                }
            }
        }
    }

    private void LoadNextScene()
    {
        System_Spawn.instance.ClearSpawners();

        if (m_customSceneName == "") // Next Scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else // Custom Scene
            SceneManager.LoadScene(m_customSceneName);
    }

    public void SetDoorOpen(bool argState)
    {
        m_doorOpen = argState;
    }
}
