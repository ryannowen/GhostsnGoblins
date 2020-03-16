using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, ISpawn
{

    public enum ArmourType
    {

        None,
        Copper,
        Silver,
        Gold

    }

    private static int playerCount = 1;
    [SerializeField] private int m_ID = 0;

    [SerializeField] private int m_PlayerHealth = 2;
    [SerializeField] private int m_ArmourPoints = 0;
    [SerializeField] private float m_FireRate = 0.5f;
    [SerializeField] private bool m_HasKey = false;

    // Sprites
    private Sprite sirArthurNude;
    private Sprite sirArthurCopper;
    private Sprite sirArthurSilver;
    private Sprite sirArthurGold;

    private ArmourType m_Armour = ArmourType.None;

    private bool m_IsInvulnerable = false;
    private float m_InvulnerabilityTimer = 0f;
    private float m_TimeSinceLastShot = 0f;

    [SerializeField] private GameObject m_EquippedItem = null;
    PlayerMovement m_MovementSystem = null;
    SpriteRenderer m_SpriteRenderer = null;

    // Start is called before the first frame update
    void Start()
    {

        m_ID = playerCount;
        playerCount++;

        // Name the player
        this.gameObject.name = "Player" + m_ID;

        m_MovementSystem = this.gameObject.GetComponent<PlayerMovement>();
        m_SpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        // Load Sprites
        sirArthurNude = Resources.Load<Sprite>("Sprites/Character/SirArthur_Nude") as Sprite;
        sirArthurCopper = Resources.Load<Sprite>("Sprites/Character/SirArthur_Copper") as Sprite;
        sirArthurSilver = Resources.Load<Sprite>("Sprites/Character/SirArthur_Silver") as Sprite;
        sirArthurGold = Resources.Load<Sprite>("Sprites/Character/SirArthur_Gold") as Sprite;

    }

    // Update is called once per frame
    void Update()
    {

        if (m_TimeSinceLastShot > 0f)
            m_TimeSinceLastShot -= Time.deltaTime;

        if (m_InvulnerabilityTimer > 0f)
            m_InvulnerabilityTimer -= Time.deltaTime;

        m_IsInvulnerable = (m_InvulnerabilityTimer <= 0f) ? false : true;

        // Armour Weights
        switch(m_Armour)
        {

            case ArmourType.None:
                m_MovementSystem.SetMovementSpeed(5f);
                m_SpriteRenderer.sprite = sirArthurNude;
                break;

            case ArmourType.Copper:
                m_MovementSystem.SetMovementSpeed(4f);
                m_SpriteRenderer.sprite = sirArthurCopper;
                break;

            case ArmourType.Silver:
                m_MovementSystem.SetMovementSpeed(3f);
                m_SpriteRenderer.sprite = sirArthurSilver;
                break;

            case ArmourType.Gold:
                m_MovementSystem.SetMovementSpeed(2f);
                m_SpriteRenderer.sprite = sirArthurGold;
                break;

            default:
                break;

        }

        ManageStats();

        if (Input.GetAxisRaw("Fire1_P" + m_ID) > 0 && m_TimeSinceLastShot <= 0 && !m_IsInvulnerable)
        {

            m_MovementSystem.AddToMovementDelayTimer(m_FireRate);

            Vector3 directionToFire = Vector3.zero;

            if (m_MovementSystem.GetMostRecentDirection())
                directionToFire = Vector3.right;
            else
                directionToFire = Vector3.left;

            if (m_EquippedItem != null)
                if (m_EquippedItem.GetComponent<IWeapon>() != null)
                    m_EquippedItem.GetComponent<IWeapon>().Action(this.gameObject, transform.position, directionToFire);

            m_TimeSinceLastShot = m_FireRate;

        }

    }

    void ManageStats()
    {

        // Limiting the players health points
        if (m_PlayerHealth > 2)
            m_PlayerHealth = 2;
        if (m_PlayerHealth <= 0)
            m_PlayerHealth = 0;

        // Limiting the players armour points
        if (m_ArmourPoints > 3)
            m_ArmourPoints = 3;
        if (m_ArmourPoints <= 0)
        {
            m_ArmourPoints = 0;
            m_Armour = ArmourType.None;
        }

        // Check for player death
        if (m_PlayerHealth <= 0)
            KillEntity();

    }

    void AddToInvulnerabilityTimer(float timeToAdd)
    {

        m_InvulnerabilityTimer += timeToAdd;

    }

    public void AddHealth(int amount)
    {

        m_PlayerHealth += amount;

    }

    public void SetArmourPoints(int argsAmount)
    {

        m_ArmourPoints = argsAmount;

    }

    public void SetArmourType(ArmourType argsType)
    {

        m_Armour = argsType;

    }

    public void SetEquippedItem(GameObject argsItem)
    {

        m_EquippedItem = argsItem;

    }
    
    public bool GetIsInvulnerable()
    {
        return m_IsInvulnerable;
    }

    public void SetHasKey(bool argsValue)
    {
        m_HasKey = argsValue;
    }

    public bool HasKey()
    {
        return m_HasKey;
    }

    public int GetID()
    {
        return m_ID;
    }

    public bool Interacting()
    {
        if (Input.GetAxisRaw("Fire3_P" + m_ID) > 0)
            return true;
        else
            return false;
    }

    // IDamageable
    public void TakeDamage(int amount)
    {

        if (!m_IsInvulnerable)
        {

            if (m_ArmourPoints > 0)
                m_ArmourPoints -= amount;
            else
                m_PlayerHealth -= amount;

            // Set Invulnerability
            AddToInvulnerabilityTimer(2f);
            m_MovementSystem.AddToMovementDelayTimer(1.5f);

        }

    }

    public void KillEntity()
    {

        // Add functionality to kill the player
        print("Player Died");
        this.gameObject.SetActive(false);

    }

    // ISpawn
    public void OnSpawn()
    {

        m_PlayerHealth = 2;
        m_ArmourPoints = 3;

    }

    public void OnDeSpawn()
    {



    }

}
