using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField] private float bulletSpeed; // Speed of bullet
    [SerializeField] private float secondsUntilDestory; // Timer for seconds until we destroy the bullet
    [SerializeField] private float damage; // Damage of bullet

    void Start()
    {
        // Get the rigidbody component from the object this script is attached to
        Rigidbody RigidBullet = GetComponent<Rigidbody>();
        // Set the velocity of this rigidbody to go forward at the speed of the bullet
        RigidBullet.velocity = transform.forward * bulletSpeed;
    }

    void Update()
    {
        // Count down a timer to when the object gets destroyed
        secondsUntilDestory -= Time.deltaTime;

        // Start to scale down the object to give the apperence of it fading out naturally 
        if (secondsUntilDestory < 1)
        {
            transform.localScale *= secondsUntilDestory;  // Object scale is set to the amount of seconds 
        }
        // Destroy the bullet when we hit 0 or less seconds remaining on the timer
        if (secondsUntilDestory <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        GameObject HitObject = collision.gameObject; // Assign our hit object to a gameobject variable

        if (HitObject.transform.tag == "Player") // Checking if it has a Player tag
        {
            HealthSystem thierHealthSystem = HitObject.GetComponent<HealthSystem>(); // Gets the players health system 
            if (thierHealthSystem != null)
            {
                thierHealthSystem.TakeDamage(damage); // They take the amount of damage that is passed through by this bullet
            }
            Destroy(gameObject); // Destroys bullet
        }

    }
}
    
