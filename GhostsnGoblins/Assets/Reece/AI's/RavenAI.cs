using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenAI : MonoBehaviour
{
    public bool alive = true;

    private GameObject Enemy;
    private GameObject Player;
    private float speed = 5f;
    private float Heightspeed = 2.5f;
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
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
        }

        if (alive)
        {
            if (PlayerX + 7 > EnemyX)
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
                    if (PlayerX + 7 > EnemyX)
                    {
                        MoveLeft = true;
                    }
                    OneTime = false;
                }


                //Will move the Zombie to the left if the player is on the left.
                if (MoveLeft)
                {
                    Vector3 moveDirection = Vector3.left + Vector3.up;
                    moveDirection.Normalize();
                    moveDirection.x *= speed;

                    if (Time.time + 0.5 > time)
                    {
                        time += 0.5f;
                        Heightspeed = -Heightspeed;
                    }

                    moveDirection.y *= Heightspeed;

                    //EnemyY += Heightspeed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);

                    //Enemy.gameObject.transform.position = new Vector3(EnemyX, EnemyY, Enemy.gameObject.transform.position.z);
                }

                //After how long the DeathTimer is the zombie will stop moving.
                if (Time.time > Deathtimer)
                {
                    alive = false;
                }
            }
        }
        else
            Enemy.SetActive(false);
    }
}
