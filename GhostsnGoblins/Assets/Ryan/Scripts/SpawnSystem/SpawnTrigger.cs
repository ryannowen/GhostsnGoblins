using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    enum ETriggerType
    {
        eActivateSpawner,
        eDeactivateSpawner,
        eToggleSpawner
    }

    [System.Serializable]
    class Spawner
    {
        public ETriggerType m_triggerType = ETriggerType.eActivateSpawner;
        public GameObject m_spawner = null;
    }

     
    [SerializeField] private bool m_canTrigger = true;
    [SerializeField] private bool m_singleUse = true;
    [SerializeField] private Vector2 m_triggerDelaySeconds = new Vector2(5.0f, 5.0f);
    [SerializeField] private Vector2 m_activeDelaySeconds = new Vector2(0.0f, 0.0f);
    [Space]
    [SerializeField] private Spawner[] m_spawners = null;

    private void Start()
    {
        if (!m_canTrigger)
           StartCoroutine(TriggerDelay(m_activeDelaySeconds));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !m_canTrigger)
            return;

        foreach (Spawner spawner in m_spawners)
        {
            if (null == spawner)
            {
                Debug.LogError("Spawner: " + spawner.m_spawner.name + " is null");
                return;
            }

            ISpawner spawnInterface = spawner.m_spawner.GetComponent<ISpawner>();

            if(null == spawnInterface)
            {
                Debug.LogWarning("Spawner: " + spawner.m_spawner.name + " has no spawn interface");
                return;
            }

            switch (spawner.m_triggerType)
            {
                case ETriggerType.eActivateSpawner:
                    TriggerSpawner(spawnInterface);
                    break;

                case ETriggerType.eDeactivateSpawner:
                    spawner.m_spawner.SetActive(false);
                    break;

                case ETriggerType.eToggleSpawner:
                    spawner.m_spawner.SetActive(!spawner.m_spawner.activeSelf);

                    if(spawner.m_spawner.activeSelf)
                        TriggerSpawner(spawnInterface);
                    break;

                default:
                    TriggerSpawner(spawnInterface);
                    Debug.LogWarning("TriggerType not specified, Defaulting to spawn, ");
                    break;
            }
        }
    }

    private void TriggerSpawner(ISpawner argSpawner)
    {
        if (!m_canTrigger)
            return;

        argSpawner.BeginSpawning();

        if (m_singleUse)
            m_canTrigger = false;
        else
            StartCoroutine(TriggerDelay(m_triggerDelaySeconds));
    }

    IEnumerator TriggerDelay(Vector2 argDelay)
    {
        WaitForSeconds m_wait = new WaitForSeconds(Random.Range(argDelay.x, argDelay.y));
        m_canTrigger = false;
        yield return argDelay;
        m_canTrigger = true;
    }

}
