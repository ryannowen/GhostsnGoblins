﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner_Area : MonoBehaviour
{
    [SerializeField] private SpawnObject[] objects = null;

    [SerializeField] private bool spawnOnLoad = true;
    [SerializeField] private float spawnDelaySeconds = 10.0f;

    private bool canSpawnObjects = false;
    private BoxCollider2D boxCollider;

    // Start is called before the firsst frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        for (int i = 0; i < objects.GetLength(0); i++)
            if(objects[i].createPool)
                System_Spawn.instance.CreatePool(objects[i].item, objects[i].amount, objects[i].spawnState);

        if (spawnOnLoad)
            SpawnObjects();        
    }

    private void Update()
    {
        if(canSpawnObjects)
            SpawnObjects();
    }

    void SpawnObjects()
    {
        for (int i = 0; i < objects.GetLength(0); i++)
        {
            for (int j = 0; j < objects[i].amount; j++)
            {
                if (Random.Range(1, 100) >= objects[i].spawnChance || objects[i].spawnChance == 100)
                {
                    GameObject spawnedObject = System_Spawn.instance.GetObjectFromPool(objects[i].item, objects[i].ignoreAllActiveCheck);
                    if (spawnedObject == null)
                    {
                        Debug.LogError("Cannot spawn object, spawn system returned null");
                        return;
                    }

                    Vector2 newPosition = new Vector2((int)Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x), (int)Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y));
                    spawnedObject.transform.position = newPosition;
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
