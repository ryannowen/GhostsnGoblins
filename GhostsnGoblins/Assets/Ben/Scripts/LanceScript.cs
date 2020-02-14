using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceScript : MonoBehaviour, IWeapon
{

    [SerializeField] private GameObject m_Projectile;

    // Start is called before the first frame update
    void Start()
    {

        this.gameObject.GetComponent<FireProjectile>().SetProjectile(m_Projectile);

    }

    // Update is called once per frame
    void Update()
    {
        


    }

    // IWeapon
    public void Action()
    {



    }

}
