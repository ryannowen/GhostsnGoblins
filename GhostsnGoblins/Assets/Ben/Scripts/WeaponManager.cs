﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour, IWeapon
{

    FireProjectile m_FireProjectile = null;
    [SerializeField] private GameObject m_Projectile = null;
    [SerializeField] private Vector3 shootingDirectionModification = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {

        m_FireProjectile = this.gameObject.GetComponent<FireProjectile>();
        m_FireProjectile.SetProjectile(m_Projectile);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // IWeapon
    public void Action(GameObject objectFiring, Vector3 argsStartPosition, Vector3 argsDirection)
    {

        Singleton_Sound.m_instance.PlayAudioClipOneShot("Throw", 0.3f);
        
        argsDirection += shootingDirectionModification;

        argsDirection.Normalize();

        m_FireProjectile.Fire(argsStartPosition, argsDirection, transform.rotation);

    }

}
