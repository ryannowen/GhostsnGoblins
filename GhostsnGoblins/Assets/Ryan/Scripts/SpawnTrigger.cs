using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private float triggerDelaySeconds = 5.0f;
    [SerializeField] private float activeDelaySeconds = 0.0f;
    [SerializeField] private bool canSpawn = true;

    private WaitForSeconds triggerWait;
    private WaitForSeconds activateWait;

    private void Start()
    {
        triggerWait = new WaitForSeconds(triggerDelaySeconds);
        activateWait = new WaitForSeconds(activeDelaySeconds);

        if (!canSpawn)
           StartCoroutine(TriggerDelay(activateWait));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !canSpawn)
            return;

        foreach (GameObject spawner in spawners)
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

            StartCoroutine(TriggerDelay(triggerWait));

            spawnInterface.BeginSpawning();
        }
    }

    IEnumerator TriggerDelay(WaitForSeconds argDelay)
    {
        canSpawn = false;
        yield return argDelay;
        canSpawn = true;
    }

}
