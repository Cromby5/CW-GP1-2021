using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    public HealthBar myHealthBar;

    void Start()
    {
        currentHealth = maxHealth; // Our current health is set to the max we can have
        myHealthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>(); // Find the health bar component on the healthbar tagged object
    }

    void Update()
    {
        myHealthBar.ShowHealth(currentHealth / maxHealth); // Show health by dividing current by max to get a fraction for the show health function
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
