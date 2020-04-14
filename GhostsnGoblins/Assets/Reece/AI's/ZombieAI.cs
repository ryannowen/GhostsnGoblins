using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour, IDamageable, ISpawn
{
    public bool Alive = true;
    public LayerMask OnGroundCheckLayerMask;

    SpawnPickup m_SpawnPickup = null;
    private GameObject Enemy;
    private GameObject Player;
    private int HP = 1;
    private float Speed = 5f;
    private float JumpForce = 7;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private float Deathtimer;
    private bool Angered = false;
    public bool OnGround = false;
    private bool Jump = false;
    private bool OneTime = true;
    private bool MoveLeft;
    private bool MoveRight;
    private bool FindPlayer;
    private float Origin;
    private float EnemyXOrigin;
    private float wait;
    private bool CheckIfStill;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        m_SpawnPickup = this.gameObject.GetComponent<SpawnPickup>();

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
            PlayerY = Player.gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
            if (PlayerX + 8 > EnemyX && PlayerX - 8 < EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
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
                    Deathtimer = Time.time + 15;
                    Origin = Time.time;
                    wait = Time.time + 0.25f;

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

                    if (Random.Range(1, 100) > 70)
                    {
                        Speed = Speed * 1.5f;
                    }
                    OneTime = false;
                }
            }

            if (Time.time > Origin)
            {
                EnemyXOrigin = EnemyX;
                Origin += 1f;
            }

            if (Time.time > wait)
            {
                wait += 0.5f;
                CheckIfStill = true;
            }


            //Will move the Zombie to the left if the player is on the left.
            if (MoveLeft)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                Vector3 moveDirection = GetDesiredMove();
                moveDirection.Normalize();
                moveDirection.y = rb.velocity.y;
                moveDirection.x *= Speed;

                rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);

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
                Vector3 moveDirection = GetDesiredMove();
                moveDirection.Normalize();
                moveDirection.y = rb.velocity.y;
                moveDirection.x *= Speed;

                rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);

                if (EnemyX == EnemyXOrigin && CheckIfStill)
                {
                    MoveLeft = true;
                    MoveRight = false;
                    CheckIfStill = false;
                }
                else
                    CheckIfStill = false;
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

        if (HP <= 0)
            KillEntity();

        //If zombie is killed will deactivate itself
        if (!Alive)
            Enemy.SetActive(false);
    }



    //Checks to see if the ground is undearneath the zombie
    void CheckGroundedState()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1f, OnGroundCheckLayerMask);

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

    public void TakeDamage(int amount)
    {

        HP -= amount;
        Singleton_Sound.m_instance.PlayAudioClipOneShot("DamageInflictedSound", 0.2f);
    }

    public void KillEntity()
    {

        Alive = false;
        
        // gives a 50% chance to drop a pickup
        if (Random.Range(0,2) == 0)
            m_SpawnPickup.CreatePickup();
        Singleton_Game.m_instance.AddScore(200, new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y));

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
        HP = 1;
        Alive = true;
        Angered = false;
        OneTime = true;
    }

    public void OnDeSpawn()
    {
    }

}

