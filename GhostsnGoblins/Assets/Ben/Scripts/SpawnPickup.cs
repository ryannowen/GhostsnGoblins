using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPickup : MonoBehaviour
{

    [SerializeField] private GameObject m_Pickup = null;

    // Start is called before the first frame update
    void Start()
    {
        CreatePickup();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreatePickup()
    {

        GameObject temp = System_Spawn.instance.GetObjectFromPool(m_Pickup);
        temp.transform.position = this.transform.position;
        temp.gameObject.GetComponent<Pickup>().SetPickupType((Pickup.PickupType)Random.Range(0, 11));

    }

}
