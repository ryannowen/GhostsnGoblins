﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<IDamageable>() != null)
            collision.gameObject.GetComponent<IDamageable>().KillEntity();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<IDamageable>() != null)
            collision.gameObject.GetComponent<IDamageable>().KillEntity();

    }

}
