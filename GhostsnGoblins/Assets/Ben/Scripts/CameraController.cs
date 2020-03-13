﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private GameObject player1 = null;
    [SerializeField] private GameObject player2 = null;
    Camera m_CameraComponent = null;

    [SerializeField] private int minSize = 5;
    [SerializeField] private int maxSize = 10;
    [SerializeField] private int maxTetherDistanceFromPlayerOne = 10;

    // Start is called before the first frame update
    void Start()
    {

        m_CameraComponent = this.gameObject.GetComponent<Camera>();

        // Set position (Potentially remove later)
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        // Find players
        if (GameObject.Find("Player1") != null)
        {
            player1 = GameObject.Find("Player1");
        }

        if (GameObject.Find("Player2") != null)

        {
            player2 = GameObject.Find("Player2");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player1 != null && player2 != null)
            UpdateCameraPosition();
        else
            transform.position = new Vector3(player1.transform.position.x, player1.transform.position.y, transform.position.z);
    }

    void UpdateCameraPosition()
    {

        Vector3 directionToSecondPlayer = player2.transform.position - player1.transform.position;
        float distanceOffsetFromPlayerOne = directionToSecondPlayer.magnitude / 2;
        if (distanceOffsetFromPlayerOne <= maxTetherDistanceFromPlayerOne)
        {
            Vector3 newPosition = player1.transform.position + directionToSecondPlayer.normalized * distanceOffsetFromPlayerOne;

            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        } else
        {
            Vector3 newPosition = player1.transform.position + directionToSecondPlayer.normalized * maxTetherDistanceFromPlayerOne;

            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }

        if (directionToSecondPlayer.magnitude >= minSize*2 && directionToSecondPlayer.magnitude <= maxSize*2)
        {
            m_CameraComponent.orthographicSize = directionToSecondPlayer.magnitude / 2;
        }
        else if (directionToSecondPlayer.magnitude <= minSize*2)
        {
            m_CameraComponent.orthographicSize = minSize;
        }
        else if (directionToSecondPlayer.magnitude <= maxSize*2)
        {
            m_CameraComponent.orthographicSize = maxSize;
        }

    }

}