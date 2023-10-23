using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{

    public List<GameObject> enemyList = new List<GameObject>(); // List containing all enemys currently in the scene 
    public GameObject closestEnemy; // The closest enemy to the object with this script
    void Update()
    {
        if (enemyList != null)
        {
            CheckDistanceToEnemies(enemyList); // Check our distance to enemies using the list
            ChangeColour(); // Will change the colour of the closest enemy to red
        }
    }
    public void CheckDistanceToEnemies(List<GameObject> objectsToCheck)
    {
        float minDistance = float.MaxValue;
        // Checks every enemy in the list, gets the distance from the enemy to the player (the object with this script on it) and if that enemys distance is less than the current min distance set that as the new distance and that becomes the closest enemy
        foreach (GameObject enemy in objectsToCheck)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
           
            if (distance < minDistance)
            {
                if (closestEnemy != null)
                {
                    closestEnemy.GetComponent<Renderer>().material.SetColor("_Color", Color.white); // Sets the enemys body to white
                }
                closestEnemy = enemy;
                minDistance = distance;
            }
        }
    }

    public void ChangeColour()
    {
       if (closestEnemy != null)
        {
            closestEnemy.GetComponent<Renderer>().material.SetColor("_Color", Color.red); // Sets the enemys body to red
        }
    }
    
}
