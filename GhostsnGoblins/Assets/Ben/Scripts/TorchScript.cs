using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour, IWeapon
{

    [SerializeField] private GameObject m_Projectile;
    FireProjectile m_FireProjectile = null;

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
    public void Action(Vector3 argsStartPosition, Vector3 argsDirection)
    {

        m_FireProjectile.Fire(argsStartPosition, argsDirection, transform.rotation);

    }

}
