﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{

    private GameObject m_Projectile = null;
    [SerializeField] private float m_ProjectileSpeed = 5f;
    [SerializeField] private bool doesDecay = true;
    [SerializeField] private float decayTime = 3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fire(Vector3 argsStartPosition, Vector3 argsDirection, Quaternion argsRotation) 
    {

        GameObject tempProjectile = System_Spawn.instance.GetObjectFromPool(m_Projectile);
        tempProjectile.GetComponent<SpriteRenderer>().flipX = false;

        Animation anim = null;
        if (tempProjectile.gameObject.GetComponent<Animation>())
            anim = tempProjectile.gameObject.GetComponent<Animation>();

        if (argsDirection.x > 0)
        {
            tempProjectile.gameObject.transform.localScale = new Vector3(1, 1, 1);
            if(anim != null)
                anim["Spin"].speed = 1;
            tempProjectile.GetComponent<Projectile>().SetObjectToLeaveBehindFlipX(false);
        }
        else
        {
            tempProjectile.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            if (anim != null)
                anim["Spin"].speed = -1;
            tempProjectile.GetComponent<Projectile>().SetObjectToLeaveBehindFlipX(true);
        }

        if (tempProjectile != null)
        {
            tempProjectile.transform.position = argsStartPosition;
            tempProjectile.transform.rotation = argsRotation;
            tempProjectile.GetComponent<Rigidbody2D>().AddForce(argsDirection * m_ProjectileSpeed, ForceMode2D.Impulse);
            tempProjectile.GetComponent<Projectile>().SetProjectileProperties(doesDecay, decayTime);
        }

    }

    public void SetProjectile(GameObject argsNewProjectile)
    {

        m_Projectile = argsNewProjectile;

    }

}
