using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Point : MonoBehaviour, ISpawner
{
    [SerializeField] private bool m_spawnOnLoad = true;
    [SerializeField] private bool m_showIfObjectCannotSpawn = false;
    [SerializeField] private bool m_timedSpawner = false;
    [SerializeField] private Vector2 m_spawnDelaySeconds = new Vector2(10.0f, 10.0f);
    [Space]
    [SerializeField] private SpawnObject[] m_objects = null;
    [SerializeField] private List<SpawnPoint> m_spawnPoints = null;

    private bool m_canSpawnObjects = false;


    // Start is called before the first frame update
    void Start()
    {
        if (m_spawnOnLoad)
            BeginSpawning();
        else if(m_timedSpawner)
            StartCoroutine(SpawnDelay());
    }

    private void Update()
    {
        if (m_canSpawnObjects)
            BeginSpawning();
    }

    IEnumerator SpawnDelay()
    {
        m_canSpawnObjects = false;
        yield return new WaitForSeconds(Random.Range(m_spawnDelaySeconds.x, m_spawnDelaySeconds.y));
        m_canSpawnObjects = true;
    }

    public void BeginSpawning()
    {
        if (null == m_spawnPoints)
        {
            Debug.LogError("Cannot spawn object at point because there are no spawn points");
            return;
        }
        else if (m_spawnPoints.Count == 0)
        {
            Debug.LogError("Cannot spawn object at point because there are no spawn points");
            return;
        }

        bool failedToSpawn = false;

        foreach (SpawnObject spawnObject in m_objects)
        {
            for (int i = 0; i < spawnObject.spawnAmount; i++)
            {
                int spawnChance = Random.Range(1, 100);

                if (spawnChance >= spawnObject.spawnChance || spawnObject.spawnChance == 100)
                {
                    List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>(m_spawnPoints);
                    SpawnPoint spawnPoint;

                    while (true)
                    {
                        int index = Random.Range(0, availableSpawnPoints.Count);
                        spawnPoint = availableSpawnPoints[index];

                        if (spawnPoint.GetCanSpawn())
                            break;
                        else
                        {
                            if (m_showIfObjectCannotSpawn)
                                Debug.LogWarning("Cannot spawn object, chance=" + spawnChance + "/" + spawnObject.spawnChance);

                            availableSpawnPoints.RemoveAt(index);
                        }

                        if (availableSpawnPoints.Count == 0)
                        {
                            Debug.LogWarning("cannot spawn object because all spawn points are in use");
                            failedToSpawn = true;
                            break;
                        }
                    }

                    if (failedToSpawn)
                        break;

                    GameObject spawnedObject = System_Spawn.instance.GetObjectFromPool(spawnObject.item, spawnObject.ignoreAllActiveCheck);
                    if (null == gameObject)
                    {
                        Debug.LogError("Cannot spawn object, spawn system returned null");
                        failedToSpawn = true;
                        break;
                    }

                    spawnPoint.SetSpawnedObject(gameObject);
                    spawnedObject.transform.position = spawnPoint.transform.position;
                }
            }

            if (failedToSpawn)
                break;
        }

        if (m_timedSpawner)
            StartCoroutine(SpawnDelay());
    }
}
