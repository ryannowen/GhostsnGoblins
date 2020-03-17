using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAI : MonoBehaviour, IDamageable
{

    [SerializeField] private GameObject Bullet = null;
    public bool alive = true;

    private GameObject Enemy = null;
    private GameObject Player = null;
    private int HP = 10;
    private float speed = 5f;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private Vector2 EnemyPos;
    private Vector2 PlayerPos;
    private bool Angered = false;
    private bool MoveLeft;
    private bool MoveRight;
    private bool FlyUp;
    private bool FlyDown;
    private float MoveTime;
    private bool Shoot;
    private int ShootTime = 3;
    private int RNG;
    private float RNGTimer;

    private Rigidbody2D rb;
    private FireProjectile fireProj;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        if (Bullet == null)
            Bullet = (GameObject)Resources.Load("Prefabs/Bullet") as GameObject;

        fireProj = this.gameObject.GetComponent<FireProjectile>();
        fireProj.SetProjectile(Bullet);

        rb = this.gameObject.GetComponent<Rigidbody2D>();


        Enemy = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > ShootTime)
        {
            ShootTime += 3;
            Shoot = true;
        }

        if (Time.time > RNGTimer)
        {
            RNG = Random.Range(2, 100);
            RNGTimer += 2;
        }

        //print(MoveTime);
        //print(Time.time);
        print(RNG);

        if (!Angered)
        {
            PlayerX = Player.gameObject.transform.position.x;
            PlayerY = Player.gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
            if (PlayerX + 5 > EnemyX && PlayerX - 5 < EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
            {
                Angered = true;
            }
        }


            if (alive && Angered)
            {
                EnemyPos = new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y);
                PlayerPos = new Vector2(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y);

            if (RNG > 0)
            {
                MoveTime = Time.time + 1;
                if (RNG <= 25 && RNG > 0)
                    MoveLeft = true;

                else if (RNG <= 50 && RNG > 25)
                    MoveLeft = true;

                else if (RNG <= 75 && RNG > 50)
                    FlyUp = true;

                else if (RNG <= 100 && RNG > 75)
                    FlyDown = true;
                RNG = 0;
            }

            if (MoveTime < Time.time)
            {
                MoveLeft = false;
                MoveRight = false;
                FlyUp = false;
                FlyDown = false;
            }

            if (Shoot)
                {
                    if (EnemyPos.x > PlayerPos.x)
                        fireProj.Fire(transform.position, Vector3.left, transform.rotation);
                    else
                        fireProj.Fire(transform.position, Vector3.right, transform.rotation);
                    Shoot = false;
                }
                 
                //Will move the Zombie to the left if the player is on the left.
                if (MoveLeft)
                {
                        Vector3 moveDirection = Vector3.left;
                        moveDirection.Normalize();
                        moveDirection.y = 0;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                        print("Hey");   
                }

                //Will move the Zombie to the right if the player is on the right
                else if (MoveRight)
                {
                        Vector3 moveDirection = Vector3.right;
                        moveDirection.Normalize();
                        moveDirection.y = 0;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                }

            else if (FlyUp)
            {
                    Vector3 moveDirection = Vector3.up;
                    moveDirection.Normalize();
                    moveDirection.y *= speed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 0.1f);
            }

            else if (FlyDown)
            {
                    Vector3 moveDirection = Vector3.down;
                    moveDirection.Normalize();
                    moveDirection.y *= speed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 0.1f);
            }
        }
        

        if (HP <= 0)
            KillEntity();

        if (!alive)
            Enemy.SetActive(false);
    }

    public void TakeDamage(int amount)
    {

        HP -= amount;

    }

    public void KillEntity()
    {

        alive = false;

    }
}

