using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKillable, IDamageable, ISpawn
{

    [SerializeField] private int m_PlayerHealth = 2;
    [SerializeField] private int m_ArmourPoints = 3;

    FireProjectile m_FireProjectile;

    public enum CurrentWeapon
    {

        Lance,
        Dagger,
        Torch,
        Axe,
        Shield

    }

    [SerializeField] private CurrentWeapon m_CurrentWeapon = CurrentWeapon.Dagger;
    [SerializeField] private bool m_InvokeWeaponChange = false;

    [SerializeField] private GameObject m_Lance;
    [SerializeField] private GameObject m_Torch;

    // Start is called before the first frame update
    void Start()
    {

        m_FireProjectile = this.gameObject.GetComponent<FireProjectile>();

    }

    // Update is called once per frame
    void Update()
    {

        ManageStats();

        if (m_InvokeWeaponChange)
            ChangeEquippedWeapon();

        if (Input.GetAxisRaw("Fire1") > 0)
            m_FireProjectile.Shoot(transform.position, transform.rotation);

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

    public void SetWeaponType(CurrentWeapon newType)
    {

        m_CurrentWeapon = newType;

    }

    public void InvokeWeaponChange()
    {

        m_InvokeWeaponChange = true;

    }

    void ChangeEquippedWeapon()
    {

        switch (m_CurrentWeapon)
        {
            case CurrentWeapon.Lance:
                m_FireProjectile.SetProjectile(m_Lance);
                break;

            case CurrentWeapon.Dagger:
                
                break;

            case CurrentWeapon.Axe:

                break;

            case CurrentWeapon.Shield:

                break;

            case CurrentWeapon.Torch:
                m_FireProjectile.SetProjectile(m_Torch);
                break;

            default:
                break;
        }

        m_InvokeWeaponChange = false;

    }

    // IDamageable
    public void TakeDamage(int amount)
    {

        if (m_ArmourPoints > 0)
            m_ArmourPoints -= amount;
        else
            m_PlayerHealth -= amount;

    }

    // IKillable
    public void KillEntity()
    {

        // Add functionality to kill the player

    }

    // ISpawn
    public void OnSpawn()
    {



    }

    public void OnDeSpawn()
    {



    }

}
