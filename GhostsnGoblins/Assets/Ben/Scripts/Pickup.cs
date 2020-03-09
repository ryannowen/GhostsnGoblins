using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, ISpawn
{

    public enum PickupType { 

        CopperArmourPickup,
        SilverArmourPickup,
        GoldArmourPickup,
        Lance, // Goes straight after thrown but slow
        Dagger, // Lance but faster
        Torch, // Thrown upwards, falls untill it hits the ground
        Axe, // Thrown horizontal fast but falls
        Shield, // Limited in range, destroys enemy projectiles, can also hit enemies
        Comb,
        Coin,
        Key,
        MoneyBag

    }

    public PickupType m_PickupType = PickupType.CopperArmourPickup;
    SpriteRenderer m_SpriteRenderer;

    [System.Serializable] struct PickupSprite {

        public string name;
        public PickupType type;
        public Sprite spr;

    }

    // Sprites
    [SerializeField] PickupSprite[] m_Sprites;

    // Start is called before the first frame update
    void Start()
    {

        m_SpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        OnSpawn(); // REMOVE THIS LATER

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPickupType(PickupType newType)
    {

        m_PickupType = newType;
        OnSpawn();

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerController playerC = collision.gameObject.GetComponent<PlayerController>();

            switch (m_PickupType)
            {

                case PickupType.CopperArmourPickup:
                    if (playerC.Interacting())
                    {
                        playerC.SetArmourPoints(1);
                        playerC.SetArmourType(PlayerController.ArmourType.Copper);
                    }
                    break;

                case PickupType.SilverArmourPickup:
                    playerC.SetArmourPoints(2);
                    playerC.SetArmourType(PlayerController.ArmourType.Silver);
                    break;

                case PickupType.GoldArmourPickup:
                    playerC.SetArmourPoints(3);
                    playerC.SetArmourType(PlayerController.ArmourType.Gold);
                    break;

                case PickupType.Lance:
                    playerC.SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("LanceWeapon").gameObject);
                    break;

                case PickupType.Dagger:
                    playerC.SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("DaggerWeapon").gameObject);
                    break;

                case PickupType.Torch:
                    playerC.SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("TorchWeapon").gameObject);
                    break;

                case PickupType.Axe:
                    playerC.SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("AxeWeapon").gameObject);
                    break;

                case PickupType.Shield:
                    playerC.SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("ShieldWeapon").gameObject);
                    break;

                case PickupType.Comb:
                    playerC.SetEquippedItem(new GameObject());
                    break;

                case PickupType.Coin:
                    Singleton_Game.m_instance.AddScore(10);
                    break;

                case PickupType.Key:
                    playerC.SetHasKey(true);
                    break;

                case PickupType.MoneyBag:
                    Singleton_Game.m_instance.AddScore(10);
                    break;

                default:
                    break;

            }

            this.gameObject.SetActive(false);

        }

    }

    // ISpawn
    public void OnSpawn()
    {

        foreach (PickupSprite p in m_Sprites)
        {

            if (m_PickupType == p.type)
                m_SpriteRenderer.sprite = p.spr;

        }

    }

    public void OnDeSpawn()
    {



    }

}
