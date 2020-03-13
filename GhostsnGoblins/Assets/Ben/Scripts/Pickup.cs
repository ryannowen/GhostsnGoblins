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
    [SerializeField] private bool m_NeedsInteraction = false;

    [System.Serializable]
    class PickupSprite
    {

        public string name = "";
        public PickupType type = PickupType.CopperArmourPickup;
        public Sprite spr = null;

    }

    // Sprites
    [SerializeField] PickupSprite[] m_Sprites = null;

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
        ChangeObjectSprite();

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
                        this.gameObject.SetActive(false);
                    }
                    break;

                case PickupType.SilverArmourPickup:
                    if (playerC.Interacting())
                    {
                        playerC.SetArmourPoints(2);
                        playerC.SetArmourType(PlayerController.ArmourType.Silver);
                        this.gameObject.SetActive(false);
                    }
                    break;

                case PickupType.GoldArmourPickup:
                    if (playerC.Interacting())
                    {
                        playerC.SetArmourPoints(3);
                        playerC.SetArmourType(PlayerController.ArmourType.Gold);
                        this.gameObject.SetActive(false);
                    }
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
                    playerC.SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("CombWeapon").gameObject);
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

            if(!m_NeedsInteraction)
                this.gameObject.SetActive(false);

        }

    }

    void ChangeObjectSprite()
    {

        foreach (PickupSprite p in m_Sprites)
        {

            if (m_PickupType == p.type)
                m_SpriteRenderer.sprite = p.spr;

        }

    }

    // ISpawn
    public void OnSpawn()
    {

        ChangeObjectSprite();

    }

    public void OnDeSpawn()
    {



    }

}
