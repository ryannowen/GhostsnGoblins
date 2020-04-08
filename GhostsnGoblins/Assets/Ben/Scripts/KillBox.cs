using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    enum EKillboxType
    {
        eKillPlayer,
        eKillEnemies,
        eKillBoth
    }

    [SerializeField] bool m_instantKillPlayer = false;
    [SerializeField] EKillboxType m_killboxType = EKillboxType.eKillBoth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Pickup") || collision.gameObject.name.Contains("Projectile"))
        {
            collision.gameObject.SetActive(false);
        }

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (null != damageable)
        {


            if (collision.CompareTag("Player"))
            {
                if (m_killboxType == EKillboxType.eKillEnemies) // Not able to hurt player
                    return;

                if (m_instantKillPlayer)
                    damageable.KillEntity();
                else
                    damageable.TakeDamage(1);

                Singleton_Game.m_instance.MoveToCheckPoint(collision.gameObject);
            }
            else if (m_killboxType != EKillboxType.eKillPlayer)
            {
                damageable.KillEntity();
            }
        }
    }
}
