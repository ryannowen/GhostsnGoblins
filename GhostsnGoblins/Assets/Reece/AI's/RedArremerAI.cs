﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedArremerAI : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Player;
    public bool alive = true;

    private int RNG;
    private float speed = 0.1f;
    private float RNGtimer = 3;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private float DistanceX;
    private float DistanceY;
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

    //private float Deathtimer = 100f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FindPlayer)
        {
            PlayerX = Player.gameObject.transform.position.x;
            PlayerY = Player.gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
            DistanceY = EnemyY - PlayerY;
            DistanceX = EnemyX - PlayerX;
            FindPlayer = false;
        }

        if (!Angered)
        {
            if (PlayerX + 5 > EnemyX)
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

        if (!InAir && !MoveLeft && !MoveRight && !FlyUp && !FlyDown) 
        {
            if (RNG <= 25 && RNG > 0)
            {
                MoveLeft = true;
            }
           else if (RNG <= 50 && RNG >25 )
            {
                MoveRight = true;
            }
           else if (RNG <= 100 && RNG>50)
            {
                FlyUp = true;
            }
            RNG = 0;
        }


        else if (InAir && !FlyUp && !FlyDown && !Swoop && !AntiSwoopLeft && !AntiSwoopRight)
        {
            if (RNG <=70 && RNG > 30)
            {
                FlyDown = true;
            }
            else if (RNG > 70)
            {
                Swoop = true;
            }
            RNG = 0;
        }

        if (Angered && alive)
        {
            //Will move the Zombie to the left if the player is on the left.
            if (MoveLeft && !InAir)
            {
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
                    EnemyX -= DistanceX/40;
                    EnemyY -= DistanceY/40;
                    if (PlayerX + 0.3f > EnemyX && PlayerX - 0.3f < EnemyX)
                    {
                        AntiSwoopLeft = true;
                        Swoop = false;
                    }
                }
              if (PlayerX > EnemyX && PlayerY < EnemyY)
                {
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
                EnemyX -= speed*5;
                // Enemy.gameObject.transform.position = new Vector3(EnemyX, Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z)
            }
            else if (EnemyX < PlayerX - 8.1f)
            {
                EnemyX += speed * 5;
                // Enemy.gameObject.transform.position = new Vector3(EnemyX, Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z)
            }
            Enemy.gameObject.transform.position = new Vector3(EnemyX, EnemyY, Enemy.gameObject.transform.position.z);
        }

    }

}
