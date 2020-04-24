using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : MonoBehaviour, IDamageable, ISpawn
{

    [SerializeField] private GameObject Bullet = null;

    SpawnPickup m_SpawnPickup = null;
    public bool Alive = true;
    private GameObject Enemy = null;
    private GameObject Player = null;
    private int HP = 1;
    private float timer = 0.5f;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private bool Angered = false;
    private bool FindPlayer;
    private bool Onetime = true;
    public bool Shoot;
    private float ShootHeight;

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

        m_SpawnPickup = this.gameObject.GetComponent<SpawnPickup>();

        Enemy = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Angered)
        {
            PlayerX = Player.gameObject.transform.position.x;
            PlayerY = Player.gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
        }

        if (HP <= 0)
            KillEntity();

        if (Alive)
        {
            Enemy.SetActive(true);
            if (PlayerX + 10 > EnemyX && PlayerX > EnemyX - 10 && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
            {
                Angered = true;
                
            }
            if (Angered)
            {
                if (Time.time > timer)
                {
                    timer += 2f;
                    FindPlayer = true;
                    Shoot = true;
                    ShootHeight = Random.Range(0, 2);
                }
                // FindPlayer = true;
                if (FindPlayer)
                {
                    PlayerX = Player.gameObject.transform.position.x;
                    EnemyX = Enemy.gameObject.transform.position.x;

                    FindPlayer = false;
                }
                if (Shoot)
                {
                    if (PlayerX < EnemyX)
                        fireProj.Fire(transform.position - new Vector3(0,ShootHeight - 0.5f ,0) , Vector3.left, transform.rotation);
                    else
                        fireProj.Fire(transform.position - new Vector3(0, ShootHeight - 0.5f , 0) , Vector3.right, transform.rotation);
                    Shoot = false;
                }
                if (Onetime)
                {
                    timer = Time.time;
                    Onetime = false;
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
