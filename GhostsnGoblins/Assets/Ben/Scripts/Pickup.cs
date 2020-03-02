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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            if (collision.gameObject.GetComponent<PlayerController>())
            {

                switch (m_PickupType)
                {

                    case PickupType.CopperArmourPickup:
                        collision.gameObject.GetComponent<PlayerController>().SetArmourPoints(1);
                        collision.gameObject.GetComponent<PlayerController>().SetArmourType(PlayerController.ArmourType.Copper);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.SilverArmourPickup:
                        collision.gameObject.GetComponent<PlayerController>().SetArmourPoints(2);
                        collision.gameObject.GetComponent<PlayerController>().SetArmourType(PlayerController.ArmourType.Silver);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.GoldArmourPickup:
                        collision.gameObject.GetComponent<PlayerController>().SetArmourPoints(3);
                        collision.gameObject.GetComponent<PlayerController>().SetArmourType(PlayerController.ArmourType.Gold);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Lance:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("LanceWeapon").gameObject);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Dagger:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("DaggerWeapon").gameObject);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Torch:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("TorchWeapon").gameObject);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Axe:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("AxeWeapon").gameObject);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Shield:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("ShieldWeapon").gameObject);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Comb:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(new GameObject());
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Coin:

                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Key:

                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.MoneyBag:

                        this.gameObject.SetActive(false);
                        break;

                    default:
                        break;

                }

            }

        }

    }

    // ISpawn
    public void OnSpawn()
    {

        switch (m_PickupType)
        {

            case PickupType.CopperArmourPickup:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            case PickupType.SilverArmourPickup:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            case PickupType.GoldArmourPickup:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            case PickupType.Lance:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            case PickupType.Dagger:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            case PickupType.Torch:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            case PickupType.Axe:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            case PickupType.Shield:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            case PickupType.Comb:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            case PickupType.Coin:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/Coin") as Sprite;
                break;

            case PickupType.Key:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/Key") as Sprite;
                break;

            case PickupType.MoneyBag:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/Money Bag") as Sprite;
                break;

            default:
                break;

        }

    }

    public void OnDeSpawn()
    {



    }

}
