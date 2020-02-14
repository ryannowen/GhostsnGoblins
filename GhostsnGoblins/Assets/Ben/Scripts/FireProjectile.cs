using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{

    [SerializeField] private GameObject m_Projectile;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fire(Vector3 argsStartPosition, Vector3 argsDirection, Quaternion argsRotation) 
    {

        GameObject tempProjectile = System_Spawn.instance.GetObjectFromPool(m_Projectile);

        tempProjectile.transform.position = argsStartPosition;
        tempProjectile.transform.rotation = argsRotation;

    }

    public void SetProjectile(GameObject argsNewProjectile)
    {

        m_Projectile = argsNewProjectile;

    }

}
