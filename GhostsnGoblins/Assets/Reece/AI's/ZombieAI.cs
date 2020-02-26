using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public bool Alive = true;
    public LayerMask OnGroundCheckLayerMask;

    private GameObject Enemy;
    private GameObject Player;
    private float Speed = 5f;
    private float JumpForce = 7;
    private float PlayerX;
    private float EnemyX;
    private float Deathtimer;
    private bool Angered = false;
    public bool OnGround = false;
    private bool Jump = false;
    private bool OneTime = true;
    private bool MoveLeft;
    private bool MoveRight;
    private bool FindPlayer;
  
    Rigidbody2D rb;
    BoxCollider2D PlayerCollide;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        rb = this.gameObject.GetComponent<Rigidbody2D>();

        Enemy = this.gameObject;
    }

   Vector3 GetDesiredMove()
    {
        Vector3 v = Vector3.zero;
        if (MoveLeft)
            v += Vector3.left;
        else
            v += Vector3.right;
        v.Normalize();
        return v;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Will find if the player is within a certain range of the zombie.
        if (!Angered)
        {
            PlayerX = Player.gameObject.transform.position.x;
            EnemyX = Enemy.gameObject.transform.position.x;
            if (PlayerX + 8 > EnemyX && PlayerX - 8 < EnemyX)
            {
                Angered = true;
            }  
        }

        GetDesiredMove();

        //If the Zombie is alive and within a certain range it will attack the player
        if (Alive && Angered)
        {
            FindPlayer = true;
            if (FindPlayer)
            {
                PlayerX = Player.gameObject.transform.position.x;
                EnemyX = Enemy.gameObject.transform.position.x;

                //A Onetime run to tell the zombie it needs to go a certain direction from where the player is when the zombie is angered.
                if (OneTime)
                {
                    //Sets the Deathtimer of the zombie 5 seconds after it spawns.
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
                        Speed = Speed * 1.5f;
                    }
                    OneTime = false;
                }
            }

            //Will move the Zombie to the left if the player is on the left.
            if (MoveLeft)
            {
                Vector3 moveDirection = GetDesiredMove();
                moveDirection.Normalize();
                moveDirection.y = rb.velocity.y;
                moveDirection.x *= Speed;

                rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
            }

            //Will move the Zombie to the right if the player is on the right
            else if (MoveRight)
            {
                Vector3 moveDirection = GetDesiredMove();
                moveDirection.Normalize();
                moveDirection.y = rb.velocity.y;
                moveDirection.x *= Speed;

                rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
            }

            CheckGroundedState();
            CheckSideState();

            //Allows for the Zombie to be able to jump
            if (Mathf.Abs(rb.velocity.x) < 8f && OnGround && Jump)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(rb.velocity.x, JumpForce), 1f);
            }
           
            if (JumpForce == 0)
                JumpForce = 7;

            //After how long the DeathTimer is the zombie will stop moving.
            if (Time.time > Deathtimer)
            {
                Alive = false;
            }
        }

        //If zombie is killed will deactivate itself
        if (!Alive)
            Enemy.SetActive(false);
    }

 

//Checks to see if the ground is undearneath the zombie
    void CheckGroundedState()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 0.55f, OnGroundCheckLayerMask);

        if (hit)
            OnGround = true;
        else
            OnGround = false;
    }

    //Checks to see if there is a wall withing a certain distance of the Zombie in which it will know to jump or not
    void CheckSideState()
    {
        RaycastHit2D Left = Physics2D.Raycast(transform.position, Vector3.left, (transform.localScale.x / 2 + 2f), OnGroundCheckLayerMask);
        RaycastHit2D Right = Physics2D.Raycast(transform.position, Vector3.right, (transform.localScale.x / 2 + 2f), OnGroundCheckLayerMask);

        if (Left || Right)
            Jump = true;
        else
            Jump = false;

    }
}

