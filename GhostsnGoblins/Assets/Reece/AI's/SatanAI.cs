﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatanAI : MonoBehaviour, IDamageable, ISpawn
{
    public bool Alive = true;

    [SerializeField] private GameObject Bullet = null;


    SpawnPickup m_SpawnPickup = null;
    private GameObject Enemy;
    private GameObject Player;
    private int RNG;
    private int HP = 4;
    private float speed = 0.1f;
    private float RNGtimer = 3;
    private float PlayerX;
    private float PlayerY;
    private float PlayerX2;
    private float PlayerY2;
    private float Distance;
    private float Distance2;
    private float EnemyX;
    private float EnemyY;
    private float DistanceX;
    private float DistanceY;
    private float ClosestPlayer = 0;
    private bool FindPlayer = true;
    private bool Swoop;
    private bool Shoot;
    private bool Angered;
    private bool AntiSwoopLeft;
    private bool AntiSwoopRight;
    private float InvicibleTimer;

    private FireProjectile fireProj;
    //private float Deathtimer = 100f;

    // Start is called before the first frame update
    void Start()
    {

        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        if (Bullet == null)
            Bullet = (GameObject)Resources.Load("Prefabs/Bullet") as GameObject;

        fireProj = this.gameObject.GetComponent<FireProjectile>();
        fireProj.SetProjectile(Bullet);

        m_SpawnPickup = this.gameObject.GetComponent<SpawnPickup>();

        Enemy = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindPlayer)
        {
            PlayerX = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.x;
            PlayerY = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.y;
            PlayerX2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.x;
            PlayerY2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.y;
            EnemyY = Enemy.gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            Distance = EnemyX - PlayerX;
            Distance2 = EnemyX - PlayerX2;

            if (Distance < 0)
                Distance = -Distance;
            if (Distance2 < 0)
                Distance2 = -Distance2;

            if (Distance < Distance2)
            {
                DistanceY = EnemyY - PlayerY;
                DistanceX = EnemyX - PlayerX;
                ClosestPlayer = 1;
            }

            if (Distance2 < Distance)
            {
                DistanceY = EnemyY - PlayerY2;
                DistanceX = EnemyX - PlayerX2;
                ClosestPlayer = 2;
            }

            FindPlayer = false;
        }

        if (!Angered)
        {
            FindPlayer = true;
            if (PlayerX + 5 > EnemyX && PlayerX - 5 < EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
            {
                Angered = true;
            }

            if (PlayerX2 + 5 > EnemyX && PlayerX2 - 5 < EnemyX && PlayerY2 + 3 > EnemyY && PlayerY2 - 3 < EnemyY)
            {
                Angered = true;
            }
        }

        if (Time.time > RNGtimer)
        {
            RNG = Random.Range(2, 100);
            RNGtimer += 3;
            FindPlayer = true;
        }

        if (!Swoop && !AntiSwoopLeft && !AntiSwoopRight && !Shoot)
        {
            if (RNG > 30)
            {
                Swoop = true;
            }
            if (RNG > 0 && RNG <= 30)
            {
                Shoot = true;
            }

            RNG = 0;

        }


        if (Angered && Alive)
        {

            if (Swoop)
            {
                if (ClosestPlayer == 1)
                {
                    if (PlayerX < EnemyX && PlayerY < EnemyY)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        EnemyX -= DistanceX / 80;
                        EnemyY -= DistanceY / 80;
                        if (PlayerX + 0.3f > EnemyX && PlayerX - 0.3f < EnemyX)
                        {
                            AntiSwoopLeft = true;
                            Swoop = false;
                        }
                    }
                    if (PlayerX > EnemyX && PlayerY < EnemyY)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        EnemyX -= DistanceX / 80;
                        EnemyY -= DistanceY / 80;
                        if (PlayerX + 0.3f > EnemyX && PlayerX - 0.3f < EnemyX)
                        {
                            AntiSwoopRight = true;
                            Swoop = false;
                        }
                    }
                }

                if (ClosestPlayer == 2)
                {
                    if (PlayerX2 < EnemyX && PlayerY2 < EnemyY)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        EnemyX -= DistanceX / 60;
                        EnemyY -= DistanceY / 60;
                        if (PlayerX + 0.3f > EnemyX && PlayerX - 0.3f < EnemyX)
                        {
                            AntiSwoopLeft = true;
                            Swoop = false;
                        }
                    }
                    if (PlayerX2 > EnemyX && PlayerY2 < EnemyY)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        EnemyX -= DistanceX / 60;
                        EnemyY -= DistanceY / 60;
                        if (PlayerX + 0.3f > EnemyX && PlayerX - 0.3f < EnemyX)
                        {
                            AntiSwoopRight = true;
                            Swoop = false;
                        }
                    }
                }
            }


            if (AntiSwoopLeft)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                if (ClosestPlayer == 1)
                {
                    if (PlayerX - 6 < EnemyX)
                    {
                        EnemyX -= speed * 3;
                    }
                    else
                    {
                        AntiSwoopLeft = false;
                    }
                    if (PlayerY + 3 > EnemyY)
                    {
                        EnemyY += speed * 1.5f;
                    }
                    else
                    {
                        AntiSwoopLeft = false;
                    }
                }

                if (ClosestPlayer == 2)
                {
                    if (PlayerX2 - 6 < EnemyX)
                    {
                        EnemyX -= speed * 3;
                    }
                    else
                    {
                        AntiSwoopLeft = false;
                    }
                    if (PlayerY2 + 3 > EnemyY)
                    {
                        EnemyY += speed * 1.5f;
                    }
                    else
                    {
                        AntiSwoopLeft = false;
                    }
                }
            }


            if (AntiSwoopRight)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                if (ClosestPlayer == 1)
                {
                    if (PlayerX + 6 > EnemyX)
                    {
                        EnemyX += speed * 3;
                    }
                    else
                    {
                        AntiSwoopRight = false;
                    }
                    if (PlayerY + 3 > EnemyY)
                    {
                        EnemyY += speed * 1.5f;
                    }
                    else
                    {
                        AntiSwoopRight = false;
                    }
                }

                if (ClosestPlayer == 2)
                {
                    if (PlayerX2 + 6 > EnemyX)
                    {
                        EnemyX += speed * 3;
                    }
                    else
                    {
                        AntiSwoopRight = false;
                    }
                    if (PlayerY2 + 3 > EnemyY)
                    {
                        EnemyY += speed * 1.5f;
                    }
                    else
                    {
                        AntiSwoopRight = false;
                    }
                }
            }
            Enemy.gameObject.transform.position = new Vector3(EnemyX, EnemyY, Enemy.gameObject.transform.position.z);
        }

        if (Shoot)
        {
            if (ClosestPlayer == 1)
            {
                Vector3 directionToFire = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position - transform.position;
                directionToFire.Normalize();
                fireProj.Fire(transform.position, directionToFire, transform.rotation);
                Shoot = false;
            }
            if (ClosestPlayer == 2)
            {
                Vector3 directionToFire = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position - transform.position;
                directionToFire.Normalize();
                fireProj.Fire(transform.position, directionToFire, transform.rotation);
                Shoot = false;
            }
        }

        if (HP <= 0)
            KillEntity();

        if (!Alive)
            Enemy.SetActive(false);
    }


    public void TakeDamage(int amount)
    {
        if (InvicibleTimer < Time.time)
        {
            InvicibleTimer = Time.time + 0.5f;
            Angered = true;
            HP -= amount;
            Singleton_Sound.m_instance.PlayAudioClipOneShot("DamageInflictedSound", 0.2f);
        }
    }

    public void KillEntity()
    {
        Alive = false;
        m_SpawnPickup.CreatePickup();
        Singleton_Game.m_instance.AddScore(3000, new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y));
        Singleton_Game.m_instance.AddGameStat(Singleton_Game.EGameStat.EKills, 1);
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

    public void OnSpawn()
    {
        HP = 4;
        Alive = true;
        Angered = false;
        FindPlayer = true;
    }

    public void OnDeSpawn()
    {
    }
}
