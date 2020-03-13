﻿using System.Collections;
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