using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMonsterAI : MonoBehaviour, IDamageable, ISpawn
{

    [SerializeField] private GameObject Bullet = null;

    public bool Alive = true;

    SpawnPickup m_SpawnPickup = null;
    private GameObject Enemy = null;
    private GameObject Player = null;
    private int HP = 1;
    private float timer;
    private float PlayerX;
    private float PlayerY;
    private float PlayerX2;
    private float PlayerY2;
    private float Distance;
    private float Distance2;
    private float EnemyX;
    private float EnemyY;
    private float ClosestPlayer = 0;
    private bool Angered = false;
    private bool FindPlayer;
    public bool Shoot;
    private bool Onetime = true;

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

        Bullet.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        m_SpawnPickup = this.gameObject.GetComponent<SpawnPickup>();

        Enemy = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Angered)
        {
            Shoot = false;
            PlayerX = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.x;
            PlayerY = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.y;
            PlayerX2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.x;
            PlayerY2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
        }

        if (HP <= 0)
            KillEntity();

        if (PlayerX + 15 > EnemyX && PlayerX - 15 < EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
        {
            Angered = true;
            if (Onetime)
            {
                timer = Time.time;
                Onetime = false;
            }
        }

        if (PlayerX2 + 15 > EnemyX && PlayerX2 > EnemyX - 15 && PlayerY2 + 3 > EnemyY && PlayerY2 - 3 < EnemyY)
        {
            Angered = true;
            if (Onetime)
            {
                timer = Time.time;
                Onetime = false;
            }
        }

        if (Alive)
        {
            Enemy.SetActive(true);
           

            if (Angered)
            {
                if (Time.time > timer)
                {
                    FindPlayer = true;
                    Shoot = true;
                    timer += 2f;
                }
                // FindPlayer = true;
                if (FindPlayer)
                {
                    PlayerX = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.x;
                    PlayerY = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position.y;
                    PlayerX2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.x;
                    PlayerY2 = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position.y;
                    Distance = EnemyX - PlayerX;
                    Distance2 = EnemyX - PlayerX2;
                    EnemyX = Enemy.gameObject.transform.position.x;

                    if (Distance < 0)
                        Distance = -Distance;
                    if (Distance2 < 0)
                        Distance2 = -Distance2;

                    if (Distance < Distance2)
                        ClosestPlayer = 1;
                    if (Distance2 < Distance)
                        ClosestPlayer = 2;

                    FindPlayer = false;
                }
                if (Shoot)
                {
                    if (ClosestPlayer == 1)
                    {
                        Vector3 directionToFire = Singleton_Game.m_instance.GetPlayer(0).gameObject.transform.position - transform.position;
                        directionToFire.Normalize();
                        fireProj.Fire(transform.position, directionToFire, transform.rotation);
                        Shoot = false;
                    }
                    if (ClosestPlayer == 2)
                    {
                        Vector3 directionToFire = Singleton_Game.m_instance.GetPlayer(1).gameObject.transform.position - transform.position;
                        directionToFire.Normalize();
                        fireProj.Fire(transform.position, directionToFire, transform.rotation);
                        Shoot = false;
                    }
                }
            }

        }
        else
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
        Singleton_Game.m_instance.AddScore(100, new Vector2(Enemy.gameObject.transform.position.x, Enemy.gameObject.transform.position.y));
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
        HP = 1;
        Alive = true;
        Angered = false;
    }

    public void OnDeSpawn()
    {
    }
}
