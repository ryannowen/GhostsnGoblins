using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeithBeam : MonoBehaviour
{
    [SerializeField] private Keith m_keithScript = null;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (18 != collision.gameObject.layer) // player layer
            return;

        if (null == m_keithScript)
            return;

        m_keithScript.BeamCollision();

        if (collision.transform.parent.gameObject.GetComponent<IDamageable>() != null)
        {
            collision.transform.parent.gameObject.GetComponent<IDamageable>().TakeDamage(1);
        }
    }
}
