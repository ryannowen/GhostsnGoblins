using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private bool m_waitForSpawnedInactive = true;
    [SerializeField] private float m_spawnDelaySeconds = 1.0f;
    [Space]
    [SerializeField] private bool m_checkPlayerDistance = false;
    [SerializeField] private float m_playerMaxDistance = 10;
    [Space]
    [SerializeField] private bool m_canSpawnOnScreen = true;
    private bool m_isOnScreen = false;

    private GameObject m_spawnedObject = null;
    private WaitForSeconds m_spawnWait;

    private GameObject m_player1;
    private GameObject m_player2;


    private void Start()
    {
        m_player1 = Singleton_Game.m_instance.GetPlayer(0);
        m_player2 = Singleton_Game.m_instance.GetPlayer(1);

        m_spawnWait = new WaitForSeconds(m_spawnDelaySeconds);
    }

    // Update is called once per frame
    void Update()
    {
        if (null == m_spawnedObject)
            return;

        if (m_waitForSpawnedInactive)
        {
            if (!m_spawnedObject.activeSelf)
                StartCoroutine(spawnDelay());
        }
    }

    public void ResetSpawnPoint()
    {
        m_spawnedObject = null;
    }

    private IEnumerator spawnDelay()
    {
        yield return m_spawnWait;
        m_spawnedObject = null;
    }

    public bool GetCanSpawn()
    {
        bool inPlayerRange = true;

        if (m_checkPlayerDistance)
        {
            if (null == m_player1)
                Debug.LogError("Player is null, cannot check distance");
            else
            {
                float distance = Vector2.Distance(m_player1.transform.position, transform.position);

                float player2Distance = 0;
                if (null != m_player2)
                    player2Distance = Vector2.Distance(m_player2.transform.position, transform.position);

                distance = (distance >= player2Distance) ? player2Distance : distance;

                inPlayerRange = distance <= m_playerMaxDistance;
            }
        }

        return (m_canSpawnOnScreen ? true : !m_isOnScreen) && (null == m_spawnedObject || !m_waitForSpawnedInactive) && inPlayerRange;
    }

    public void SetSpawnedObject(GameObject argNewObject)
    {
        m_spawnedObject = argNewObject;
    }

    private void OnBecameVisible()
    {
        m_isOnScreen = true;
    }

    private void OnBecameInvisible()
    {
        m_isOnScreen = false;
    }
}
