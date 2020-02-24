using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, ISpawn, IDamageable
{

    [SerializeField] private Rigidbody2D m_Rigidbody = null;
    [SerializeField] private int m_Damage = 1;
    [SerializeField] private float m_KnockbackPower = 1f;
    [SerializeField] private LayerMask m_LayerMask;

    void Awake()
    {

        m_Rigidbody = this.gameObject.GetComponent<Rigidbody2D>();

    }

    // Start is called before the first frame update
    void Start()
    {

        //m_Rigidbody = this.gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (((1 << collision.gameObject.layer) & m_LayerMask) != 0)
        {
            if (collision.gameObject.GetComponent<IDamageable>() != null)
                collision.gameObject.GetComponent<IDamageable>().TakeDamage(m_Damage);

            if (collision.gameObject.GetComponent<ICanTakeKnockback>() != null)
                collision.gameObject.GetComponent<ICanTakeKnockback>().TakeKnockback(transform.position, m_KnockbackPower);

            KillEntity();
        }

    }

    // ISpawn
    public void OnSpawn()
    {

        m_Rigidbody.velocity = Vector3.zero;

    }

    public void OnDeSpawn()
    {



    }

    // IDamageable
    public void TakeDamage(int amount)
    {

        

    }

    public void KillEntity()
    {

        gameObject.SetActive(false);

    }

}
