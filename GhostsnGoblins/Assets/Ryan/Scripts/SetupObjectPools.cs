using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupObjectPools : MonoBehaviour
{

    [SerializeField] private GameObject[] m_Pools = null;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < m_Pools.Length; i++)
        {

            System_Spawn.instance.CreatePool(m_Pools[i], 20, false);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
