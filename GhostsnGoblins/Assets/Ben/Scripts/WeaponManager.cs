using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour, IWeapon
{

    FireProjectile m_FireProjectile = null;
    [SerializeField] private GameObject m_Projectile = null;
    [SerializeField] private Vector3 shotDirectionAddition = Vector3.zero;
    [SerializeField] private Vector3 singleShotModification = Vector3.one;
    [SerializeField] private float m_Firerate = 1f;

    [System.Serializable]
    class ProjectilesToFire
    {

        public string name = "";
        public Vector3 shootingDirectionAddition = Vector3.zero;
        public Vector3 shootingDirectionModification = Vector3.one;

    }

    // Sprites
    [SerializeField] ProjectilesToFire[] m_Projectiles = null;

    // Start is called before the first frame update
    void Start()
    {

        m_FireProjectile = this.gameObject.GetComponent<FireProjectile>();
        m_FireProjectile.SetProjectile(m_Projectile);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // IWeapon
    public void Action(GameObject objectFiring, Vector3 argsStartPosition, Vector3 argsDirection)
    {

        if (m_Projectiles.Length == 0)
        {

            Singleton_Sound.m_instance.PlayAudioClipOneShot("Throw", 0.3f);

            Vector3 shotDirection = new Vector3((argsDirection.x + shotDirectionAddition.x) * singleShotModification.x, (argsDirection.y + shotDirectionAddition.y) * singleShotModification.y, (argsDirection.z + shotDirectionAddition.z) * singleShotModification.z);

            shotDirection.Normalize();

            m_FireProjectile.Fire(argsStartPosition, shotDirection, transform.rotation);

        } else
        {

            foreach(ProjectilesToFire p in m_Projectiles)
            {

                Singleton_Sound.m_instance.PlayAudioClipOneShot("Throw", 0.3f);

                Vector3 shotDirection = new Vector3((argsDirection.x + p.shootingDirectionAddition.x) * p.shootingDirectionModification.x, (argsDirection.y + p.shootingDirectionAddition.y) * p.shootingDirectionModification.y, (argsDirection.z + p.shootingDirectionAddition.z) * p.shootingDirectionModification.z);
                //Vector3 shotDirection = Vector3.one;
                shotDirection.Normalize();

                m_FireProjectile.Fire(argsStartPosition, shotDirection, transform.rotation);

            }

        }

    }

    public float GetFireRate()
    {
        return m_Firerate;
    }

}
