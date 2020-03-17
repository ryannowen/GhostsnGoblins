using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_Spawn : MonoBehaviour
{
    public static System_Spawn instance;

    [SerializeField] private int m_seed = 500;

    private Dictionary<string, Queue<GameObject>> m_objectPool = new Dictionary<string, Queue<GameObject>>();
    private List<int> m_setupObjectPoolIDs = new List<int>();
    private GameObject objectPoolContainer = null;

    private void Awake()
    {
        if (null == instance && this != instance)
        {
            Random.InitState(m_seed);

            instance = this;
            objectPoolContainer = new GameObject("System_Spawn_ObjectPool");
            objectPoolContainer.transform.parent = transform;
        }
        else
            Destroy(gameObject);
    }

    public void CreatePool(GameObject argGameObject, int argAmount)
    {
        GameObject parent;
        Queue<GameObject> queue;

        if (m_objectPool.ContainsKey(argGameObject.name))
        {
            parent = objectPoolContainer.transform.Find("P_" + argGameObject.name).gameObject;
            queue = m_objectPool[argGameObject.name];
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            m_objectPool.Add(argGameObject.name, newQueue);
            queue = newQueue;

            parent = new GameObject("P_" + argGameObject.name);
            parent.transform.parent = objectPoolContainer.transform;
        }

        for (int i = 0; i < argAmount; i++)
        {
            GameObject newGameObject = Instantiate(argGameObject, parent.transform);
            newGameObject.SetActive(false);

            queue.Enqueue(newGameObject);
        }
    }

    public GameObject GetObjectFromPool(GameObject argGameObject, bool argIgnoreAllActiveCheck = false)
    {
        if (null == argGameObject)
        {
            Debug.LogError("Cannot Get object from pool because given object is null");
            return null;
        }

        if(m_objectPool.ContainsKey(argGameObject.name))
        {
            if (m_objectPool[argGameObject.name].Peek().activeSelf && !argIgnoreAllActiveCheck)
            {
                Debug.LogWarning("All objects in queue are active, given key=" + argGameObject.name);
            }

            GameObject poolObject = m_objectPool[argGameObject.name].Dequeue().gameObject;
            m_objectPool[argGameObject.name].Enqueue(poolObject);

            poolObject.SetActive(true);

            ISpawn spawnInterface = poolObject.GetComponent<ISpawn>();

            if (null != spawnInterface)
                spawnInterface.OnSpawn();
   
            return poolObject;
        }
        else
        {
            Debug.LogError("Cannot get object from pool because key is invalid, given key=" + argGameObject.name);
            return null;
        }
    }

    public bool RegisterSetupObjectPool(int argSetupPoolID)
    {
        if (m_setupObjectPoolIDs.Contains(argSetupPoolID))
            return false;

        m_setupObjectPoolIDs.Add(argSetupPoolID);
        return true;
    }
}