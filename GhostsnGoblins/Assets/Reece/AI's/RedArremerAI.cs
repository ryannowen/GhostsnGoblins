﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedArremerAI : MonoBehaviour, IDamageable, ISpawn
{
    public bool Alive = true;

    [SerializeField] private GameObject Bullet = null;
    SpawnPickup m_SpawnPickup = null;
    private GameObject Enemy;
    private GameObject Player;
    private int RNG;
    private int HP = 3;
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
    private bool MoveLeft;
    private bool MoveRight;
    private bool Swoop;
    private bool FlyUp;
    private bool FlyDown;
    private bool Angered;
    private bool InAir;
    private bool AntiSwoopLeft;
    private bool AntiSwoopRight;
    private bool Shoot;
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
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;

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

        if (Angered)
        {
            if (PlayerX > EnemyX + 12 && PlayerX < EnemyX - 12 && PlayerY > EnemyY + 7 && PlayerY < EnemyY - 7 && PlayerX2 > EnemyX + 12 && PlayerX2 < EnemyX - 12 && PlayerY2 > EnemyY + 7 && PlayerY2 < EnemyY - 7)
            {
                Angered = false;
            }
        }

        if (Time.time > RNGtimer)
        {
            RNG = Random.Range(2, 100);
            RNGtimer += 3;
            FindPlayer = true;
        }

        if (!InAir && !MoveLeft && !MoveRight && !FlyUp && !FlyDown)
        {
            if (RNG <= 25 && RNG > 0)
            {
                MoveLeft = true;
            }
            else if (RNG <= 50 && RNG > 25)
            {
                MoveRight = true;
            }
            else if (RNG <= 100 && RNG > 50)
            {
                FlyUp = true;
            }
            RNG = 0;
        }


        else if (InAir && !FlyUp && !FlyDown && !Swoop && !AntiSwoopLeft && !AntiSwoopRight && !Shoot)
        {
            if (RNG <= 70 && RNG > 30)
            {
                FlyDown = true;
            }
            else if (RNG > 70 && RNG <= 90)
            {
                Swoop = true;
            }
            else if (RNG > 90)
            {
                Shoot = true;
            }
            RNG = 0;
        }

        if (Angered && Alive)
        {
            if (ClosestPlayer == 1)
            {
                //Will move the Zombie to the left if the player is on the left.
                if (MoveLeft && !InAir)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (EnemyX > PlayerX - 8)
                    {
                        EnemyX -= speed;
                    }
                    else
                        MoveLeft = false;
                }
                //Will move the Zombie to the right if the player is on the right
                else if (MoveRight && !InAir)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    if (EnemyX < PlayerX + 8)
                    {
                        EnemyX += speed;
                    }
                    else
                        MoveRight = false;
                }


                if (FlyUp)
                {
                    if (EnemyY < PlayerY + 4)
                    {
                        EnemyY += speed;
                        InAir = true;
                    }
                    else
                        FlyUp = false;
                }
                else if (FlyDown)
                {
                    if (EnemyY > PlayerY)
                    {
                        EnemyY -= speed;
                        InAir = false;
                    }
                    else
                        FlyDown = false;
                }


                if (Swoop && InAir)
                {
                    if (PlayerX < EnemyX && PlayerY < EnemyY)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        EnemyX -= DistanceX / 40;
                        EnemyY -= DistanceY / 40;
                        if (PlayerX + 0.3f > EnemyX && PlayerX - 0.3f < EnemyX)
                        {
                            AntiSwoopLeft = true;
                            Swoop = false;
                        }
                    }
                    if (PlayerX > EnemyX && PlayerY < EnemyY)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        EnemyX -= DistanceX / 40;
                        EnemyY -= DistanceY / 40;
                        if (PlayerX + 0.3f > EnemyX && PlayerX - 0.3f < EnemyX)
                        {
                            AntiSwoopRight = true;
                            Swoop = false;
                        }
                    }
                }


                if (AntiSwoopLeft)
                {
                    if (PlayerX - 8 < EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        EnemyX -= speed * 4;
                    }
                    else
                    {
                        AntiSwoopLeft = false;
                    }
                    if (PlayerY + 4 > EnemyY)
                    {
                        EnemyY += speed * 2;
                    }
                    else
                    {
                        AntiSwoopLeft = false;
                    }
                }


                if (AntiSwoopRight)
                {
                    if (PlayerX + 8 > EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        EnemyX += speed * 4;
                    }
                    else
                    {
                        AntiSwoopRight = false;
                    }
                    if (PlayerY + 4 > EnemyY)
                    {
                        EnemyY += speed * 2;
                    }
                    else
                    {
                        AntiSwoopRight = false;
                    }
                }
                if (EnemyX > PlayerX + 8.1f)
                {
                    EnemyX -= speed * 5;
                    // Enemy.gameObject.transform.position = new Vector3(EnemyX, Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z)
                }
                else if (EnemyX < PlayerX - 8.1f)
                {
                    EnemyX += speed * 5;
                    // Enemy.gameObject.transform.position = new Vector3(EnemyX, Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z)
                }
                Enemy.gameObject.transform.position = new Vector3(EnemyX, EnemyY, Enemy.gameObject.transform.position.z);
            }

            if (ClosestPlayer == 2)
            {
                //Will move the Zombie to the left if the player is on the left.
                if (MoveLeft && !InAir)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (EnemyX > PlayerX2 - 8)
                    {
                        EnemyX -= speed;
                    }
                    else
                        MoveLeft = false;
                }
                //Will move the Zombie to the right if the player is on the right
                else if (MoveRight && !InAir)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    if (EnemyX < PlayerX2 + 8)
                    {
                        EnemyX += speed;
                    }
                    else
                        MoveRight = false;
                }


                if (FlyUp)
                {
                    if (EnemyY < PlayerY2 + 4)
                    {
                        EnemyY += speed;
                        InAir = true;
                    }
                    else
                        FlyUp = false;
                }
                else if (FlyDown)
                {
                    if (EnemyY > PlayerY2)
                    {
                        EnemyY -= speed;
                        InAir = false;
                    }
                    else
                        FlyDown = false;
                }


                if (Swoop && InAir)
                {
                    if (PlayerX2 < EnemyX && PlayerY2 < EnemyY)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        EnemyX -= DistanceX / 40;
                        EnemyY -= DistanceY / 40;
                        if (PlayerX2 + 0.3f > EnemyX && PlayerX2 - 0.3f < EnemyX)
                        {
                            AntiSwoopLeft = true;
                            Swoop = false;
                        }
                    }
                    if (PlayerX2 > EnemyX && PlayerY2 < EnemyY)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        EnemyX -= DistanceX / 40;
                        EnemyY -= DistanceY / 40;
                        if (PlayerX2 + 0.3f > EnemyX && PlayerX2 - 0.3f < EnemyX)
                        {
                            AntiSwoopRight = true;
                            Swoop = false;
                        }
                    }
                }


                if (AntiSwoopLeft)
                {
                    if (PlayerX2 - 8 < EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        EnemyX -= speed * 4;
                    }
                    else
                    {
                        AntiSwoopLeft = false;
                    }
                    if (PlayerY2 + 4 > EnemyY)
                    {
                        EnemyY += speed * 2;
                    }
                    else
                    {
                        AntiSwoopLeft = false;
                    }
                }


                if (AntiSwoopRight)
                {
                    if (PlayerX2 + 8 > EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        EnemyX += speed * 4;
                    }
                    else
                    {
                        AntiSwoopRight = false;
                    }
                    if (PlayerY2 + 4 > EnemyY)
                    {
                        EnemyY += speed * 2;
                    }
                    else
                    {
                        AntiSwoopRight = false;
                    }
                }
                if (EnemyX > PlayerX2 + 8.1f)
                {
                    EnemyX -= speed * 5;
                    // Enemy.gameObject.transform.position = new Vector3(EnemyX, Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z)
                }
                else if (EnemyX < PlayerX2 - 8.1f)
                {
                    EnemyX += speed * 5;
                    // Enemy.gameObject.transform.position = new Vector3(EnemyX, Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z)
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
        Singleton_Game.m_instance.AddScore(1000, new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y));
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
        HP = 3;
        Alive = true;
        Angered = false;
        FindPlayer = true;
    }

    public void OnDeSpawn()
    {
    }
}
