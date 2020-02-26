﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodyPigAI : MonoBehaviour
{
    public bool alive = true;

    private GameObject Enemy;
    private GameObject Player;
    private float speed = 5f;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private Vector2 EnemyPos;
    private bool setTarget = true;
    private Vector2 targetPos;
    private float Deathtimer;
    private bool OneTime = true;
    private bool Angered = false;
    private bool MoveLeft;
    private bool MoveRight;
    private bool FindPlayer;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        rb = this.gameObject.GetComponent<Rigidbody2D>();

        Enemy = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    { 

        if (!Angered)
        {
            PlayerX = Player.gameObject.transform.position.x;
            PlayerY = Player.gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
            EnemyPos = new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y);
            if (PlayerX + 5 > EnemyX)
            {  
                Angered = true;
            }
        }  

        if (alive && Angered)
        {
            
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
            EnemyPos = new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y);
            PlayerY = Player.gameObject.transform.position.y;
            FindPlayer = true;
            if (FindPlayer && OneTime)
            {
                PlayerX = Player.gameObject.transform.position.x;
                
                Deathtimer = Time.time + 30;

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

                if (Random.Range(2, 101) > 70)
                {
                    speed = speed * 1.5f;
                }
                OneTime = false;
            }

            //Will move the Zombie to the left if the player is on the left.
            if (MoveLeft)
            {
                if (EnemyPos.x > PlayerX - 8)
                {
                    //print("Moving Left");
                    Vector3 moveDirection = Vector3.left;
                    moveDirection.Normalize();
                    moveDirection.y = 0;
                    moveDirection.x *= speed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                }
                else if (EnemyPos.y > PlayerY)
                {
                    if(setTarget)
                    {

                        targetPos = new Vector2(EnemyPos.x, EnemyPos.y - 1);
                        setTarget = false;
                    }
                    Vector2 direction = targetPos - EnemyPos;
                    direction.Normalize();

                    EnemyPos = EnemyPos + (direction * speed);

                    //Vector3 moveDirection = Vector3.down;
                    //moveDirection.Normalize();
                    //moveDirection.y *= speed;

                    //rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 0.1f);
                    if (Vector2.Distance(EnemyPos, targetPos) < 0.1f)
                    {
                        setTarget = true;
                        MoveLeft = false;
                        MoveRight = true;
                    }
                }
                else
                {
                    if (setTarget)
                    {

                        targetPos = new Vector2(EnemyPos.x, EnemyPos.y + 1);
                        setTarget = false;
                    }

                    Vector2 direction = targetPos - EnemyPos;
                    direction.Normalize();

                    EnemyPos = EnemyPos + (direction * speed);

                    //Vector3 moveDirection = Vector3.up;
                    //moveDirection.Normalize();
                    //moveDirection.y *= speed;

                    //rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 0.1f);
                    if (Vector2.Distance(EnemyPos, targetPos) < 0.1f)
                    {
                        setTarget = true;
                        MoveLeft = false;
                        MoveRight = true;
                    }
                }

            }
            //Will move the Zombie to the right if the player is on the right
            else if (MoveRight)
            {
                if (EnemyPos.x < PlayerX + 8)
                {
                    //print(EnemyX + "    " + PlayerX + "    " + PlayerX + 8);
                    Vector3 moveDirection = Vector3.right;
                    moveDirection.Normalize();
                    moveDirection.y = 0;
                    moveDirection.x *= speed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                }
                else if (EnemyPos.y > PlayerY)
                {
                    if (setTarget)
                    {

                        targetPos = new Vector2(EnemyPos.x, EnemyPos.y - 1);
                        setTarget = false;
                    }

                    Vector2 direction = targetPos - EnemyPos;
                    direction.Normalize();

                    EnemyPos = EnemyPos + (direction * speed);

                    //Vector3 moveDirection = Vector3.down;
                    //moveDirection.Normalize();
                    //moveDirection.y *= speed;

                    //rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 0.1f);
                    if (Vector2.Distance(EnemyPos, targetPos) < 0.1f)
                    {
                        setTarget = true;
                        MoveLeft = true;
                        MoveRight = false;
                    }
                }
                else
                {
                    if (setTarget)
                    {

                        targetPos = new Vector2(EnemyPos.x, EnemyPos.y + 1);
                        setTarget = false;
                    }

                    Vector2 direction = targetPos - EnemyPos;
                    direction.Normalize();

                    EnemyPos = EnemyPos + (direction * speed);

                    //Vector3 moveDirection = Vector3.up;
                    //moveDirection.Normalize();
                    //moveDirection.y *= speed;

                    //rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 0.1f);
                    if (Vector2.Distance(EnemyPos, targetPos) < 0.1f)
                    {
                        setTarget = true;
                        MoveLeft = true;
                        MoveRight = false;
                    }
                }
            }
            //After how long the DeathTimer is the zombie will stop moving.
            if (Time.time > Deathtimer)
            {
                alive = false;
            }

            //Enemy.gameObject.transform.position = new Vector3(EnemyX, EnemyY, Enemy.gameObject.transform.position.z);
        }
        if (!alive)
            Enemy.SetActive(false);
    }
}

