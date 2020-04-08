using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SkeletonAI : MonoBehaviour, IDamageable
{

    public Sprite Head;
    public Sprite Body;
    private SpriteRenderer SpriteRender;


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
    private bool OneTime = true;
    private bool MoveLeft;
    private bool MoveRight;
    private bool FindPlayer;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRender = GetComponent<SpriteRenderer>();
        if (SpriteRender.sprite == null)
            SpriteRender.sprite = Head;

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
            ChangeSprite();
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


            //Allows for the Zombie to be able to jump
            if (Mathf.Abs(rb.velocity.x) < 8f && OnGround)
            {
                if (Random.Range(1, 100) > 70)
                    JumpForce = 10;
                else
                    JumpForce = 7;
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


    void CheckGroundedState()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1f, OnGroundCheckLayerMask);

        if (hit)
        {
            OnGround = true;
        }
        else
        {
            OnGround = false;
        }
    }


    public void TakeDamage(int amount)
    {

        HP -= amount;
        Singleton_Sound.m_instance.PlayAudioClip("DamageInflictedSound");
    }

public void KillEntity()
{

    Alive = false;
        m_SpawnPickup.CreatePickup();
        Singleton_Game.m_instance.AddScore(100);

    }

void ChangeSprite()
    {
        if (SpriteRender.sprite == Head)
        {
            SpriteRender.sprite = Body;
        }
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
}
