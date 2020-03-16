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
    [SerializeField] private GameObject m_playerPrefab = null;
    [Space]
    [SerializeField] private bool m_canSpawnOnScreen = true;
    private bool m_isOnScreen = false;

    private GameObject m_spawnedObject = null;
    private WaitForSeconds m_spawnWait;

    private GameObject m_player;

    private void Start()
    {
        m_player = System_Spawn.instance.GetObjectFromPool(m_playerPrefab, true);
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
                spawnDelay();
        }
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
            if (null == m_player)
                Debug.LogError("Player is null, cannot check distance");
            else
                inPlayerRange = Vector2.Distance(m_player.transform.position, transform.position) <= m_playerMaxDistance;
        }

        return (m_canSpawnOnScreen ? true : m_isOnScreen) && (null == m_spawnedObject || !m_waitForSpawnedInactive) && inPlayerRange;
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
