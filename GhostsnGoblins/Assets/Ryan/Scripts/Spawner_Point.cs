using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Point : MonoBehaviour
{
    [SerializeField] private SpawnObject[] objects = null;
    [SerializeField] private List<SpawnPoint> spawnPoints = null;

    [SerializeField] private bool spawnOnLoad = true;
    [SerializeField] private float spawnDelaySeconds = 10.0f;


    private bool canSpawnObjects = false;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < objects.GetLength(0); i++)
            if (objects[i].createPool)
                System_Spawn.instance.CreatePool(objects[i].item, objects[i].amount, objects[i].spawnState);

        if (spawnOnLoad)
            SpawnObjects();
    }

    private void Update()
    {
        if (canSpawnObjects)
            SpawnObjects();
    }

    void SpawnObjects()
    {
        if (spawnPoints == null)
        {
            Debug.LogError("Cannot spawn object at point because there are no spawn points");
            return;
        }
        else if (spawnPoints.Count == 0)
        {
            Debug.LogError("Cannot spawn object at point because there are no spawn points");
            return;
        }

        for (int i = 0; i < objects.GetLength(0); i++)
        {
            for (int j = 0; j < objects[i].amount; j++)
            {
                if (Random.Range(1, 100) >= objects[i].spawnChance || objects[i].spawnChance == 100)
                {
                    List<SpawnPoint> availableSpawnPoints = spawnPoints;
                    SpawnPoint spawnPoint;

                    while (true)
                    {
                        int index = Random.Range(0, spawnPoints.Count);
                        spawnPoint = spawnPoints[index];

                        if (spawnPoint.GetCanSpawn())
                            break;
                        else
                            availableSpawnPoints.RemoveAt(index);

                        if (availableSpawnPoints.Count == 0)
                        {
                            Debug.LogWarning("cannot spawn object because all spawn points are in use");
                            return;
                        }
                    }

                    GameObject spawnedObject = System_Spawn.instance.GetObjectFromPool(objects[i].item, objects[i].ignoreAllActiveCheck);
                    if (gameObject == null)
                    {
                        Debug.LogError("Cannot spawn object, spawn system returned null");
                        return;
                    }

                    spawnPoint.SetSpawnedObject(gameObject);
                    spawnedObject.transform.position = spawnPoint.transform.position;
                }
            }
        }

        StartCoroutine(SpawnDelay());
    }

    IEnumerator SpawnDelay()
    {
        canSpawnObjects = false;
        yield return new WaitForSeconds(spawnDelaySeconds);
        canSpawnObjects = true;
    }
}
