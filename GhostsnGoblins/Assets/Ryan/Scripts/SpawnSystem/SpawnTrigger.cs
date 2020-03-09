using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private bool m_canSpawn = true;
    [SerializeField] private bool m_singleUse = true;
    [SerializeField] private Vector2 m_triggerDelaySeconds = new Vector2(5.0f, 5.0f);
    [SerializeField] private Vector2 m_activeDelaySeconds = new Vector2(0.0f, 0.0f);
    [Space]
    [SerializeField] private GameObject[] m_spawners = null;

    private void Start()
    {
        if (!m_canSpawn)
           StartCoroutine(TriggerDelay(m_activeDelaySeconds));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !m_canSpawn)
            return;

        foreach (GameObject spawner in m_spawners)
        {
            if (null == spawner)
            {
                Debug.LogError("Spawner: " + spawner.name + " has is null");
                return;
            }

            ISpawner spawnInterface = spawner.GetComponent<ISpawner>();

            if(null == spawnInterface)
            {
                Debug.LogWarning("Spawner: " + spawner.name + " has no spawn interface");
                return;
            }

            if(m_singleUse)
                m_canSpawn = false;
            else
                StartCoroutine(TriggerDelay(m_triggerDelaySeconds));

            spawnInterface.BeginSpawning();
        }
    }

    IEnumerator TriggerDelay(Vector2 argDelay)
    {
        WaitForSeconds m_wait = new WaitForSeconds(Random.Range(argDelay.x, argDelay.y));
        m_canSpawn = false;
        yield return argDelay;
        m_canSpawn = true;
    }

}
