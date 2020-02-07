using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawn_Area : MonoBehaviour
{
    [System.Serializable]
    struct SSpawnObject
    {
        public string prefabName;
        public GameObject item;
        public int amount;

        [Range(0, 100)]
        public int spawnChance;
    }

    [SerializeField] private SSpawnObject[] objects;
    private BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();

        for (int i = 0; i < objects.Length; i++)
            System_Spawn.instance.CreatePool(objects[i].prefabName, objects[i].item, objects[i].amount);

        for (int i = 0; i < objects.Length; i++)
        {
            int chance = 100 - Random.Range(0, 100);
            if (chance < objects[i].spawnChance)
            {
                Debug.Log(chance);
                continue;
            }

            GameObject gameObject = System_Spawn.instance.GetObjectFromPool(objects[i].prefabName);
            if (gameObject == null)
            {
                Debug.Log("Bloop");
                return;
            }

            gameObject.SetActive(true);

            Vector2 newPosition = new Vector2(Random.Range(collider.bounds.min.x, collider.bounds.max.x), Random.Range(collider.bounds.min.y, collider.bounds.max.y));
            gameObject.transform.position = newPosition;

            Debug.Log("Spawned " + objects[i].prefabName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
