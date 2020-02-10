using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawn_Area : MonoBehaviour
{
    [System.Serializable]
    struct SSpawnObject
    {
        public GameObject item;
        public int amount;

        [Range(0, 100)]
        public int spawnChance;
    }

    [SerializeField] private SSpawnObject[] objects = null;
    private BoxCollider2D boxCollider;

    // Start is called before the firsst frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        for (int i = 0; i < objects.GetLength(0); i++)
            System_Spawn.instance.CreatePool(objects[i].item.name, objects[i].item, objects[i].amount);

        for (int i = 0; i < objects.GetLength(0); i++)
        {
            for (int j = 0; j < objects[i].amount; j++)
            {
                int chance = Random.Range(1, 100);
                if (chance >= objects[i].spawnChance || objects[i].spawnChance == 100)
                {
                    GameObject gameObject = System_Spawn.instance.GetObjectFromPool(objects[i].item.name);
                    if (gameObject == null)
                    {
                        Debug.LogError("Cannot spawn object, spawn system returned null");
                        return;
                    }

                    gameObject.SetActive(true);

                    Vector2 newPosition = new Vector2((int)Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x), (int)Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y));
                    gameObject.transform.position = newPosition;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
