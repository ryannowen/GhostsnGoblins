using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupObjectPools : MonoBehaviour
{

    //Create a custom struct and apply [Serializable] attribute to it
    [System.Serializable]
    public struct ObjectPool
    {
        public GameObject m_Object;
        public int m_Amount;
    }

    [SerializeField] private ObjectPool[] m_Pools = null;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < m_Pools.Length; i++)
        {

            System_Spawn.instance.CreatePool(m_Pools[i].m_Object, m_Pools[i].m_Amount, false);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
