using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private GameObject player1 = null;
    [SerializeField] private GameObject player2 = null;
    Camera m_CameraComponent = null;

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
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {

        Vector3 directionToSecondPlayer = player2.transform.position - player1.transform.position;
        float distanceOffsetFromPlayerOne = directionToSecondPlayer.magnitude / 2;
        Vector3 newPosition = player1.transform.position + directionToSecondPlayer.normalized * distanceOffsetFromPlayerOne;

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        if (directionToSecondPlayer.magnitude >= 10f && directionToSecondPlayer.magnitude <= 20f)
        {
            m_CameraComponent.orthographicSize = directionToSecondPlayer.magnitude / 2;
        }
        else if (directionToSecondPlayer.magnitude <= 10f)
        {
            m_CameraComponent.orthographicSize = 5f;
        }
        else if (directionToSecondPlayer.magnitude <= 20f)
        {
            m_CameraComponent.orthographicSize = 10f;
        }

    }

}
