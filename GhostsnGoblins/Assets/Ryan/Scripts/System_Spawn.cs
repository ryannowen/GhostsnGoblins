using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_Spawn : MonoBehaviour
{
    public static System_Spawn instance;

    Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>(); 

    private void Awake()
    {
        if (instance == null && instance != this)
        {
            instance = this;

            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    public void CreatePool(GameObject argGameObject, int argAmount, bool argActive)
    {
        GameObject newGameObject;

        if (objectPool.ContainsKey(argGameObject.name))
        {
            for (int i = 0; i < argAmount; i++)
            {
                newGameObject = Instantiate(argGameObject, transform);
                objectPool[argGameObject.name].Enqueue(newGameObject);

                newGameObject.SetActive(argActive);
            }
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            for (int i = 0; i < argAmount; i++)
            {
                newGameObject = Instantiate(argGameObject, transform);
                newQueue.Enqueue(newGameObject);

                newGameObject.SetActive(argActive);
            }

            objectPool.Add(argGameObject.name, newQueue);
        }
    }

    public GameObject GetObjectFromPool(GameObject argGameObject, bool argIgnoreAllActiveCheck = false)
    {
        if(objectPool.ContainsKey(argGameObject.name))
        {
            if (objectPool[argGameObject.name].Peek().activeSelf && !argIgnoreAllActiveCheck)
            {
                Debug.LogWarning("All objects in queue are active");
            }

            GameObject poolObject = objectPool[argGameObject.name].Dequeue().gameObject;
            objectPool[argGameObject.name].Enqueue(poolObject);
            poolObject.SetActive(true);
            poolObject.GetComponent<ISpawn>().OnSpawn();
   
            return poolObject;

        }
        else
        {
            Debug.LogError("Cannot get object from pool because key is invalid");
            return null;
        }
    }
}