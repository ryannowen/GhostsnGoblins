using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{

    [SerializeField] private Collider2D m_LadderPlatformCollider;

    // Start is called before the first frame update
    void Start()
    {

        m_LadderPlatformCollider = transform.Find("LadderPlatform").GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {

        

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.9f && !collision.gameObject.GetComponent<PlayerMovement>().GetClimbingState())
            {
                collision.gameObject.GetComponent<PlayerMovement>().SetClimbingState(true);
            }

            if (collision.gameObject.GetComponent<PlayerMovement>().GetClimbingState())
            {
                collision.gameObject.GetComponent<PlayerMovement>().LerpToLadder(transform.position);
                m_LadderPlatformCollider.enabled = false;
            }
            else
            {
                m_LadderPlatformCollider.enabled = true;
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            collision.gameObject.GetComponent<PlayerMovement>().SetClimbingState(false);
            m_LadderPlatformCollider.enabled = true;

        }

    }

}
