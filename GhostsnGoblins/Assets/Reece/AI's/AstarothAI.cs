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
    private int RNG;
    private int HP = 4;
    private float speed = 5f;
    private float DelayTimer;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private float DashTime;
    private bool FindPlayer = true;
    private bool Dash;
    private bool DashForward;
    private bool DashBackwards;
    private float Distance;
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
            PlayerX = Player.transform.position.x;
            PlayerY = Player.transform.position.y;
            EnemyX = Enemy.transform.position.x;
            EnemyY = Enemy.transform.position.y;
            Distance = PlayerX - EnemyX;
            if (Distance < 0)
                Distance = -Distance;
            FindPlayer = false;
        }

        if (!Angered)
        {
            if (PlayerX + 10 > EnemyX && PlayerX - 10 < EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
            {
                Angered = true;
            }
        }

        if (Angered)
        {
            if (PlayerX > EnemyX + 16 && PlayerX < EnemyX - 16 && PlayerY > EnemyY + 10 && PlayerY < EnemyY - 10)
            {
                Angered = false;
            }
        }

        if (Time.time > DelayTimer)
        {
            FindPlayer = true;
            
            if (!Shoot && !DashForward && !DashBackwards && Angered)
            {
                if (Distance > 10)
                {
                    Shoot = true;
                    DelayTimer += 1;
                }
                else if (Distance <= 10)
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
            else if (Distance > 5 && Distance <= 10)
            {
                DashBackwards = true;
                Dash = false;
            }
        }

        GetDesiredMove();

        if (Angered && Alive)
        {
            if (Shoot)
            {
                Vector3 directionToFire = Player.transform.position - transform.position;
                directionToFire.Normalize();
                fireProj.Fire(transform.position, directionToFire, transform.rotation);
                Shoot = false;
            }

            if (DashForward)
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

                if (DashTime < Time.time)
                {
                    DashForward = false;
                }
            }

            if (DashBackwards)
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
        }
    }

    public void KillEntity()
    {
        Alive = false;
        m_SpawnPickup.CreatePickup();
        Singleton_Game.m_instance.AddScore(2000, new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y));
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

