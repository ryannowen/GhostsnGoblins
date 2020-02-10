using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner_Area : MonoBehaviour
{
    [SerializeField] private SpawnObject[] objects = null;
    private BoxCollider2D boxCollider;

    // Start is called before the firsst frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        for (int i = 0; i < objects.GetLength(0); i++)
            System_Spawn.instance.CreatePool(objects[i].item, objects[i].amount, objects[i].spawnState);

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
    }
}
