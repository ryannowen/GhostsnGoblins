using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Point : MonoBehaviour, ISpawner
{
    [SerializeField] private bool m_spawnOnLoad = true;
    [SerializeField] private bool m_disableAllSpawnPointsInUseLog = false;
    [SerializeField] private bool m_showIfObjectCannotSpawn = false;
    [SerializeField] private bool m_timedSpawner = false;
    [SerializeField] private bool m_reuseActiveObjects = true;
    [SerializeField] private bool m_registerSelf = true;

    [SerializeField] private Vector2 m_spawnDelaySeconds = new Vector2(10.0f, 10.0f);
    [Space]
    [SerializeField] private SpawnObject[] m_objects = null;
    [SerializeField] private List<SpawnPoint> m_spawnPoints = null;

    private bool m_canSpawnObjects = false;


    // Start is called before the first frame update
    void Start()
    {
        if(m_registerSelf)
            System_Spawn.instance.RegisterSpawner(gameObject);

        ActivateSpawner();
    }

    public void ActivateSpawner()
    {
        if (m_spawnOnLoad)
            BeginSpawning();
        else if (m_timedSpawner)
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
                ActivateSpawnReactors(ESpawnReactorType.eOnBeginSpawning, spawnObject);

                int spawnChance = Random.Range(1, 100);

                if (spawnChance >= spawnObject.spawnChance || spawnObject.spawnChance == 100)
                {
                    List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>(m_spawnPoints);
                    SpawnPoint spawnPoint;

                    while (true)
                    {
                        int index = Random.Range(0, availableSpawnPoints.Count);
                        spawnPoint = availableSpawnPoints[index];

                        if(null == spawnPoint)
                        {
                            Debug.Log("Cannot use spawn point because none are specified at index=" + index);
                            availableSpawnPoints.RemoveAt(index);
                            continue;
                        }

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
                            if(!m_disableAllSpawnPointsInUseLog)
                                Debug.LogWarning("cannot spawn object because all spawn points are in use / cannot be used");

                            failedToSpawn = true;
                            break;
                        }
                    }

                    if (failedToSpawn)
                        break;

                    GameObject spawnedObject = System_Spawn.instance.GetObjectFromPool(spawnObject.item, spawnObject.ignoreAllActiveCheck, false, spawnObject.shouldPeek);

                    if (null == spawnedObject)
                    {
                        Debug.LogError("Cannot spawn object, spawn system returned null");
                        failedToSpawn = true;
                        break;
                    }

                    ActivateSpawnReactors(ESpawnReactorType.eOnEndSpawning, spawnObject, spawnedObject);
                    if (m_reuseActiveObjects ? true : !spawnedObject.activeSelf)
                    {
                        spawnedObject.SetActive(true);

                        spawnPoint.SetSpawnedObject(spawnedObject);
                        spawnedObject.transform.position = spawnPoint.transform.position;
                    }

                }

                ActivateSpawnReactors(ESpawnReactorType.eOnEndSpawning, spawnObject);
            }

            if (failedToSpawn)
                break;
        }

        if (m_timedSpawner)
            StartCoroutine(SpawnDelay());        
    }

    private void ActivateSpawnReactors(ESpawnReactorType argReactorType, SpawnObject argSpawnObjectData, GameObject argSpawnedObject = null)
    {
        switch (argReactorType)
        {
            case ESpawnReactorType.eOnBeginSpawning:
                foreach (GameObject reactorObject in argSpawnObjectData.spawnReactors)
                {
                    ISpawnReactor reactor = reactorObject.GetComponent<ISpawnReactor>();

                    if (null != reactor)
                    {
                        reactor.ReactorOnBeginSpawning();
                    }
                }

                break;
            case ESpawnReactorType.eOnSpawn:
                foreach (GameObject reactorObject in argSpawnObjectData.spawnReactors)
                {
                    ISpawnReactor reactor = reactorObject.GetComponent<ISpawnReactor>();

                    if (null != reactor && null != argSpawnedObject)
                    {
                        reactor.ReactorOnSpawn(argSpawnedObject);
                    }
                }
                break;
            case ESpawnReactorType.eOnEndSpawning:
                foreach (GameObject reactorObject in argSpawnObjectData.spawnReactors)
                {
                    ISpawnReactor reactor = reactorObject.GetComponent<ISpawnReactor>();

                    if (null != reactor)
                    {
                        reactor.ReactorOnEndSpawning();
                    }
                }
                break;
            default:
                Debug.LogError("Cannot activate spawner area reactor because case does not exist, case=" + argReactorType);
                break;
        }
    }
}
