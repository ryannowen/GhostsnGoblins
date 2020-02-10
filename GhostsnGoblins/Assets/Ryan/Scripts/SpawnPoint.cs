using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] bool waitForSpawnedInactive = true;
    [SerializeField] float spawnDelaySeconds = 1.0f;
    private GameObject spawnedObject = null;
    private bool deSpawning = false;
    // Update is called once per frame
    void Update()
    {
        if (spawnedObject == null)
            return;

        if (waitForSpawnedInactive)
        {
            if (!spawnedObject.activeSelf && !deSpawning)
                spawnDelay();
        }
    }

    private IEnumerator spawnDelay()
    {
        deSpawning = true;
        yield return new WaitForSeconds(spawnDelaySeconds);
        spawnedObject = null;
        deSpawning = false;
    }

    public bool GetCanSpawn()
    {
        return (spawnedObject == null || !waitForSpawnedInactive ? true : false);
    }

    public void SetSpawnedObject(GameObject argNewObject)
    {
        spawnedObject = argNewObject;
    }
}
