using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, ISpawn
{

    [SerializeField] private int m_PlayerHealth = 2;
    [SerializeField] private int m_ArmourPoints = 3;

    [SerializeField] private GameObject m_EquipedItem;
    PlayerMovement m_MovementSystem = null;

    // Start is called before the first frame update
    void Start()
    {

        m_MovementSystem = this.gameObject.GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {

        ManageStats();

        if (m_EquipedItem == null)
            m_EquipedItem = GameObject.Find("Pre Loaded").transform.Find("LanceWeapon").gameObject;

        if (Input.GetAxisRaw("Fire1") > 0)
        {
            Vector3 directionToFire = Vector3.zero;

            if (m_MovementSystem.GetMostRecentDirection())
                directionToFire = Vector3.right;
            else
                directionToFire = Vector3.left;

            m_EquipedItem.GetComponent<IWeapon>().Action(transform.position, directionToFire);
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

    public void AddHealth(int amount)
    {

        m_PlayerHealth += amount;

    }

    public void AddArmour(int amount)
    {

        m_ArmourPoints += amount;

    }

    public void SetEquippedItem(GameObject argsItem)
    {

        m_EquipedItem = argsItem;

    }

    // IDamageable
    public void TakeDamage(int amount)
    {

        if (m_ArmourPoints > 0)
            m_ArmourPoints -= amount;
        else
            m_PlayerHealth -= amount;

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
