using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, ISpawn, IDamageable
{

    [SerializeField] private Rigidbody2D m_Rigidbody = null;
    [SerializeField] private int m_Damage = 1;
    [SerializeField] private float m_KnockbackPower = 1f;
    [SerializeField] private bool m_DecayAfterTime = false;
    [SerializeField] private bool m_DestroyedOnCollision = true;
    [SerializeField] private float m_DecayTime = 3f;
    [SerializeField] private LayerMask m_LayerMask = new LayerMask();
    [SerializeField] private bool m_LeaveObjectBehind = false;
    [SerializeField] private GameObject m_ObjectToLeaveBehind = null;
    private bool flipXObjectToLeaveBehind = false;

    private float m_StoredDecayTime = 0f;

    void Awake()
    {

        m_Rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        m_StoredDecayTime = m_DecayTime;

    }

    // Start is called before the first frame update
    void Start()
    {

        //m_Rigidbody = this.gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        // Check if the entity should use the decay timer
        if (m_DecayAfterTime)
        {
            // Decrease the timer
            m_DecayTime -= Time.deltaTime;
            // Kill the entity if the timer runs out
            if (m_DecayTime <= 0f)
            {
                KillEntity();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (((1 << collision.gameObject.layer) & m_LayerMask) != 0)
        {
            if (collision.gameObject.GetComponent<IDamageable>() != null)
                collision.gameObject.GetComponent<IDamageable>().TakeDamage(m_Damage);
            if (collision.gameObject.GetComponent<ICanTakeKnockback>() != null)
                collision.gameObject.GetComponent<ICanTakeKnockback>().TakeKnockback(transform.position, m_KnockbackPower);

            if (m_DestroyedOnCollision)
            {
                if (m_LeaveObjectBehind && collision.gameObject.layer == 0)
                {
                    GameObject temp = System_Spawn.instance.GetObjectFromPool(m_ObjectToLeaveBehind);
                    temp.transform.position = transform.position;
                    //temp.transform.rotation = transform.rotation;
                    if (flipXObjectToLeaveBehind)
                        temp.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    else
                        temp.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
                KillEntity();
            }
        }

    }

    // ISpawn
    public void OnSpawn()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_DecayTime = m_StoredDecayTime;
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

    public void SetProjectileProperties(bool canDecay, float decayTime) 
    {
        m_DecayAfterTime = canDecay;
        m_DecayTime = decayTime;
    }

    public void SetObjectToLeaveBehindFlipX(bool argsValue)
    {
        flipXObjectToLeaveBehind = argsValue;
    }

}
