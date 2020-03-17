using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupObjectPools : MonoBehaviour
{
    //Create a custom struct and apply [Serializable] attribute to it
    [System.Serializable]
    public struct ObjectPool
    {
        public string m_InspectorName;
        public GameObject m_Object;
        public int m_Amount;
    }

    [SerializeField] private int m_setupPoolID = 0;
    [SerializeField] private ObjectPool[] m_Pools = null;

    // Start is called before the first frame update
    void Start()
    {
        // If pool is already registered
        if (!System_Spawn.instance.RegisterSetupObjectPool(m_setupPoolID))
            return;

        // Creates Objects in Pool
        foreach (ObjectPool objectPool in m_Pools)
            System_Spawn.instance.CreatePool(objectPool.m_Object, objectPool.m_Amount);
    }
}
