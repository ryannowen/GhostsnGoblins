using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public healthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            maxHealth = 1;
            currentHealth = maxHealth;
            healthBar.fill.color = new Color(239,15,30,255);
            healthBar.setMaxHealth(maxHealth);
            healthBar.setHealth(currentHealth);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            maxHealth = 2;
            currentHealth = maxHealth;
            healthBar.fill.color = new Color(192,192,192,255);
            healthBar.setMaxHealth(maxHealth);
            healthBar.setHealth(currentHealth);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            maxHealth = 3;
            currentHealth = maxHealth;
            healthBar.fill.color = new Color(255,215,0,255);
            healthBar.setMaxHealth(maxHealth);
            healthBar.setHealth(currentHealth);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage(1);
        }
    }

    void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }
}
