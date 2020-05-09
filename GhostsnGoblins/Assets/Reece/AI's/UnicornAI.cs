using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornAI : MonoBehaviour, IDamageable, ISpawn
{
    [SerializeField] private GameObject Bullet = null;
    public bool Alive = true;

    SpawnPickup m_SpawnPickup = null;
    private GameObject Enemy;
    private GameObject Player;
    private int RNG;
    private int HP = 4;
    private float speed = 5f;
    private float RNGtimer;
    private float PlayerX;
    private float PlayerY;
    private float PlayerX2;
    private float PlayerY2;
    private float Distance;
    private float Distance2;
    private float EnemyX;
    private float EnemyY;
    private float JumpTimer;
    private float DashTime;
    private float ClosestPlayer = 0;
    private bool FindPlayer = true;
    private bool Jump;
    private bool Dash;
    private bool Shoot;
    private bool Angered;
    private float InvicibleTimer;

    private FireProjectile fireProj;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        if (Bullet == null)
            Bullet = (GameObject)Resources.Load("Prefabs/Bullet") as GameObject;

        fireProj = this.gameObject.GetComponent<FireProjectile>();
        fireProj.SetProjectile(Bullet);

        m_SpawnPickup = this.gameObject.GetComponent<SpawnPickup>();

        Enemy = this.gameObject;
    }

    Vector3 GetDesiredMove()
    {
        Vector3 v = Vector3.zero;
        if (PlayerX < EnemyX)
            v += Vector3.left;
        else
            v += Vector3.right;
        v.Normalize();
        return v;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindPlayer)
        {
            PlayerX = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.x;
            PlayerY = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.y;
            PlayerX2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.x;
            PlayerY2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.y;
            EnemyX = Enemy.transform.position.x;
            EnemyY = Enemy.transform.position.y;
            FindPlayer = false;

            Distance = EnemyX - PlayerX;
            Distance2 = EnemyX - PlayerX2;

            if (Distance < 0)
                Distance = -Distance;
            if (Distance2 < 0)
                Distance2 = -Distance2;

            if (Distance < Distance2)
                ClosestPlayer = 1;
            if (Distance2 < Distance)
                ClosestPlayer = 2;
        }

        if (!Angered)
        {
            if (ClosestPlayer == 1)
            {
                if (PlayerX + 10 > EnemyX && PlayerX - 10 < EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
                {
                    Angered = true;
                }
            }

            if (ClosestPlayer == 2)
            {
                if (PlayerX2 + 10 > EnemyX && PlayerX2 - 10 < EnemyX && PlayerY2 + 3 > EnemyY && PlayerY2 - 3 < EnemyY)
                {
                    Angered = true;
                }
            }
        }

        if (Time.time > RNGtimer)
        {
            FindPlayer = true;
            RNG = Random.Range(2, 100);
            RNGtimer += 1;
        }



        if (!Jump && !Shoot && !Dash)
        {
            if (RNG <= 65 && RNG > 0)
            {
                Jump = true;
                JumpTimer = Time.time + 1;
            }
            else if (RNG <= 66 && RNG > 65)
            {
                Shoot = true;
            }
            else if (RNG <= 100 && RNG > 66)
            {
                Dash = true;
                DashTime = Time.time + 0.2f;
            }
            RNG = 0;
        }

        GetDesiredMove();

        if (Angered && Alive)
        {
            if (Jump)
            {
                if (ClosestPlayer == 1)
                {
                    if (PlayerX < EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        if (Time.time < JumpTimer - 0.8f)
                        {
                            //EnemyY += speed * 2;
                            //EnemyX -= speed;
                            Vector3 moveDirection = GetDesiredMove();
                            moveDirection.Normalize();
                            moveDirection.y = rb.velocity.y;
                            moveDirection.x *= speed;

                            rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(rb.velocity.x, 14), 1f);
                        }
                    }
                    else if (PlayerX > EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        if (Time.time < JumpTimer - 0.8f)
                        {
                            //EnemyY += speed * 2;
                            //EnemyX += speed;
                            Vector3 moveDirection = GetDesiredMove();
                            moveDirection.Normalize();
                            moveDirection.y = rb.velocity.y;
                            moveDirection.x *= speed;

                            rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(rb.velocity.x, 14), 1f);
                        }
                    }
                }

                if (ClosestPlayer == 2)
                {
                    if (PlayerX2 < EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        if (Time.time < JumpTimer - 0.8f)
                        {
                            //EnemyY += speed * 2;
                            //EnemyX -= speed;
                            Vector3 moveDirection = GetDesiredMove();
                            moveDirection.Normalize();
                            moveDirection.y = rb.velocity.y;
                            moveDirection.x *= speed;

                            rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(rb.velocity.x, 14), 1f);
                        }
                    }
                    else if (PlayerX2 > EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        if (Time.time < JumpTimer - 0.8f)
                        {
                            //EnemyY += speed * 2;
                            //EnemyX += speed;
                            Vector3 moveDirection = GetDesiredMove();
                            moveDirection.Normalize();
                            moveDirection.y = rb.velocity.y;
                            moveDirection.x *= speed;

                            rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(rb.velocity.x, 14), 1f);
                        }
                    }
                }

                if (Time.time > JumpTimer)
                {
                    Jump = false;
                }
            }

            if (Shoot)
            {
                if (ClosestPlayer == 1)
                {
                    if (PlayerX < EnemyX)
                        fireProj.Fire(transform.position, Vector3.left, transform.rotation);
                    else
                        fireProj.Fire(transform.position, Vector3.right, transform.rotation);
                }

                if (ClosestPlayer == 2)
                {
                    if (PlayerX2 < EnemyX)
                        fireProj.Fire(transform.position, Vector3.left, transform.rotation);
                    else
                        fireProj.Fire(transform.position, Vector3.right, transform.rotation);
                }

                Shoot = false;
            }

            if (Dash)
            {
                if (ClosestPlayer == 1)
                {
                    if (PlayerX < EnemyX)
                    {
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                    }

                    else if (PlayerX > EnemyX)
                    {
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                    }
                }

                if (ClosestPlayer == 2)
                {
                    if (PlayerX2 < EnemyX)
                    {
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                    }

                    else if (PlayerX2 > EnemyX)
                    {
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                    }
                }

                if (DashTime < Time.time)
                {
                    Dash = false;
                }
            }
        }

        if (HP <= 0)
            KillEntity();

        if (!Alive)
            Enemy.SetActive(false);

        //Enemy.gameObject.transform.position = new Vector3(EnemyX, EnemyY, Enemy.gameObject.transform.position.z);
    }

    public void TakeDamage(int amount)
    {
        if (InvicibleTimer < Time.time)
        {
            InvicibleTimer = Time.time + 0.5f;
            Angered = true;
            HP -= amount;
            Singleton_Sound.m_instance.PlayAudioClip("DamageInflictedSound", 0.4f);
        }
    }

    public void KillEntity()
    {
        Alive = false;
        m_SpawnPickup.CreatePickup();
        Singleton_Game.m_instance.AddScore(2000, new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y));
        Singleton_Game.m_instance.AddGameStat(Singleton_Game.EGameStat.EKills, 1);
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
        HP = 4;
        Alive = true;
        Angered = false;
        FindPlayer = true;
    }

    public void OnDeSpawn()
    {
    }
}
