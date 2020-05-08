using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keith : MonoBehaviour, IDamageable
{
    enum EState
    {
        eIdle,
        eRoll,
        eBeam,
        eLaughing
    }

    [SerializeField] private EState m_state = EState.eRoll;

    private Animator m_animator;
    private GameObject m_player;
    private Rigidbody2D m_rigidbody;

    [SerializeField] private int m_speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_player = Singleton_Game.m_instance.GetPlayer(0);
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool playerOnRight = false;

        if(m_player.transform.position.x - transform.position.x > 0)
        {
            
            playerOnRight = true;
        }
        else
        {
            playerOnRight = false;
        }

        if (null != m_animator)
        {
            m_animator.SetBool("isPlayerOnRight", playerOnRight);
            m_animator.SetFloat("RollSpeed", playerOnRight ? 1 : -1);
        }

        switch (m_state)
        {
            case EState.eIdle:
                if (!m_animator.GetBool("isIdle") && (null != m_animator))
                {
                    m_animator.SetBool("isRolling", false);
                    m_animator.SetBool("isIdle", true);
                    m_animator.SetBool("isBeaming", false);
                    m_animator.SetBool("isLaughing", false);
                }
                break;
            case EState.eRoll:
                if (!m_animator.GetBool("isRolling") && (null != m_animator))
                {
                    m_animator.SetBool("isRolling", true);
                    m_animator.SetBool("isIdle", false);
                    m_animator.SetBool("isBeaming", false);
                    m_animator.SetBool("isLaughing", false);
                }

                m_rigidbody.AddRelativeForce((playerOnRight ? transform.right : -transform.right) * m_speed);
                break;
            case EState.eBeam:
                if (!m_animator.GetBool("isBeaming") && (null != m_animator))
                {
                    m_animator.SetBool("isRolling", false);
                    m_animator.SetBool("isIdle", false);
                    m_animator.SetBool("isBeaming", true);
                    m_animator.SetBool("isLaughing", false);
                }
                break;
            case EState.eLaughing:
                if (!m_animator.GetBool("isLaughing") && (null != m_animator))
                {
                    m_animator.SetBool("isRolling", false);
                    m_animator.SetBool("isIdle", false);
                    m_animator.SetBool("isBeaming", false);
                    m_animator.SetBool("isLaughing", true);
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (18 == collision.gameObject.layer) // player layer
        {

            if (collision.transform.parent.gameObject.GetComponent<IDamageable>() != null)
            {
                collision.transform.parent.gameObject.GetComponent<IDamageable>().TakeDamage(1);
            }


            m_state = EState.eBeam;
        }
    }

    public void TakeDamage(int argDamage)
    {
        if (EState.eRoll == m_state)
        {
            m_rigidbody.velocity = Vector3.zero;
            m_state = EState.eIdle;
        }
    }

    public void KillEntity()
    {

    }
}
