using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    public Transform spawnPoint; // Transform storing where the player should spawn

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player") // If the gameobject has the "player" tag
        {
            col.gameObject.transform.position = spawnPoint.position; // Moves the gameobject to the spawn point
        }
    }
}
