using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public bool alive = true;

    private GameObject Enemy;
    private GameObject Player;
    private float speed = 5f;
    private float PlayerX;
    private float EnemyX;
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
            EnemyX = Enemy.gameObject.transform.position.x;
            if (PlayerX + 5 > EnemyX)
            {
                Angered = true;
            }
        }



        if (alive && Angered)
        {
            FindPlayer = true;
            if (FindPlayer && OneTime)
            {
                PlayerX = Player.gameObject.transform.position.x;
                EnemyX = Enemy.gameObject.transform.position.x;
                Deathtimer = Time.time + 5;

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
                Vector3 moveDirection = Vector3.left;
                moveDirection.Normalize();
                moveDirection.y = rb.velocity.y;
                moveDirection.x *= speed;

                rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                //EnemyX -= speed;
            }
            //Will move the Zombie to the right if the player is on the right
            else if (MoveRight)
            {
                Vector3 moveDirection = Vector3.right;
                moveDirection.Normalize();
                moveDirection.y = rb.velocity.y;
                moveDirection.x *= speed;

                rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                //EnemyX += speed;
            }
            //After how long the DeathTimer is the zombie will stop moving.
            if (Time.time > Deathtimer)
            {
                alive = false;
            }
            //Enemy.gameObject.transform.position = new Vector3(EnemyX, Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z);
        }
        if (!alive)
            Enemy.SetActive(false);
    }
}
