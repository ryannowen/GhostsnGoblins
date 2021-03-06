﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraController : MonoBehaviour
{

    [SerializeField] private GameObject player1 = null;
    [SerializeField] private GameObject player2 = null;
    Camera m_CameraComponent = null;

    [SerializeField] private int minSize = 5;
    [SerializeField] private int maxSize = 10;
    [SerializeField] private int maxTetherDistanceFromPlayerOne = 10;

    [System.Serializable]
    public struct SLevelBackgrounds
    {
        public Sprite[] backgrounds;
    }

    [System.Serializable]
    public struct SParalaxBackgrounds
    {
        public SpriteRenderer bg1;
        public SpriteRenderer bg2;
        public SpriteRenderer bg3;
    }


    [SerializeField] private SLevelBackgrounds[] levelBackgrounds = null;
    [SerializeField] private SParalaxBackgrounds[] paralaxBackgrounds = null;

    // Start is called before the first frame update
    void Start()
    {

        m_CameraComponent = this.gameObject.GetComponent<Camera>();

        // Set position (Potentially remove later)
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        player1 = Singleton_Game.m_instance.GetPlayer(0);
        player2 = Singleton_Game.m_instance.GetPlayer(1);

        Singleton_Game.m_instance.SetCamera(gameObject);

        UpdateBackgrounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (player1 != null || player2 != null) // If there is a player active
            UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {

        // Both players exist
        if (player1 != null && player2 != null)
        {
            // Both players are active
            if (player1.activeSelf && player2.activeSelf)
            {
                Vector3 directionToSecondPlayer = player2.transform.position - player1.transform.position;
                float distanceOffsetFromPlayerOne = directionToSecondPlayer.magnitude / 2;
                if (distanceOffsetFromPlayerOne <= maxTetherDistanceFromPlayerOne)
                {
                    Vector3 newPosition = player1.transform.position + directionToSecondPlayer.normalized * distanceOffsetFromPlayerOne;

                    transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
                }
                else
                {
                    Vector3 newPosition = player1.transform.position + directionToSecondPlayer.normalized * maxTetherDistanceFromPlayerOne;

                    transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
                }

                if (directionToSecondPlayer.magnitude >= minSize * 2 && directionToSecondPlayer.magnitude <= maxSize * 2)
                {
                    m_CameraComponent.orthographicSize = directionToSecondPlayer.magnitude / 2;
                }
                else if (directionToSecondPlayer.magnitude <= minSize * 2)
                {
                    m_CameraComponent.orthographicSize = minSize;
                }
                else if (directionToSecondPlayer.magnitude <= maxSize * 2)
                {
                    m_CameraComponent.orthographicSize = maxSize;
                }
            }

            // Only player 1 active
            if (player1.activeSelf && !player2.activeSelf)
            {
                transform.position = new Vector3(player1.transform.position.x, player1.transform.position.y, transform.position.z);
            }

            // Only player 2 active
            if (!player1.activeSelf && player2.activeSelf)
            {
                transform.position = new Vector3(player2.transform.position.x, player2.transform.position.y, transform.position.z);
            }
        }
        // Only player 1 exisits
        if (player1 != null && player2 == null)
            transform.position = new Vector3(player1.transform.position.x, player1.transform.position.y, transform.position.z);

    }

    private void OnLevelWasLoaded(int level)
    {

        UpdateBackgrounds();
    }

    private void UpdateBackgrounds()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if ("victory" == sceneName || "death" == sceneName || "MenuScene" == sceneName)
            return;

        int sceneID = SceneManager.GetActiveScene().buildIndex - 2;
        for (int i = 0; i < 6; i++)
        {
            paralaxBackgrounds[i].bg1.sprite = levelBackgrounds[sceneID].backgrounds[i];
            paralaxBackgrounds[i].bg2.sprite = levelBackgrounds[sceneID].backgrounds[i];
            paralaxBackgrounds[i].bg3.sprite = levelBackgrounds[sceneID].backgrounds[i];
        }
    }
}
