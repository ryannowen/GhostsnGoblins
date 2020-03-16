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
            if (m_useCustomLocation) // custom location
                Singleton_Game.m_instance.SetCheckPoint(m_customLocation);
            else // checkpoint position
                Singleton_Game.m_instance.SetCheckPoint(transform.position);
        }
    }
}
