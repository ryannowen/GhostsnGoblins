using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, ISpawn
{



    public enum PickupType { 
    
        HealthPickup,
        ArmourPickup,
        Lance,
        Dagger,
        Torch,
        Axe,
        Shield

    }

    public PickupType m_PickupType = PickupType.HealthPickup;
    SpriteRenderer m_SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {

        m_SpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

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

                    case PickupType.HealthPickup:
                        collision.gameObject.GetComponent<PlayerController>().AddHealth(1);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.ArmourPickup:
                        collision.gameObject.GetComponent<PlayerController>().AddArmour(1);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Lance:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(GameObject.Find("Pre Loaded").transform.Find("LanceWeapon").gameObject);
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Dagger:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(new GameObject());
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Torch:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(new GameObject());
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Axe:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(new GameObject());
                        this.gameObject.SetActive(false);
                        break;

                    case PickupType.Shield:
                        collision.gameObject.GetComponent<PlayerController>().SetEquippedItem(new GameObject());
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

            case PickupType.HealthPickup:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/HealthPickup") as Sprite;
                break;

            case PickupType.ArmourPickup:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            case PickupType.Lance:
                m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Pickups/ArmourPickup") as Sprite;
                break;

            default:
                break;

        }

    }

    public void OnDeSpawn()
    {



    }

}
