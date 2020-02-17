using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMonsterAI : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Player;
    public bool alive = true;
    public bool angered = false;

    private float time = 0.5f;
    private float PlayerX;
    private float PlayerY;
    private float EnemyX;
    private bool FindPlayer;
    private bool Shoot;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (alive)
        {
            if (PlayerX + 10 > EnemyX)
            {
                angered = true;
            }
            if (angered)
            {
                if (Time.time > time)
                {
                    time += 0.5f;
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
            }

        }
    }
}
