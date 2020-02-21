﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenAI : MonoBehaviour
{
    public bool alive = true;

    private GameObject Enemy;
    private GameObject Player;
    private float speed = 0.08f;
    private float Heightspeed = 0.04f;
    private float time = 0.2f;
    private float PlayerX;
    private float EnemyX;
    private float EnemyY;
    private float Deathtimer;
    private bool Angered = false;
    private bool FindPlayer;
    private bool OneTime = true;
    private bool MoveLeft;
    private bool MoveRight;


    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        Enemy = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Angered)
        {
            PlayerX = Player.gameObject.transform.position.x;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
        }

        if (alive)
        {
            if (PlayerX + 10 > EnemyX)
            {
                Angered = true;
            }
            if (Angered)
            {
                FindPlayer = true;
                if (FindPlayer && OneTime)
                {
                    PlayerX = Player.gameObject.transform.position.x;
                    EnemyX = Enemy.gameObject.transform.position.x;
                    EnemyY = Enemy.gameObject.transform.position.y;
                    Deathtimer = Time.time + 7;

                    //Finds if the player is on the left.
                    if (PlayerX + 10 > EnemyX)
                    {
                        MoveLeft = true;
                    }
                    OneTime = false;
                }


                //Will move the Zombie to the left if the player is on the left.
                if (MoveLeft)
                {
                    EnemyX -= speed;

                    if (Time.time + 0.5 > time)
                    {
                        time += 0.5f;
                        Heightspeed = -Heightspeed;
                    }

                    EnemyY += Heightspeed;

                    Enemy.gameObject.transform.position = new Vector3(EnemyX, EnemyY, Enemy.gameObject.transform.position.z);
                }

                //After how long the DeathTimer is the zombie will stop moving.
                if (Time.time > Deathtimer)
                {
                    alive = false;
                }
            }
        }
    }
}
