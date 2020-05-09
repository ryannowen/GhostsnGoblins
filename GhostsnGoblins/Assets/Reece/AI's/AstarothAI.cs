using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarothAI : MonoBehaviour, IDamageable, ISpawn
{
    [SerializeField] private GameObject Bullet = null;
    public bool Alive = true;

    SpawnPickup m_SpawnPickup = null;
    private GameObject Enemy;
    private GameObject Player;
    private int HP = 5;
    private float speed = 5f;
    private float DelayTimer;
    private float PlayerX;
    private float PlayerY;
    private float PlayerX2;
    private float PlayerY2;
    private float Distance;
    private float Distance2;
    private float EnemyX;
    private float EnemyY;
    private float DashTime;
    private float ClosestPlayer = 0;
    private bool FindPlayer = true;
    private bool Dash;
    private bool DashForward;
    private bool DashBackwards;
    private bool Shoot;
    private bool Angered;
    private float InvicibleTimer;
    private bool OneTime = true;
    private float Origin;
    private float EnemyXOrigin;
    private float wait;
    private bool CheckIfStill;

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
            Distance = EnemyX - PlayerX;
            Distance2 = EnemyX - PlayerX2;

            if (Distance < 0)
                Distance = -Distance;
            if (Distance2 < 0)
                Distance2 = -Distance2;

            if (Distance < Distance2)
                ClosestPlayer = 1;
            else if (Distance2 < Distance)
                ClosestPlayer = 2;

            FindPlayer = false;
        }

        if (!Angered)
        {
            Shoot = false;
            if (PlayerX + 10 > EnemyX && PlayerX - 10 < EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
            {
                Angered = true;
            }
            if (PlayerX2 + 10 > EnemyX && PlayerX2 - 10 < EnemyX && PlayerY2 + 3 > EnemyY && PlayerY2 - 3 < EnemyY)
            {
                Angered = true;
            }
        }

        if (Angered)
        {
            if (PlayerX > EnemyX + 16 && PlayerX < EnemyX - 16 && PlayerY > EnemyY + 10 && PlayerY < EnemyY - 10 && PlayerX2 > EnemyX + 16 && PlayerX2 < EnemyX - 16 && PlayerY2 > EnemyY + 10 && PlayerY2 < EnemyY - 10)
            {
                Angered = false;
            }
        }

        

        if (Time.time > Origin)
        {
            EnemyXOrigin = EnemyX;
            Origin += 1f;
        }

        if (Time.time > wait)
        {
            wait += 1.5f;
            CheckIfStill = true;
        }

        GetDesiredMove();

        if (Angered && Alive)
        {

            if (OneTime)
            {
                Origin = Time.time;
                wait = Time.time + 0.25f;
                OneTime = false;
                DelayTimer = Time.time + 1f;
            }

            if (Time.time > DelayTimer)
            {
                FindPlayer = true;

                if (!Shoot && !DashForward && !DashBackwards && Angered)
                {
                    if (Distance > 8)
                    {
                        Shoot = true;
                        DelayTimer += 1.5f;
                    }
                    else if (Distance <= 8)
                    {
                        Dash = true;
                        DashTime = Time.time + 0.2f;
                        DelayTimer += 1;
                    }
                }
            }

            if (Dash == true)
            {
                if (Distance <= 5)
                {
                    DashForward = true;
                    Dash = false;
                }
                else if (Distance > 7 && Distance <= 8)
                {
                    DashBackwards = true;
                    Dash = false;
                }
            }



            if (EnemyX == EnemyXOrigin && CheckIfStill)
            {
                Dash = false;
                DashBackwards = false;
                DashForward = false;
                Shoot = true;
                CheckIfStill = false;
            }
            else
                CheckIfStill = false;

            if (Shoot)
            {
                if (ClosestPlayer == 1)
                {
                    Vector3 directionToFire = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position - transform.position;
                    directionToFire.Normalize();
                    fireProj.Fire(transform.position, directionToFire, transform.rotation);
                }
                if (ClosestPlayer == 2)
                {
                    Vector3 directionToFire = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position - transform.position;
                    directionToFire.Normalize();
                    fireProj.Fire(transform.position, directionToFire, transform.rotation);
                }
                Shoot = false;
            }

            if (DashForward)
            {
                if (ClosestPlayer == 1)
                {
                    if (PlayerX < EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                    }

                    else if (PlayerX > EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
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
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                    }

                    else if (PlayerX2 > EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, 1f);
                    }
                }

                if (DashTime < Time.time)
                {
                    DashForward = false;
                }
            }

            if (DashBackwards)
            {
                if (ClosestPlayer == 1)
                {
                    if (PlayerX < EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, -moveDirection, 1f);
                    }

                    else if (PlayerX > EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, -moveDirection, 1f);
                    }
                }

                if (ClosestPlayer == 2)
                {
                    if (PlayerX2 < EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, -moveDirection, 1f);
                    }

                    else if (PlayerX2 > EnemyX)
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        Vector3 moveDirection = GetDesiredMove();
                        moveDirection.Normalize();
                        moveDirection.y = rb.velocity.y;
                        moveDirection.x *= speed;

                        rb.velocity = Vector3.Lerp(rb.velocity, -moveDirection, 1f);
                    }
                }

                if (DashTime < Time.time)
                {
                    DashBackwards = false;
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
            DashForward = true;
        }
    }

    public void KillEntity()
    {
        Alive = false;
        m_SpawnPickup.CreatePickup();
        Singleton_Game.m_instance.AddScore(10000, new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y));
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
        HP = 5;
        Alive = true;
        OneTime = true;
        Angered = false;
        FindPlayer = true;
    }

    public void OnDeSpawn()
    {
    }
}

