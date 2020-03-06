using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private bool m_canSpawn = true;
    [SerializeField] private bool m_singleUse = true;
    [SerializeField] private float m_triggerDelaySeconds = 5.0f;
    [SerializeField] private float m_activeDelaySeconds = 0.0f;
    [Space]
    [SerializeField] private GameObject[] m_spawners = null;

    private WaitForSeconds m_triggerWait;
    private WaitForSeconds m_activateWait;

    private void Start()
    {
        m_triggerWait = new WaitForSeconds(m_triggerDelaySeconds);
        m_activateWait = new WaitForSeconds(m_activeDelaySeconds);

        if (!m_canSpawn)
           StartCoroutine(TriggerDelay(m_activateWait));
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
                StartCoroutine(TriggerDelay(m_triggerWait));

            spawnInterface.BeginSpawning();
        }
    }

    IEnumerator TriggerDelay(WaitForSeconds argDelay)
    {
        m_canSpawn = false;
        yield return argDelay;
        m_canSpawn = true;
    }

}
