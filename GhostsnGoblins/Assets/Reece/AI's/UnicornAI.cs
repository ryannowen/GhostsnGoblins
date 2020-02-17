using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornAI : MonoBehaviour
{

    public GameObject Enemy;
    public GameObject Player;
    public bool alive = true;


    private int RNG;
    private float speed = 0.1f;
    private float RNGtimer = 3;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private float JumpTimer;
    private float DashTime;
    private bool FindPlayer = true;

    [SerializeField] private bool Jump;
    [SerializeField] private bool Dash;
    [SerializeField] private bool Shoot;
    private bool Angered;


    // Start is called before the first frame update
    void Start()
    {
        
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
            FindPlayer = false;
        }

        if (!Angered)
        {
            if (PlayerX + 100 > EnemyX)
            {
                Angered = true;
            }
        }

        if (Time.time > RNGtimer)
        {
            RNG = Random.Range(2, 100);
            RNGtimer += 1.5f;
            FindPlayer = true;
        }

        if (!Jump && !Shoot && !Dash)
        {
            if (RNG <= 75 && RNG > 0)
            {
                Jump = true;
                JumpTimer = Time.time + 1f;
            }
            else if (RNG <= 95 && RNG > 75)
            {
                Shoot = true;
            }
            else if (RNG <= 100 && RNG > 95)
            {
                Dash = true;
                DashTime = Time.time + 1;
            }
            RNG = 0;
        }


        if (Angered && alive)
        {
            if(Jump)
            {
                if (PlayerX < EnemyX)
                {


                    if (Time.time > JumpTimer - 0.5f && EnemyY > PlayerY)
                    {
                        EnemyY -= speed * 2;
                    }
                    else if (Time.time < JumpTimer - 0.5f)
                    {
                        EnemyY += speed * 2;
                        EnemyX -= speed;
                    }
                }
                else if (PlayerX > EnemyX)
                {
               
                    if (Time.time > JumpTimer - 0.5f && EnemyY < PlayerY)
                    {
                        EnemyY -= speed * 2;
                    }
                    else if (Time.time < JumpTimer - 0.5f)
                    {
                        EnemyY += speed * 2;
                        EnemyX += speed;
                    }
                }
                if (Time.time > JumpTimer)
                {
                    Jump = false;
                }
            }

            if (Shoot)
            {
                Shoot = false;
            }

            if (Dash)
            {
                if (PlayerX < EnemyX)
                {
                    EnemyX -= speed * 2;
                }

                else if (PlayerX > EnemyX)
                {
                    EnemyX += speed * 2;
                }

                if (DashTime < Time.time)
                {
                    Dash = false;
                }
            }
        }
        print(JumpTimer);
        Enemy.gameObject.transform.position = new Vector3(EnemyX, EnemyY, Enemy.gameObject.transform.position.z);
    }
    
}
