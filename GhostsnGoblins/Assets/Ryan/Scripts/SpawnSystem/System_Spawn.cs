using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class System_Spawn : MonoBehaviour
{
    [System.Serializable]
    public enum ESpawnType
    {
        eUndefined,
        eEnemy
    }

    [System.Serializable]
    public struct SSpawnTypeNames
    {
        public string name;
        public ESpawnType type;
    }

    [SerializeField] private SSpawnTypeNames[] m_spawnTypeNames = null;

    public static System_Spawn instance;

    [SerializeField] private int m_seed = 500;

    private Dictionary<string, Queue<GameObject>> m_objectPool = new Dictionary<string, Queue<GameObject>>();
    private List<int> m_setupObjectPoolIDs = new List<int>();
    private GameObject objectPoolContainer = null;
    private List<ISpawner> m_levelSpawners = new List<ISpawner>();
    private List<ISpawnReactor> m_levelReactors = new List<ISpawnReactor>();

    private void Awake()
    {
        if (null == instance && this != instance)
        {
            UnityEngine.Random.InitState(m_seed);

            instance = this;
            objectPoolContainer = new GameObject("System_Spawn_ObjectPool");
            objectPoolContainer.transform.parent = transform;

            for(int i = 0; i < m_spawnTypeNames.Length; i++)
            {
                GameObject typeName = new GameObject(m_spawnTypeNames[i].name);
                typeName.transform.parent = objectPoolContainer.transform;
            }
        }
        else
            Destroy(gameObject);
    }

    public void CreatePool(GameObject argGameObject, int argAmount, ESpawnType argSpawnType = ESpawnType.eUndefined)
    {
        GameObject typeParent = null;
        GameObject parent;
        Queue<GameObject> queue;

        for (int i = 0; i < m_spawnTypeNames.Length; i++)
        {
            if (m_spawnTypeNames[i].type == argSpawnType)
            {
                typeParent = objectPoolContainer.transform.Find(m_spawnTypeNames[i].name).gameObject;
                break;
            }
        }

        if (m_objectPool.ContainsKey(argGameObject.name))
        {
            parent = typeParent.transform.Find("P_" + argGameObject.name).gameObject;
            queue = m_objectPool[argGameObject.name];
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();

            m_objectPool.Add(argGameObject.name, newQueue);
            queue = newQueue;

            parent = new GameObject("P_" + argGameObject.name);



            parent.transform.parent = typeParent.transform;
        }

        for (int i = 0; i < argAmount; i++)
        {
            GameObject newGameObject = Instantiate(argGameObject, parent.transform);
            newGameObject.name = "PL_" + newGameObject.name;
            newGameObject.SetActive(false);
            queue.Enqueue(newGameObject);
        }
    }

    public GameObject GetObjectFromPool(GameObject argGameObject, bool argIgnoreAllActiveCheck = false, bool argActivateGameObject = true, bool argShouldPeek = false)
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

            GameObject poolObject;
            if (argShouldPeek)
            {
                poolObject = m_objectPool[argGameObject.name].Peek().gameObject;
            }
            else
            {
                poolObject = m_objectPool[argGameObject.name].Dequeue().gameObject;
                m_objectPool[argGameObject.name].Enqueue(poolObject);                
            }

            if(argActivateGameObject)
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
    public void DisableAllSpawns()
    {
        GameObject player1 = Singleton_Game.m_instance.GetPlayer(0);
        GameObject player2 = Singleton_Game.m_instance.GetPlayer(0);
        
        player1.transform.parent = player1.GetComponent<PlayerController>().originalParent();
        player2.transform.parent = player2.GetComponent<PlayerController>().originalParent();

        DisableChildren(objectPoolContainer.transform);
    }

    private void DisableChildren(Transform argParentTransform)
    {
        foreach(Transform childTransform in argParentTransform)
        {
            if(childTransform.gameObject.name.Contains("PL_"))
                childTransform.gameObject.SetActive(false);

            DisableChildren(childTransform);
        }
    }

    public void DisableAllSpawnType(ESpawnType argSpawnType)
    {
        for(int i = 0; i < m_spawnTypeNames.Length; i++)
        {
            if(m_spawnTypeNames[i].type == argSpawnType)
                DisableChildren(objectPoolContainer.transform.Find(m_spawnTypeNames[i].name));
        }
    }

    public void RegisterSpawner(GameObject argSpawner)
    {
        ISpawner spawner = argSpawner.GetComponent<ISpawner>();

        if (null == spawner)
            return;

        m_levelSpawners.Add(spawner);
    }

    public void RegisterIReactor(GameObject argReactor)
    {
        ISpawnReactor reactor = argReactor.GetComponent<ISpawnReactor>();

        if (null == reactor)
            return;

        m_levelReactors.Add(reactor);
    }

    public void ClearSpawners()
    {
        m_levelSpawners.Clear();
        m_levelReactors.Clear();
    }

    public void ResetRegisteredIReactors()
    {
        foreach (ISpawnReactor reactor in m_levelReactors)
        {
            reactor.ReactorReset();
        }
    }

    public void ResetRegisteredISpawners()
    {
        foreach (ISpawner spawner in m_levelSpawners)
        {
            spawner.ResetSpawner();
        }
    }

    public void ActivateRegisteredSpawners()
    {
        foreach(ISpawner spawner in m_levelSpawners)
        {
            spawner.ForceSpawn();
        }
    }
}