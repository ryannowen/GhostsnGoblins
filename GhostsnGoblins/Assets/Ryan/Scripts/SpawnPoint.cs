using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] bool waitForSpawnedInactive = true;
    [SerializeField] bool checkPlayerDistance = false;
    [SerializeField] float spawnDelaySeconds = 1.0f;
    [SerializeField] float playerMaxDistance = 10;

    private GameObject spawnedObject = null;
    private WaitForSeconds spawnWait;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnWait = new WaitForSeconds(spawnDelaySeconds);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedObject == null)
            return;

        if (waitForSpawnedInactive)
        {
            if (!spawnedObject.activeSelf)
                spawnDelay();
        }
    }

    private IEnumerator spawnDelay()
    {
        yield return spawnWait;
        spawnedObject = null;
    }

    public bool GetCanSpawn()
    {
        if (checkPlayerDistance)
            if (null == player)
                Debug.LogError("Player is null, cannot check distance");
            else
                return (Vector2.Distance(player.transform.position, transform.position) <= playerMaxDistance) && (spawnedObject == null || !waitForSpawnedInactive);

        return (spawnedObject == null || !waitForSpawnedInactive);
    }

    public void SetSpawnedObject(GameObject argNewObject)
    {
        spawnedObject = argNewObject;
    }
}
