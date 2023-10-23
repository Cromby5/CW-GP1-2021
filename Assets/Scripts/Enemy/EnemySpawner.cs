using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    public GameObject spawnPoint; // Spawn point of our enemys

    public float secondsBetweenSpawn;
    float secondsSinceLastSpawn;

    void Start()
    {
        secondsSinceLastSpawn = 0;
    }

    void FixedUpdate()
    {
        // This will only spawn enemies if player is alive
        if (References.player != null) 
        {
            secondsSinceLastSpawn += Time.deltaTime;
            
            if (secondsSinceLastSpawn >= secondsBetweenSpawn)
            {
                Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation); // Create enemy prefab at spawnpoint
                secondsSinceLastSpawn = 0; // Reset spawn timer
            }
        }
    }
}
