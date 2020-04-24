using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatanAI : MonoBehaviour, IDamageable, ISpawn
{
    public bool Alive = true;

    [SerializeField] private GameObject Bullet = null;

    SpawnPickup m_SpawnPickup = null;
    private GameObject Enemy;
    private GameObject Player;
    private int RNG;
    private int HP = 10;
    private float speed = 0.1f;
    private float RNGtimer = 3;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private float DistanceX;
    private float DistanceY;
    private bool FindPlayer = true;
    private bool Swoop;
    private bool Shoot;
    private bool Angered;
    private bool AntiSwoopLeft;
    private bool AntiSwoopRight;
    private float InvicibleTimer;

    private FireProjectile fireProj;
    //private float Deathtimer = 100f;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        if (Bullet == null)
            Bullet = (GameObject)Resources.Load("Prefabs/Bullet") as GameObject;

        fireProj = this.gameObject.GetComponent<FireProjectile>();
        fireProj.SetProjectile(Bullet);

        m_SpawnPickup = this.gameObject.GetComponent<SpawnPickup>();

        Enemy = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindPlayer)
        {
            PlayerX = Player.gameObject.transform.position.x;
            PlayerY = Player.gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
            DistanceY = EnemyY - PlayerY;
            DistanceX = EnemyX - PlayerX;
            FindPlayer = false;
        }

        if (!Angered)
        {
            FindPlayer = true;
            if (PlayerX + 5 > EnemyX && PlayerX - 5 < EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
            {
                Angered = true;
            }
        }

        if (Time.time > RNGtimer)
        {
            RNG = Random.Range(2, 100);
            RNGtimer += 3;
            FindPlayer = true;
        }

        if (!Swoop && !AntiSwoopLeft && !AntiSwoopRight && !Shoot)
        {
            if (RNG > 30)
            {
                Swoop = true;
            }
            if (RNG > 0 && RNG <= 30)
            {
                Shoot = true;
            }

            RNG = 0;
            
        }

        if (Angered && Alive)
        {

            if (Swoop)
            {
                if (PlayerX < EnemyX && PlayerY < EnemyY)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    EnemyX -= DistanceX / 40;
                    EnemyY -= DistanceY / 40;
                    if (PlayerX + 0.3f > EnemyX && PlayerX - 0.3f < EnemyX)
                    {
                        AntiSwoopLeft = true;
                        Swoop = false;
                    }
                }
                if (PlayerX > EnemyX && PlayerY < EnemyY)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    EnemyX -= DistanceX / 40;
                    EnemyY -= DistanceY / 40;
                    if (PlayerX + 0.3f > EnemyX && PlayerX - 0.3f < EnemyX)
                    {
                        AntiSwoopRight = true;
                        Swoop = false;
                    }
                }
            }


            if (AntiSwoopLeft)
            {
                if (PlayerX - 8 < EnemyX)
                {
                    EnemyX -= speed * 4;
                }
                else
                {
                    AntiSwoopLeft = false;
                }
                if (PlayerY + 4 > EnemyY)
                {
                    EnemyY += speed * 2;
                }
                else
                {
                    AntiSwoopLeft = false;
                }
            }


            if (AntiSwoopRight)
            {
                if (PlayerX + 8 > EnemyX)
                {
                    EnemyX += speed * 4;
                }
                else
                {
                    AntiSwoopRight = false;
                }
                if (PlayerY + 4 > EnemyY)
                {
                    EnemyY += speed * 2;
                }
                else
                {
                    AntiSwoopRight = false;
                }
            }

            /* if (EnemyX > PlayerX + 8.1f)
             {
                 EnemyX -= speed * 5;
                 // Enemy.gameObject.transform.position = new Vector3(EnemyX, Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z)
             }
             else if (EnemyX < PlayerX - 8.1f)
             {
                 EnemyX += speed * 5;
                 // Enemy.gameObject.transform.position = new Vector3(EnemyX, Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z)
             }*/
            Enemy.gameObject.transform.position = new Vector3(EnemyX, EnemyY, Enemy.gameObject.transform.position.z);
        }

        if (Shoot)
        {
            Vector3 directionToFire = Player.transform.position - transform.position;
            directionToFire.Normalize();
            fireProj.Fire(transform.position, directionToFire, transform.rotation);
            Shoot = false;
        }

        if (HP <= 0)
            KillEntity();

        if (!Alive)
            Enemy.SetActive(false);
    }


    public void TakeDamage(int amount)
    {
        if (InvicibleTimer < Time.time)
        {
            InvicibleTimer = Time.time + 0.5f;
            Angered = true;
            HP -= amount;
            Singleton_Sound.m_instance.PlayAudioClipOneShot("DamageInflictedSound", 0.2f);
        }
    }

    public void KillEntity()
    {
        Alive = false;
        m_SpawnPickup.CreatePickup();
        Singleton_Game.m_instance.AddScore(3000, new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y));
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
        HP = 10;
        Alive = true;
        Angered = false;
        FindPlayer = true;
    }

    public void OnDeSpawn()
    {
    }
}
