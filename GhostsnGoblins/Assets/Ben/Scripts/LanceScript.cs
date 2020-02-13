using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceScript : MonoBehaviour, ISpawn
{

    private float m_LanceSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_LanceSpeed = 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (transform.up * m_LanceSpeed);
    }

    public void OnSpawn()
    {

    }

    public void OnDeSpawn()
    {

    }

}
