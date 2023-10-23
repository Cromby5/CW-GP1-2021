using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : MonoBehaviour
{
    [SerializeField] private int amount; // Amount of grenades to give the player 

    private void OnTriggerEnter(Collider other)
    {
        // If the player enters the collider the weapons component is called so we can add the amount of grenades this pickup has to the players grenade count
        if (other.transform.CompareTag("Player"))
        {
            PlayerWeapons playerWeapon = other.gameObject.GetComponent<PlayerWeapons>();
            playerWeapon.GrenadePickup(amount); 
            Destroy(gameObject);
        }
    }
}
