using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner_Area : MonoBehaviour, ISpawner
{
    [SerializeField] private bool m_spawnOnLoad = true;
    [SerializeField] private bool m_timedSpawner = false;
    [SerializeField] private Vector2 m_spawnDelaySeconds = new Vector2(10.0f, 10.0f);
    [Space]
    [SerializeField] private SpawnObject[] m_objects = null;

    private bool m_canSpawnObjects = false;
    private BoxCollider2D m_boxCollider;

    // Start is called before the firsst frame update
    void Start()
    {
        m_boxCollider = GetComponent<BoxCollider2D>();

        if (m_spawnOnLoad)
            BeginSpawning();
        else if(m_timedSpawner)
            StartCoroutine(SpawnDelay());
    }

    private void Update()
    {
        if(m_canSpawnObjects)
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
        foreach (SpawnObject spawnObject in m_objects)
        {
            ActivateSpawnReactors(ESpawnReactorType.eOnBeginSpawning, spawnObject);

            for (int i = 0; i < spawnObject.spawnAmount; i++)
            {
                if (Random.Range(1, 100) >= spawnObject.spawnChance || spawnObject.spawnChance == 100)
                {
                    GameObject spawnedObject = System_Spawn.instance.GetObjectFromPool(spawnObject.item, spawnObject.ignoreAllActiveCheck, true, spawnObject.shouldPeek);
                    if (null == spawnedObject)
                    {
                        Debug.LogError("Cannot spawn object, spawn system returned null");
                        return;
                    }

                    ActivateSpawnReactors(ESpawnReactorType.eOnSpawn, spawnObject, spawnedObject);

                    Vector2 newPosition = new Vector2((int)Random.Range(m_boxCollider.bounds.min.x, m_boxCollider.bounds.max.x), (int)Random.Range(m_boxCollider.bounds.min.y, m_boxCollider.bounds.max.y));
                    spawnedObject.transform.position = newPosition;
                }
            }

            ActivateSpawnReactors(ESpawnReactorType.eOnEndSpawning, spawnObject);
        }

        if (m_timedSpawner)
            StartCoroutine(SpawnDelay());
    }

    private void ActivateSpawnReactors(ESpawnReactorType argReactorType, SpawnObject argSpawnObjectData, GameObject argSpawnedObject = null)
    {
        switch (argReactorType)
        {
            case ESpawnReactorType.eOnBeginSpawning:
                foreach(GameObject reactorObject in argSpawnObjectData.spawnReactors)
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
