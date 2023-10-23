using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoles : MonoBehaviour
{
    [SerializeField] // Allows the private variable below to show in the inspector
    private float MaxTimeToDespawn; 
    private float timeUntilDespawn; 

    void Update()
    {
        timeUntilDespawn += Time.deltaTime; // Starts counting up the timer

        // Destroy the bullet hole when the despawn time is greater than our max time
        if (timeUntilDespawn > MaxTimeToDespawn) 
        {
            Destroy(gameObject); 
        }
    }
}
