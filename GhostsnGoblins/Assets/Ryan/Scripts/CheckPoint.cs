using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private bool m_useCustomLocation = false;
    [SerializeField] private Vector2Int m_customLocation = new Vector2Int(0, 0);
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Singleton_Game.m_instance.SetCheckPoint(m_useCustomLocation ? m_customLocation : (Vector2)transform.position);

            if (1 == other.GetComponent<PlayerController>().GetID()) // is player 1
            {
                if (!Singleton_Game.m_instance.GetPlayer(1).activeSelf) // is player 2 not active
                {
                    Singleton_Game.m_instance.ReSpawnPlayerAtCheckpoint(1);
                }
            }
            else
            {
                if (!Singleton_Game.m_instance.GetPlayer(0).activeSelf) // is player 1 not active
                {
                    Singleton_Game.m_instance.ReSpawnPlayerAtCheckpoint(0);
                }
            }
        }
    }
}
