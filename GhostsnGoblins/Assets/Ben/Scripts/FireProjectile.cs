using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{

    private GameObject m_Projectile;
    [SerializeField] private float m_ProjectileSpeed = 5f;

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
        tempProjectile.GetComponent<Rigidbody2D>().AddForce(argsDirection * m_ProjectileSpeed, ForceMode2D.Impulse);

    }

    public void SetProjectile(GameObject argsNewProjectile)
    {

        m_Projectile = argsNewProjectile;

    }

}
