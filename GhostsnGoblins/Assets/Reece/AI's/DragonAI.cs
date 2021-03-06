﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAI : MonoBehaviour, IDamageable, ISpawn
{

    [SerializeField] private GameObject Bullet = null;
    public bool Alive = true;

    SpawnPickup m_SpawnPickup = null;
    private GameObject Enemy = null;
    private GameObject Player = null;
    private int HP = 3;
    private float speed = 5f;
    private float PlayerX;
    private float PlayerY;
    private float PlayerX2;
    private float PlayerY2;
    private float Distance;
    private float Distance2;
    private float EnemyX;
    private float EnemyY;
    private Vector2 EnemyPos;
    private Vector2 PlayerPos;
    private bool Angered = false;
    private bool MoveLeft;
    private bool MoveRight;
    private bool FlyUp;
    private bool FlyDown;
    private float MoveTime;
    private bool Shoot;
    private float ShootTime;
    private int RNG;
    private float RNGTimer;
    private float InvicibleTimer;
    private bool OneTime = true;
    private float Origin;
    private float EnemyXOrigin;
    private float wait;
    private bool CheckIfStill;

    private Rigidbody2D rb;
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

        rb = this.gameObject.GetComponent<Rigidbody2D>();

        m_SpawnPickup = this.gameObject.GetComponent<SpawnPickup>();

        Enemy = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {


        if (Time.time > RNGTimer)
        {
            RNG = Random.Range(2, 100);
            RNGTimer += 0.3f;
        }

        if (!Angered)
        {
            PlayerX = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.x;
            PlayerY = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.y;
            PlayerX2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.x;
            PlayerY2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;

            if (PlayerX + 5 > EnemyX && PlayerX - 5 < EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
            {
                Angered = true;
            }
            if (PlayerX2 + 5 > EnemyX && PlayerX2 - 5 < EnemyX && PlayerY2 + 3 > EnemyY && PlayerY2 - 3 < EnemyY)
            {
                Angered = true;
            }
        }


        if (Alive && Angered)
        {
            EnemyPos = new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y);
            PlayerPos = new Vector2(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y);
           

            if (OneTime)
            {
                EnemyX = Enemy.gameObject.transform.position.x;
                EnemyY = Enemy.gameObject.transform.position.y;
                PlayerX = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.x;
                PlayerX2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.x;
                ShootTime = Time.time;
                Origin = Time.time;
                wait = Time.time + 0.25f;

                Distance = EnemyX - PlayerX;
                Distance2 = EnemyX - PlayerX2;

                if (Distance < 0)
                    Distance = -Distance;
                if (Distance2 < 0)
                    Distance2 = -Distance2;

                if (Distance < Distance2)
                {
                    //Finds if the player is on the left.
                    if (PlayerX < EnemyX)
                    {
                        MoveLeft = true;
                    }

                    //Finds if the player is on the right.
                    if (PlayerX > EnemyX)
                    {
                        MoveRight = true;
                    }
                }

                if (Distance2 < Distance)
                {
                    //Finds if the player is on the left.
                    if (PlayerX2 < EnemyX)
                    {
                        MoveLeft = true;
                    }

                    //Finds if the player is on the right.
                    if (PlayerX2 > EnemyX)
                    {
                        MoveRight = true;
                    }
                }

                OneTime = false;
            }

            if (Time.time > ShootTime)
            {
                ShootTime += 3;
                Shoot = true;
            }

            if (Time.time > Origin)
            {
                EnemyXOrigin = EnemyX;
                Origin += 1f;
            }

            if (Time.time > wait)
            {
                wait += 1.5f;
                CheckIfStill = true;
            }

            if (RNG > 0)
            {
                MoveTime = Time.time + 0.2f;
                if (RNG <= 90 && RNG > 50)
                    FlyUp = true;

                else if (RNG <= 100 && RNG > 90)
                    FlyDown = true;
                RNG = 0;
            }

            if (MoveTime < Time.time)
            {
                FlyUp = false;
                FlyDown = false;
            }

            if (EnemyPos.y < PlayerPos.y + 0.5)
            {
                FlyDown = false;
                FlyUp = true;
            }
            else if (EnemyPos.y > PlayerPos.y + 2)
            {
                FlyUp = false;
                FlyDown = true;
            }

            if (Shoot)
            {
                Vector3 directionToFire = Player.transform.position - transform.position;
                directionToFire.Normalize();
                fireProj.Fire(transform.position, directionToFire, transform.rotation);
                Shoot = false;
            }

            //Will move the Zombie to the left if the player is on the left.
            if (MoveLeft)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                if (EnemyPos.x > PlayerPos.x - 8)
                {
                    Vector3 moveDirection = Vector3.left;
                    moveDirection.Normalize();
                    moveDirection.y = 0;
                    moveDirection.x *= speed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                }
                else
                {
                    MoveLeft = false;
                    MoveRight = true;
                }

                if (EnemyX == EnemyXOrigin && CheckIfStill)
                {
                    MoveLeft = false;
                    MoveRight = true;
                    CheckIfStill = false;
                }
                else
                    CheckIfStill = false;
            }

            //Will move the Zombie to the right if the player is on the right
            else if (MoveRight)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                if (EnemyPos.x < PlayerPos.x + 8)
                {
                    Vector3 moveDirection = Vector3.right;
                    moveDirection.Normalize();
                    moveDirection.y = 0;
                    moveDirection.x *= speed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                }
                else
                {
                    MoveLeft = true;
                    MoveRight = false;
                }

                if (EnemyX == EnemyXOrigin && CheckIfStill)
                {
                    MoveLeft = true;
                    MoveRight = false;
                    CheckIfStill = false;
                }
                else
                    CheckIfStill = false;
            }

            if (FlyUp)
            {
                Vector3 moveDirection = Vector3.up;
                moveDirection.Normalize();
                moveDirection.y *= speed;

                rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 0.3f);
            }

            else if (FlyDown)
            {
                Vector3 moveDirection = Vector3.down;
                moveDirection.Normalize();
                moveDirection.y *= speed;

                rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 0.3f);
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
        Angered = true;
        Alive = false;
        m_SpawnPickup.CreatePickup();
        Singleton_Game.m_instance.AddScore(2000, EnemyPos);
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
        HP = 6;
        Alive = true;
        Angered = false;
        OneTime = true;
    }

    public void OnDeSpawn()
    {
    }
}

