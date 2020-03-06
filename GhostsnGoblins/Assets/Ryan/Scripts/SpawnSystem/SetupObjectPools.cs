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

    [SerializeField] private ObjectPool[] m_Pools = null;

    // Start is called before the first frame update
    void Start()
    {

        foreach (ObjectPool objectPool in m_Pools)
        {
            System_Spawn.instance.CreatePool(objectPool.m_Object, objectPool.m_Amount);
        }

    }
}
