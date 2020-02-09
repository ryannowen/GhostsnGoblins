using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{

    public bool m_PlayerClimbing;
    public bool m_playerColliding;
    BoxCollider2D m_LadderCollider;
    public LayerMask m_PlayerLayerMask;

    // Start is called before the first frame update
    void Start()
    {

        m_LadderCollider = this.gameObject.GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {

        CheckForPlayerColliding();
        CheckForPlayerClimbing();

        if (m_PlayerClimbing)
        {

            transform.GetChild(0).gameObject.SetActive(false);
            //GameObject.Find("Prefab_Player").GetComponent<PlayerMovement>().SetClimbingState(true);

        }
        else 
        {

            transform.GetChild(0).gameObject.SetActive(true);
            //GameObject.Find("Prefab_Player").GetComponent<PlayerMovement>().SetClimbingState(false);

        }

    }

    void CheckForPlayerColliding()
    {

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, m_LadderCollider.size, 0, -Vector3.forward, Mathf.Infinity, m_PlayerLayerMask);

        if (hit)
        {

            if (hit.collider.gameObject.tag == "Player")
            {

                m_playerColliding = true;

            }
            else
            {

                m_playerColliding = false;

            }

        }
        else
        {

            m_playerColliding = false;

        }

    }

    void CheckForPlayerClimbing()
    {

        if (m_playerColliding)
        {

            if (!m_PlayerClimbing)
            {

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {

                    m_PlayerClimbing = true;

                }

            }

        }
        else
        {

            m_PlayerClimbing = false;

        }

    }

}
