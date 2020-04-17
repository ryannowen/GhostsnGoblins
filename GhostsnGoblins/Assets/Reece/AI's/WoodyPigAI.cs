using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodyPigAI : MonoBehaviour, IDamageable, ISpawn
{

    [SerializeField] private GameObject Bullet = null;
    public bool Alive = true;

    SpawnPickup m_SpawnPickup = null;
    private GameObject Enemy = null;
    private GameObject Player = null;
    private int HP = 1;
    private int RNG;
    private float speed = 3f;
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
    private bool playerLevelReached = false;
    private bool Shoot;
    private float ShootTime = 3;

    private Rigidbody2D rb;
    private FireProjectile fireProj;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        if (Bullet == null)
            Bullet = (GameObject)Resources.Load("Prefabs/Bullet") as GameObject;

        m_SpawnPickup = this.gameObject.GetComponent<SpawnPickup>();

        fireProj = this.gameObject.GetComponent<FireProjectile>();
        fireProj.SetProjectile(Bullet);

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
            if (PlayerX + 5 > EnemyX && PlayerX - 5 < EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
            {
                Angered = true;
            }
        }

        if (Alive && Angered)
        {
            EnemyPos = new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y);
            PlayerY = Player.gameObject.transform.position.y;
            FindPlayer = true;
           

            if (FindPlayer && OneTime)
            {
                EnemyX = Enemy.gameObject.transform.position.x;
                EnemyY = Enemy.gameObject.transform.position.y;
                PlayerX = Player.gameObject.transform.position.x;
                ShootTime = Time.time;

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

            if (Time.time > ShootTime)
            {
                ShootTime += 3;
                RNG = Random.Range(1, 3);
                Shoot = true;
            }


            if (Shoot)
            {
                if (RNG == 1)
                    fireProj.Fire(transform.position, Vector3.down, transform.rotation);
                else if (RNG == 2)
                    if (MoveLeft)
                        fireProj.Fire(transform.position, Vector3.left, transform.rotation);
                    else
                        fireProj.Fire(transform.position, Vector3.right, transform.rotation);
                Shoot = false;
            }

            //Will move the Zombie to the left if the player is on the left.
            if (MoveLeft)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                if (EnemyPos.x > PlayerX - 8)
                {
                    //print("Moving Left");
                    Vector3 moveDirection = Vector3.left;
                    moveDirection.Normalize();
                    moveDirection.y = 0;
                    moveDirection.x *= speed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                }
                else if (!playerLevelReached) //if (EnemyPos.y > PlayerY + 2)
                {
                    if (setTarget)
                    {
                        targetPos = new Vector2(EnemyPos.x, EnemyPos.y - 1);
                        setTarget = false;
                    }
                    Vector2 direction = targetPos - EnemyPos;
                    direction.Normalize();

                    // EnemyPos = EnemyPos + (direction * speed);

                    Vector3 moveDirection = Vector3.down;
                    moveDirection.Normalize();
                    moveDirection.y *= speed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 0.1f);
                    if (Vector2.Distance(new Vector2(0, EnemyPos.y), new Vector2(0, targetPos.y)) < 0.1f)
                    {
                        setTarget = true;
                        MoveLeft = false;
                        MoveRight = true;
                        if (EnemyPos.y <= PlayerY + 0.5)
                        {
                            playerLevelReached = true;
                        }
                    }
                }
                else if (playerLevelReached)
                {
                    if (setTarget)
                    {

                        targetPos = new Vector2(EnemyPos.x, EnemyPos.y + 1);
                        setTarget = false;
                    }

                    Vector2 direction = targetPos - EnemyPos;
                    direction.Normalize();
                    direction.y *= speed;

                    // EnemyPos = EnemyPos + (direction * speed);

                    Vector3 moveDirection = Vector3.up;
                    moveDirection.Normalize();
                    moveDirection.y *= speed;
                    rb.velocity = Vector3.Lerp(rb.velocity, direction, 0.1f);
                    if (Vector2.Distance(new Vector2(0, EnemyPos.y), new Vector2(0, targetPos.y)) < 0.1f)
                    {
                        setTarget = true;
                        MoveLeft = false;
                        MoveRight = true;
                        if (EnemyPos.y <= PlayerY)
                            playerLevelReached = false;
                    }
                }


            }

            //Will move the Zombie to the right if the player is on the right
            else if (MoveRight)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                if (EnemyPos.x < PlayerX + 8)
                {
                    Vector3 moveDirection = Vector3.right;
                    moveDirection.Normalize();
                    moveDirection.y = 0;
                    moveDirection.x *= speed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                }
                else if (!playerLevelReached) // if (EnemyPos.y > PlayerY + 2)
                {
                    if (setTarget)
                    {

                        targetPos = new Vector2(EnemyPos.x, EnemyPos.y - 1); //line
                        setTarget = false;
                    }

                    Vector2 direction = targetPos - EnemyPos; //line
                    direction.Normalize();

                    // EnemyPos = EnemyPos + (direction * speed); //line

                    Vector3 moveDirection = Vector3.down;
                    moveDirection.Normalize();
                    moveDirection.y *= speed;

                    rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 0.1f);
                    if (Vector2.Distance(new Vector2(0, EnemyPos.y), new Vector2(0, targetPos.y)) < 0.1f) //line
                    {
                        setTarget = true;
                        MoveLeft = true;
                        MoveRight = false;
                        if (EnemyPos.y <= PlayerY + 0.5)
                        {
                            playerLevelReached = true;
                        }
                    }

                }
                else if (playerLevelReached)
                {
                    if (setTarget)
                    {

                        targetPos = new Vector2(EnemyPos.x, EnemyPos.y + 1);
                        setTarget = false;
                    }

                    Vector2 direction = targetPos - EnemyPos;
                    direction.Normalize();
                    direction.y *= speed;

                    // EnemyPos = EnemyPos + (direction * speed);

                    Vector3 moveDirection = Vector3.up;
                    moveDirection.Normalize();
                    moveDirection.y *= speed;
                    moveDirection.y *= speed;
                    playerLevelReached = true;
                    rb.velocity = Vector3.Lerp(rb.velocity, direction, 0.1f);
                    if (Vector2.Distance(new Vector2(0, EnemyPos.y), new Vector2(0, targetPos.y)) < 0.1f)
                    {
                        setTarget = true;
                        MoveLeft = true;
                        MoveRight = false;
                        if (EnemyPos.y >= PlayerY)
                            playerLevelReached = false;
                    }
                }
            }

            //After how long the DeathTimer is the zombie will stop moving.
            if (Time.time > Deathtimer)
            {
                Alive = false;
            }
        }

        if (HP <= 0)
            KillEntity();

        if (!Alive)
            Enemy.SetActive(false);
    }

    public void TakeDamage(int amount)
    {

        HP -= amount;
        Singleton_Sound.m_instance.PlayAudioClipOneShot("DamageInflictedSound", 0.2f);
    }

    public void KillEntity()
    {

        Alive = false;
        m_SpawnPickup.CreatePickup();
        Singleton_Game.m_instance.AddScore(100, EnemyPos);

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
        setTarget = true;
        playerLevelReached = false;
    }

    public void OnDeSpawn()
    {
    }
}

