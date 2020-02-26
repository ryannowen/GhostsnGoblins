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
    private float JumpForce = 7f;
    private bool OneTime = true;
    private bool Angered = false;
    private bool MoveLeft;
    private bool MoveRight;
    private bool FindPlayer;


    public bool m_Grounded = false;
    public LayerMask m_GroundCheckLayerMask;

    Rigidbody2D rb;
    BoxCollider2D m_PlayerCollider;




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
            if (FindPlayer)
            {
                PlayerX = Player.gameObject.transform.position.x;
                EnemyX = Enemy.gameObject.transform.position.x;
                Deathtimer = Time.time + 5;
                if (OneTime)
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

                    if (Random.Range(2, 101) > 70)
                    {
                        speed = speed * 1.5f;
                    }
                    OneTime = false;
                }
            }

            //Will move the Zombie to the left if the player is on the left.
            if (MoveLeft)
            {
                print("left");
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
                print("right");
                Vector3 moveDirection = Vector3.right;
                moveDirection.Normalize();
                moveDirection.y = rb.velocity.y;
                moveDirection.x *= speed;

                rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                //EnemyX += speed;
            }


            if (GetComponent<BoxCollider2D>())
            {
                m_PlayerCollider = GetComponent<BoxCollider2D>();
            }
            else
            {
                m_PlayerCollider = this.gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
            }

            CheckGroundedState();

            print(rb.velocity.x);
            if (Mathf.Abs(rb.velocity.x) < 1f && m_Grounded)
            {
                print("hi");
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(rb.velocity.x, JumpForce), 1f);
                m_Grounded = false;
            }
           
            if (JumpForce == 0)
                JumpForce = 7f;

            //After how long the DeathTimer is the zombie will stop moving.
            if (Time.time > Deathtimer)
            {
                print("dead");
                alive = false;
            }
        }
        if (!alive)
            Enemy.SetActive(false);
    }
    void CheckGroundedState()
    {

        //RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Vector3.down, (m_PlayerCollider.size.y / 2 + 0.05f) * transform.localScale.x, m_GroundCheckLayerMask);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 0.55f, m_GroundCheckLayerMask);
       // Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Vector3.down * (m_PlayerCollider.size.y / 2 + 0.02f) * transform.localScale.x, Color.red);
        Debug.DrawRay(transform.position, Vector3.down * 0.55f, Color.red);

        if (hit)
            m_Grounded = true;
        else
            m_Grounded = false;

    }
}

