using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, ISpawn
{

    public enum EEquippedWeaponType
    {
        eLance,
        eComb,
        eDagger,
        eShield,
        eTorch,
        eAxe
    }

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
    [SerializeField] private Sprite[] m_Sprites;

    private Sprite sirArthurNude;
    private Sprite sirArthurCopper;
    private Sprite sirArthurSilver;
    private Sprite sirArthurGold;

    private EEquippedWeaponType m_EquippedWeaponType = EEquippedWeaponType.eLance;
    private ArmourType m_Armour = ArmourType.None;

    private bool m_IsInvulnerable = false;
    private float m_InvulnerabilityTimer = 0f;
    private float m_TimeSinceLastShot = 0f;
    private bool usingGUI = false;

    [SerializeField] private GameObject m_StartingWeapon = null;
    [SerializeField] private GameObject m_EquippedItem = null;
    PlayerMovement m_MovementSystem = null;
    SpriteRenderer m_SpriteRenderer = null;

    // Initial parent transform
    private Transform initialParent;

    void Awake()
    {
        m_ID = playerCount;
        playerCount++;

        // Name the player
        this.gameObject.name = "Player" + m_ID;

        Singleton_Game.m_instance.RegisterPlayer(m_ID, gameObject);

        // Set the initial parent of the player.
        initialParent = transform.parent;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_MovementSystem = this.gameObject.GetComponent<PlayerMovement>();
        m_SpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        // Load Sprites
        sirArthurNude = m_Sprites[m_ID-1];
        sirArthurCopper = m_Sprites[m_ID - 1 + 2];
        sirArthurSilver = m_Sprites[m_ID - 1 + 4];
        sirArthurGold = m_Sprites[m_ID - 1 + 6];

        m_EquippedItem = System_Spawn.instance.GetObjectFromPool(m_StartingWeapon, true, true);
        if (m_StartingWeapon.gameObject.GetComponent<WeaponManager>())
            m_FireRate = m_StartingWeapon.gameObject.GetComponent<WeaponManager>().GetFireRate();
        else if (m_StartingWeapon.gameObject.GetComponent<CombManager>())
            m_FireRate = m_StartingWeapon.gameObject.GetComponent<CombManager>().GetFireRate();

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
                m_MovementSystem.SetMovementSpeed(7.5f);
                m_SpriteRenderer.sprite = sirArthurNude;
                break;

            case ArmourType.Copper:
                m_MovementSystem.SetMovementSpeed(7.25f);
                m_SpriteRenderer.sprite = sirArthurCopper;
                break;

            case ArmourType.Silver:
                m_MovementSystem.SetMovementSpeed(7f);
                m_SpriteRenderer.sprite = sirArthurSilver;
                break;

            case ArmourType.Gold:
                m_MovementSystem.SetMovementSpeed(6.75f);
                m_SpriteRenderer.sprite = sirArthurGold;
                break;

            default:
                break;

        }

        ManageStats();

        // Player Shooting
        if (Input.GetAxisRaw("Fire1_P" + m_ID) > 0 && m_TimeSinceLastShot <= 0 && !m_IsInvulnerable && !usingGUI)
        {

            //m_MovementSystem.AddToMovementDelayTimer(m_FireRate);

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

        Singleton_Game.m_instance.GetHUD().GetComponent<HUD>().SetArmourType(m_ID - 1, m_Armour);
        Singleton_Game.m_instance.GetHUD().GetComponent<HUD>().SetArmourValue(m_ID - 1, m_ArmourPoints == 0 ? m_PlayerHealth : m_ArmourPoints);

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

    public int GetHealth()
    {
        return m_PlayerHealth;
    }

    public void SetArmourPoints(int argsAmount)
    {

        m_ArmourPoints = argsAmount;

    }

    public void SetArmourType(ArmourType argsType)
    {

        m_Armour = argsType;

    }

    public void SetEquippedItem(GameObject argsItem, EEquippedWeaponType argsType)
    {

        m_EquippedItem = argsItem;
        if (argsItem.gameObject.GetComponent<WeaponManager>())
            m_FireRate = argsItem.gameObject.GetComponent<WeaponManager>().GetFireRate();
        else if (argsItem.gameObject.GetComponent<CombManager>())
            m_FireRate = argsItem.gameObject.GetComponent<CombManager>().GetFireRate();

    }

    public void SetUsingGUI(bool argsState)
    {
        usingGUI = argsState;
    }

    public GameObject GetEquippedItem()
    {
        return m_EquippedItem;
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

    public Transform originalParent() {
        return initialParent;
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

            Singleton_Sound.m_instance.PlayAudioClipOneShot("DamageInflictedSound", 0.2f);

            // Set Invulnerability
            AddToInvulnerabilityTimer(2f);
            m_MovementSystem.AddToMovementDelayTimer(1f);

        }

    }

    public void KillEntity()
    {

        // Add functionality to kill the player
        print("Player Died");

        // Subtract from player lives
        Singleton_Game.m_instance.AddPlayerLives(-1);

        // Deactivate player
        this.gameObject.SetActive(false);

        // Checks if both players are dead, respawns them at checkpoint if they are
        Singleton_Game.m_instance.CheckPlayersAlive();
    }

    // ISpawn
    public void OnSpawn()
    {
        m_PlayerHealth = 2;
    }

    public void OnDeSpawn()
    {



    }

}
