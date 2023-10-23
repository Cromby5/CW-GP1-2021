using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyHealthSystem : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    [Header("Prefabs")]
    public GameObject healthBarPrefab;
    public GameObject deathEffectPrefab;
    private HealthBar myHealthBar;

    private AudioSource death;

    public GameObject tracker; // Object containing the Enemy Tracker 

    private Manager manager;
    private Transform cam;
    [SerializeField] private int score; // Score that the enemy gives when killed 

    void Start()
    {
        // Setting variables using the References class
        manager = References.manager;
        cam = References.camHolder.transform;
        tracker = References.player.gameObject;
        tracker.GetComponent<EnemyTracker>().enemyList.Add(gameObject); // Getting the enemytracker component and adding this enemy onto the list
        death = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioSource>(); // Grabbing Death audio from elsewhere
        currentHealth = maxHealth; // Our current health is set to the max
        myHealthBar = GetComponentInChildren<HealthBar>(); // Gets the healthbar component from the object
    }


    void Update()
    {
        myHealthBar.ShowHealth(currentHealth / maxHealth); // Show current health
        if (Camera.main != null)
        {
            myHealthBar.transform.LookAt(transform.position + cam.forward * 2000); // Looks at main camera
            if (tracker == null)
            {
                tracker = References.player.gameObject;
                cam = References.camHolder.transform;
            }
        }
        else
        {
            myHealthBar.transform.LookAt(transform.position + cam.position); // Looks at the death camera
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount; // Remove the damageamount from the objects current health

            if (currentHealth <= 0)
            {
                if (deathEffectPrefab != null) // Checking to see if the enemy has a death effect such as an explosion
                {
                    Instantiate(deathEffectPrefab, transform.position, transform.rotation); // Spawns the death effect at the death location
                    deathEffectPrefab.transform.localScale = Vector3.zero; // Sets the scale of the death effect to 0 as the effect will expand out on its own rather than being at tits max scale
                }
                death.Play(); // Death sound
                tracker.GetComponent<EnemyTracker>().enemyList.Remove(gameObject); // Removes from list
                manager.AddScore(score);
                Destroy(gameObject); // Destroys this enemy
                Destroy(myHealthBar.gameObject); // Destroys this enemys health bar
            }
        }
    }

    //  Cant figure out how to hide when behind wall effectively and health spawns in centre of screen. Not relevant anymore but might be useful elsewhere
    /*
        float dot = Vector3.Dot((transform.position - Camera.main.gameObject.transform.position).normalized, Camera.main.gameObject.transform.forward);
        if (dot >= 0)
        {
            myHealthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2); // Pushing the transform up by 2 above the other objects head
        }
        else
        {

        }
    }
    else
    {
        myHealthBar.transform.position = deathCamera.WorldToScreenPoint(transform.position + Vector3.up * 2); //To see the health bars from the death camera view
    */
}
