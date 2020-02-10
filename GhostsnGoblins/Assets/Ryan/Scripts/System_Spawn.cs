using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_Spawn : MonoBehaviour
{
    public static System_Spawn instance;

    [SerializeField] public int test = 10;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePool(GameObject argGameObject, int argAmount)
    {
        GameObject gameobject;

        if (objectPool.ContainsKey(argGameObject.name))
        {
            for (int i = 0; i < argAmount; i++)
            {
                gameobject = Instantiate(argGameObject, transform);
                objectPool[argGameObject.name].Enqueue(gameobject);

                gameobject.SetActive(false);
            }
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            for (int i = 0; i < argAmount; i++)
            {
                gameobject = Instantiate(argGameObject, transform);
                newQueue.Enqueue(gameobject);

                gameobject.SetActive(false);
            }

            objectPool.Add(argGameObject.name, newQueue);
        }
    }

    public GameObject GetObjectFromPool(string argKey)
    {
        if(objectPool.ContainsKey(argKey))
        {
            if (objectPool[argKey].Peek().activeSelf)
            {
                Debug.LogWarning("All objects in queue are active");
                return null;
            }

            GameObject gameObject = objectPool[argKey].Dequeue().gameObject;
            objectPool[argKey].Enqueue(gameObject);
            gameObject.SetActive(true);

            return gameObject;

        }
        else
        {
            Debug.LogError("Cannot get object from pool because key is invalid");
            return null;
        }
    }
}
