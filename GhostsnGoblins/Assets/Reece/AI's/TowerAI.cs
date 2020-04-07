﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : MonoBehaviour, IDamageable
{

    [SerializeField] private GameObject Bullet = null;

    public bool Alive = true;
    private GameObject Enemy = null;
    private GameObject Player = null;
    private int HP = 1;
    private float time = 0.5f;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private bool angered = false;
    private bool FindPlayer;
    public bool Shoot;

    private FireProjectile fireProj;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        if (Bullet == null)
            Bullet = (GameObject)Resources.Load("Prefabs/Bullet") as GameObject;

        fireProj = this.gameObject.GetComponent<FireProjectile>();
        fireProj.SetProjectile(Bullet);



        Enemy = this.gameObject;

        Singleton_Sound.m_instance.CreateAudioClip(Resources.Load("Sounds/EnemyShoot") as AudioClip, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!angered)
        {
            PlayerX = Player.gameObject.transform.position.x;
            PlayerY = Player.gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
        }

        if (HP <= 0)
            KillEntity();

        if (Alive)
        {
            Enemy.SetActive(true);
            if (PlayerX + 10 > EnemyX && PlayerX + 10 > EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
            {
                angered = true;
            }
            if (angered)
            {
                if (Time.time > time)
                {
                    time += 1.5f;
                    FindPlayer = true;
                    Shoot = true;
                }
                // FindPlayer = true;
                if (FindPlayer)
                {
                    PlayerX = Player.gameObject.transform.position.x;
                    EnemyX = Enemy.gameObject.transform.position.x;

                    FindPlayer = false;
                }
                if (Shoot)
                {
                  if (PlayerX < EnemyX)
                        fireProj.Fire(transform.position, Vector3.left, transform.rotation);
                  else
                        fireProj.Fire(transform.position, Vector3.right, transform.rotation);
                }
            }

        }
        else
            Enemy.SetActive(false);
    }

    public void TakeDamage(int amount)
    {

        HP -= amount;
        Singleton_Sound.m_instance.PlayAudioClip("DamageInflictedSound");
    }

    public void KillEntity()
    {

        Alive = false;

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 18)
        {
            if (col.transform.parent.gameObject.GetComponent<IDamageable>() != null)
            {
                col.transform.parent.gameObject.GetComponent<IDamageable>().TakeDamage(1);
            }

            if (col.transform.parent.gameObject.GetComponent<ICanTakeKnockback>() != null)
            {
                col.transform.parent.gameObject.GetComponent<ICanTakeKnockback>().TakeKnockback(transform.position, 30);
            }
        }
    }
}
