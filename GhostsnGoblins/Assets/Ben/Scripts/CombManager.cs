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

        Animation anim = null;
        if (tempProjectile.gameObject.GetComponent<Animation>())
            anim = tempProjectile.gameObject.GetComponent<Animation>();

        if (argsDirection.x > 0)
        {
            tempProjectile.gameObject.transform.localScale = new Vector3(1, 1, 1);
            anim["Spin"].speed = 1;
        }
        else
        {
            tempProjectile.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            anim["Spin"].speed = -1;
        }

        AudioSource a = Singleton_Sound.m_instance.PlayAudioClip("Comberang", 0.2f);
        a.loop = true;

        tempProjectile.transform.position = argsStartPosition;
        tempProjectile.transform.rotation = transform.rotation;
        tempProjectile.GetComponent<CombProjectile>().SetPathInfo(objectFiring, targetPos, m_ProjectileSpeed);
        tempProjectile.GetComponent<CombProjectile>().SetAudioInfo(a);

    }

}
