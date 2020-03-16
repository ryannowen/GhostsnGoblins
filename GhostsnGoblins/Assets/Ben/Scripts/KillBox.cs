using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    [SerializeField] bool m_instantKillPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (null != damageable)
        {
            if (collision.CompareTag("Player") && !m_instantKillPlayer)
            {
                damageable.TakeDamage(-1);
                Singleton_Game.m_instance.MoveToCheckPoint(collision.gameObject);
            }
            else
                damageable.KillEntity();
        }
    }
}
