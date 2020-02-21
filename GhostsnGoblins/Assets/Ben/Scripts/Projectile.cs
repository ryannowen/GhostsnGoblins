using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, ISpawn
{

    [SerializeField] private Rigidbody2D m_Rigidbody = null;

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

    // ISpawn
    public void OnSpawn()
    {

        m_Rigidbody.velocity = Vector3.zero;

    }

    public void OnDeSpawn()
    {



    }

}
