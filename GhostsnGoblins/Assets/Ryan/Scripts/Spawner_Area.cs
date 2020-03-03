using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner_Area : MonoBehaviour, ISpawner
{
    [SerializeField] private bool m_spawnOnLoad = true;
    [SerializeField] private bool m_timedSpawner = false;
    [SerializeField] private float m_spawnDelaySeconds = 10.0f;
    [Space]
    [SerializeField] private SpawnObject[] m_objects = null;

    private bool m_canSpawnObjects = false;
    private BoxCollider2D m_boxCollider;
    private WaitForSeconds m_spawnWait;

    // Start is called before the firsst frame update
    void Start()
    {
        m_spawnWait = new WaitForSeconds(m_spawnDelaySeconds);
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
        yield return m_spawnWait;
        m_canSpawnObjects = true;
    }

    public void BeginSpawning()
    {
        foreach (SpawnObject spawnObject in m_objects)
        {
            for (int i = 0; i < spawnObject.spawnAmount; i++)
            {
                if (Random.Range(1, 100) >= spawnObject.spawnChance || spawnObject.spawnChance == 100)
                {
                    GameObject spawnedObject = System_Spawn.instance.GetObjectFromPool(spawnObject.item, spawnObject.ignoreAllActiveCheck);
                    if (null == spawnedObject)
                    {
                        Debug.LogError("Cannot spawn object, spawn system returned null");
                        return;
                    }

                    Vector2 newPosition = new Vector2((int)Random.Range(m_boxCollider.bounds.min.x, m_boxCollider.bounds.max.x), (int)Random.Range(m_boxCollider.bounds.min.y, m_boxCollider.bounds.max.y));
                    spawnedObject.transform.position = newPosition;
                }
            }
        }

        if (m_timedSpawner)
            StartCoroutine(SpawnDelay());
    }
}
