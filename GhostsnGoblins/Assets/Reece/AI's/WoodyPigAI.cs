using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodyPigAI : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Player;
    public bool alive = true;

    private float speed = 0.08f;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private float EnemyY;
    private float Deathtimer;
    private bool OneTime = true;
    private bool OneTime2 = true;
    private bool Angered = false;
    private bool MoveLeft;
    private bool MoveRight;
    private bool FindPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OneTime)
        {
            PlayerX = Player.gameObject.transform.position.x;
            PlayerY = Player.gameObject.transform.position.y;
            EnemyX = Enemy.gameObject.transform.position.x;
            EnemyY = Enemy.gameObject.transform.position.y;
            OneTime = false;
        }

        if (!Angered)
        {
            if (PlayerX + 5 > EnemyX)
            {
                Angered = true;
            }
        }  

        if (alive && Angered)
        {
            FindPlayer = true;
            if (FindPlayer && OneTime2)
            {
                PlayerX = Player.gameObject.transform.position.x;
                PlayerY = Player.gameObject.transform.position.y;
                EnemyX = Enemy.gameObject.transform.position.x;
                EnemyY = Enemy.gameObject.transform.position.y;
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
                OneTime2 = false;
            }

            //Will move the Zombie to the left if the player is on the left.
            if (MoveLeft)
            {
                if (EnemyX > PlayerX - 8)
                {
                    EnemyX -= speed;
                }
                else if (EnemyY > PlayerY)
                {
                    EnemyY -= 1;
                    MoveLeft = false;
                    MoveRight = true;
                }
                else
                {
                    EnemyY += 1;
                    MoveLeft = false;
                    MoveRight = true;
                }

            }
            //Will move the Zombie to the right if the player is on the right
            else if (MoveRight)
            {
                if (EnemyX < PlayerX + 8)
                {
                    EnemyX += speed;
                }
                else if (EnemyY > PlayerY)
                {
                    EnemyY -= 1;
                    MoveLeft = true;
                    MoveRight = false;
                }
                else
                {
                    EnemyY += 1;
                    MoveLeft = true;
                    MoveRight = false;
                }
            }
            //After how long the DeathTimer is the zombie will stop moving.
            if (Time.time > Deathtimer)
            {
                alive = false;
            }

            Enemy.gameObject.transform.position = new Vector3(EnemyX, EnemyY, Enemy.gameObject.transform.position.z);
        }
    }
}

