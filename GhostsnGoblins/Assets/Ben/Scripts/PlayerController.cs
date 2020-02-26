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

    [SerializeField] private int m_PlayerHealth = 2;
    [SerializeField] private int m_ArmourPoints = 0;
    [SerializeField] private float m_FireRate = 0.5f;

    private ArmourType m_Armour = ArmourType.None;

    private bool m_IsInvulnerable = false;
    private float m_InvulnerabilityTimer = 0f;
    private float m_TimeSinceLastShot = 0f;

    [SerializeField] private GameObject m_EquippedItem;
    PlayerMovement m_MovementSystem = null;

    // Start is called before the first frame update
    void Start()
    {

        m_MovementSystem = this.gameObject.GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {

        // remove this later
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(1);
            m_MovementSystem.TakeKnockback(new Vector3(0, 0, 0), 20);
        }

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
                break;

            case ArmourType.Copper:
                m_MovementSystem.SetMovementSpeed(4f);
                break;

            case ArmourType.Silver:
                m_MovementSystem.SetMovementSpeed(3f);
                break;

            case ArmourType.Gold:
                m_MovementSystem.SetMovementSpeed(2f);
                break;

            default:
                break;

        }

        ManageStats();

        if (m_EquippedItem == null)
            m_EquippedItem = GameObject.Find("Pre Loaded").transform.Find("LanceWeapon").gameObject;

        if (Input.GetAxisRaw("Fire1") > 0 && m_TimeSinceLastShot <= 0 && !m_IsInvulnerable)
        {

            m_MovementSystem.AddToMovementDelayTimer(m_FireRate);

            Vector3 directionToFire = Vector3.zero;

            if (m_MovementSystem.GetMostRecentDirection())
                directionToFire = Vector3.right;
            else
                directionToFire = Vector3.left;

            if (m_EquippedItem.GetComponent<IWeapon>() != null)
                m_EquippedItem.GetComponent<IWeapon>().Action(transform.position, directionToFire);

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
            m_ArmourPoints = 0;

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
