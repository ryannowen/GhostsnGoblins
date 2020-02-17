using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Player;
    public bool alive = true;

    private float speed = 0.04f;
    private float PlayerX;
    private float EnemyX;
    private float Deathtimer;
    private bool OneTime = true;
    public bool Angered = false;
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
            if (FindPlayer && OneTime)
            {
                PlayerX = Player.gameObject.transform.position.x;
                EnemyX = Enemy.gameObject.transform.position.x;
                Deathtimer = Time.time + 5;

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

                if (Random.Range(2,101) > 70)
                {
                    speed = speed*1.5f;
                }
                OneTime = false;
            }

            //Will move the Zombie to the left if the player is on the left.
            if (MoveLeft)
            {
                EnemyX -= speed;              
            }
            //Will move the Zombie to the right if the player is on the right
            else if (MoveRight)
            {
                EnemyX += speed;
            }
            //After how long the DeathTimer is the zombie will stop moving.
            if (Time.time > Deathtimer)
            {
                alive = false;
            }
            Enemy.gameObject.transform.position = new Vector3(EnemyX, Enemy.gameObject.transform.position.y, Enemy.gameObject.transform.position.z);
        }
    }

}
