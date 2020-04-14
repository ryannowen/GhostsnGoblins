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
        MoneyBag,
        Panties,
        Necklace,
        Shoe,
        Ring,
        Dress,
        Doll,
        Crown,
        Key

    }

    public PickupType m_PickupType = PickupType.CopperArmourPickup;
    SpriteRenderer m_SpriteRenderer;
    [SerializeField] private bool m_NeedsInteraction = true;

    [System.Serializable]
    class PickupSprite
    {

        public string name = "";
        public PickupType type = PickupType.CopperArmourPickup;
        public Sprite spr = null;

    }

    // Sprites
    [SerializeField] PickupSprite[] m_Sprites = null;

    [System.Serializable]
    class PickupObject
    {

        public string name = "";
        public PickupType type = PickupType.Lance;
        public GameObject obj = null;

    }

    // Objects
    [SerializeField] PickupObject[] m_Objects = null;

    // Start is called before the first frame update
    void Start()
    {

        m_SpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        Setup();

    }

    void Setup()
    {

        foreach(PickupObject o in m_Objects)
        {

            o.obj = System_Spawn.instance.GetObjectFromPool(o.obj, true);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPickupType(PickupType newType)
    {

        m_PickupType = newType;
        ChangeObjectSprite();
        ChangeNeedsInteraction();
        /*
        switch (m_PickupType)
        {

            case PickupType.CopperArmourPickup:
            case PickupType.SilverArmourPickup:
            case PickupType.GoldArmourPickup:
                m_NeedsInteraction = true;
                break;

            default:
                m_NeedsInteraction = true;
                break;
            
        }
        */

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerController playerC = collision.gameObject.GetComponent<PlayerController>();

            if (m_NeedsInteraction)
                if (!playerC.Interacting())
                    return;

            switch (m_PickupType)
            {

                case PickupType.CopperArmourPickup:
                        playerC.SetArmourPoints(1);
                        playerC.SetArmourType(PlayerController.ArmourType.Copper);
                        Singleton_Sound.m_instance.PlayAudioClipOneShot("ArmourPickup", 0.3f);
                    break;

                case PickupType.SilverArmourPickup:
                        playerC.SetArmourPoints(2);
                        playerC.SetArmourType(PlayerController.ArmourType.Silver);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("ArmourPickup", 0.3f);
                    break;

                case PickupType.GoldArmourPickup:
                        playerC.SetArmourPoints(3);
                        playerC.SetArmourType(PlayerController.ArmourType.Gold);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("ArmourPickup", 0.3f);
                    break;

                case PickupType.Lance:
                case PickupType.Dagger:
                case PickupType.Torch:
                case PickupType.Axe:
                case PickupType.Shield:
                case PickupType.Comb:
                    playerC.SetEquippedItem(FindObjectToEquip());
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.15f);
                    break;

                case PickupType.Coin:
                    Singleton_Game.m_instance.AddScore(200, transform.position);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.15f);
                    break;

                case PickupType.MoneyBag:
                    Singleton_Game.m_instance.AddScore(500, transform.position);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.15f);
                    break;

                case PickupType.Panties:
                    Singleton_Game.m_instance.AddScore(200, transform.position);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.15f);
                    break;

                case PickupType.Necklace:
                    Singleton_Game.m_instance.AddScore(400, transform.position);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.15f);
                    break;

                case PickupType.Shoe:
                    Singleton_Game.m_instance.AddScore(800, transform.position);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.15f);
                    break;

                case PickupType.Ring:
                    Singleton_Game.m_instance.AddScore(1000, transform.position);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.15f);
                    break;

                case PickupType.Dress:
                    Singleton_Game.m_instance.AddScore(2000, transform.position);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.15f);
                    break;

                case PickupType.Doll:
                    Singleton_Game.m_instance.AddScore(400, transform.position);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.15f);
                    break;

                case PickupType.Crown:
                    Singleton_Game.m_instance.AddScore(5000, transform.position);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.15f);
                    break;

                case PickupType.Key:
                    playerC.SetHasKey(true);
                    Singleton_Sound.m_instance.PlayAudioClipOneShot("Pickup", 0.4f);
                    break;

                default:
                    break;

            }

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

    void ChangeNeedsInteraction() 
    {

        switch (m_PickupType)
        {

            case PickupType.CopperArmourPickup:
            case PickupType.SilverArmourPickup:
            case PickupType.GoldArmourPickup:
            case PickupType.Lance:
            case PickupType.Dagger:
            case PickupType.Torch:
            case PickupType.Axe:
            case PickupType.Shield:
            case PickupType.Comb:
                m_NeedsInteraction = true;
                break;

            case PickupType.Coin:
            case PickupType.MoneyBag:
            case PickupType.Panties:
            case PickupType.Necklace:
            case PickupType.Shoe:
            case PickupType.Ring:
            case PickupType.Dress:
            case PickupType.Doll:
            case PickupType.Crown:
            case PickupType.Key:
                m_NeedsInteraction = false;
                break;

            default:
                break;

        }

    }

    GameObject FindObjectToEquip()
    {

        foreach (PickupObject o in m_Objects)
        {

            if (m_PickupType == o.type)
                return o.obj;

        }

        return null;

    }

    // ISpawn
    public void OnSpawn()
    {

        if (m_SpriteRenderer == null)
            m_SpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ChangeObjectSprite();
        ChangeNeedsInteraction();

    }

    public void OnDeSpawn()
    {



    }

}
