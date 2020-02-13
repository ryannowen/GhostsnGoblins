using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour, ISpawn
{

    private float m_TorchSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_TorchSpeed = 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {



    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (transform.up * m_TorchSpeed);
    }

    public void OnSpawn()
    {

    }

    public void OnDeSpawn()
    {

    }

}
