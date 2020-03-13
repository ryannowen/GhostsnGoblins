using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombManager : MonoBehaviour, IWeapon
{

    [SerializeField] private GameObject m_Projectile = null;
    [SerializeField] private float m_ProjectileSpeed = 5f;
    [SerializeField] private float m_Range = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // IWeapon
    public void Action(GameObject objectFiring, Vector3 argsStartPosition, Vector3 argsDirection)
    {

        // Find the target position

        Vector3 targetPos = argsStartPosition + argsDirection * m_Range;

        GameObject tempProjectile = System_Spawn.instance.GetObjectFromPool(m_Projectile);

        tempProjectile.transform.position = argsStartPosition;
        tempProjectile.transform.rotation = transform.rotation;
        tempProjectile.GetComponent<CombProjectile>().SetPathInfo(objectFiring, targetPos, m_ProjectileSpeed);

    }

}
