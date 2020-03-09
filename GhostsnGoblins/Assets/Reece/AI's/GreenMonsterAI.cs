using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMonsterAI : MonoBehaviour
{

    [SerializeField] private GameObject Bullet = null;

    public bool Alive = true; 

    private GameObject Enemy = null;
    private GameObject Player = null;
    private int HP = 1;
    private float time = 0.5f;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private bool angered = false;
    private bool FindPlayer;
    public bool Shoot;

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



        Enemy = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!angered)
        {
            PlayerX = Player.gameObject.transform.position.x;
            PlayerY = Player.gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
        }

        if (HP <= 0)
            Alive = false;

        if (Alive)
        {
            Enemy.SetActive(true);
            if (PlayerX + 10 > EnemyX && PlayerX + 10 > EnemyX && PlayerY + 3 > EnemyY && PlayerY - 3 < EnemyY)
            {
                angered = true;
            }
            if (angered)
            {
                if (Time.time > time)
                {
                    time += 1.5f;
                    FindPlayer = true;
                    Shoot = true;
                }
                // FindPlayer = true;
                if (FindPlayer)
                {
                    PlayerX = Player.gameObject.transform.position.x;
                    PlayerY = Player.gameObject.transform.position.y;
                    EnemyX = Enemy.gameObject.transform.position.x;

                    FindPlayer = false;
                }
                if (Shoot)
                {
                    Vector3 directionToFire = Player.transform.position - transform.position;
                    directionToFire.Normalize();
                    fireProj.Fire(transform.position, directionToFire, transform.rotation);
                    Shoot = false;
                }
            }
           
        }
        else
            Enemy.SetActive(false);
    }
}
