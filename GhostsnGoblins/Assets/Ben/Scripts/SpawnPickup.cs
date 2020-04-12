using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPickup : MonoBehaviour
{

    [SerializeField] private GameObject m_Pickup = null;

    [System.Serializable]
    class PickupDrop
    {

        public string name = "";
        public Pickup.PickupType type = Pickup.PickupType.CopperArmourPickup;
        public int dropChance = 100;

    }

    // Sprites
    [SerializeField] PickupDrop[] m_AvaliableDrops = null;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreatePickup()
    {

        int PickupToUse = Random.Range(0, m_AvaliableDrops.Length);

        int DropChance = Random.Range(0, 100);

        if (DropChance > m_AvaliableDrops[PickupToUse].dropChance)
        {
            GameObject temp = System_Spawn.instance.GetObjectFromPool(m_Pickup);
            temp.transform.position = this.transform.position;

            // Get random pickup type excluding key
            temp.gameObject.GetComponent<Pickup>().SetPickupType(m_AvaliableDrops[PickupToUse].type);
        }

    }

}
