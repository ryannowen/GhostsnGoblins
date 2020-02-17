using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornAI : MonoBehaviour
{

    public GameObject Enemy;
    public GameObject Player;
    public bool alive = true;
    public bool Angered = false;

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
    private bool Jump;
    private bool Dash;
    private bool Shoot;

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
            if (PlayerX + 9 > EnemyX)
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

        if (!Jump && !Shoot && !Dash)
        {
            if (RNG <= 75 && RNG > 0)
            {
                Jump = true;
            }
            else if (RNG <= 95 && RNG > 75)
            {
                Shoot = true;
            }
            else if (RNG <= 100 && RNG > 95)
            {
                Dash = true;
            }
            RNG = 0;
        }


        if (Angered && alive)
        {
            if(Jump)
            {
                JumpTimer = Time.time + 1;
                if (PlayerX < EnemyX)
                {
                    EnemyY += speed;
                    EnemyX -= speed;

                    if (JumpTimer < Time.time && EnemyY > PlayerY)
                    {
                        EnemyY -= speed*2;
                    }
                }

                if (PlayerX > EnemyX)
                {
                    EnemyY += speed;
                    EnemyX += speed;

                    if (JumpTimer < Time.time && EnemyY > PlayerY)
                    {
                        EnemyY -= speed*2;
                    }
                }
                Jump = false;
            }

            if (Shoot)
            {

            }

            if (Dash)
            {
                DashTime = Time.time + 1;
                EnemyX -= speed * 2;
                if (DashTime < Time.time)
                {
                    Dash = false;
                }
            }
        }
        print(PlayerX);
        print(EnemyX);
    }
    
}
