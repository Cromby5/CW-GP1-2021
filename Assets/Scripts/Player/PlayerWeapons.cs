using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Controls")]
    private KeyCode grenadeKey = KeyCode.G;

    [Header("Guns")]
    public List<gunLogic> weapons = new List<gunLogic>(); // Weapon List
    public int selectedWeaponIndex; // Number of weapon to be selected

    [Header("Grenades")]
    public float throwForce; // Force grenade will be thrown at 
    public GameObject grenadePrefab; // Grenade prefab
    private int grenadeCount = 5; // How many grenades the player currently has
    public Transform throwPoint; // Where the grenades will be thrown from and where objects are held

    public Image[] grenadeImage; // Array of images showing our current grenades on UI

    void Start()    
    {
        selectedWeaponIndex = 0; // Selected weapon set to the first weapon in the list 
        grenadeImage = GameObject.FindGameObjectWithTag("GrenadeAmmo").GetComponentsInChildren<Image>(true); // Look for all the images and put them into the array. Including disabled gameobjects
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (weapons.Count > 0 && Input.GetButton("Fire1"))
            {
                weapons[selectedWeaponIndex].Fire();  // Fire the weapon currently equipped by the player
            }

            if (Input.GetButtonDown("Fire2"))
            {
                ChangeWeaponIndex(selectedWeaponIndex + 1); // Increment index by one to switch weapon to the next in the list
            }

            if (Input.GetKeyDown(grenadeKey) && grenadeCount > 0)
            {
                grenadeCount -= 1; // Remove a grenade
                GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, transform.rotation); // Spawn a grenade at the throwpoint
                Rigidbody rb = grenade.GetComponent<Rigidbody>(); // Get its rigidbody component
                rb.AddForce(throwPoint.forward * throwForce, ForceMode.VelocityChange); // Add a instant force forward that ignores the objects mass
            }
            // This switch statement will enable/disable images for our grenade UI sprites to visuallize how many the player has left
            if (grenadeImage[0] != null)
            {
                switch (grenadeCount)
                {
                    case 5:
                        foreach (Image img in grenadeImage)
                        {
                            img.gameObject.SetActive(true);
                        }
                        break;
                    case 4:
                        grenadeImage[0].gameObject.SetActive(true);
                        grenadeImage[1].gameObject.SetActive(true);
                        grenadeImage[2].gameObject.SetActive(true);
                        grenadeImage[3].gameObject.SetActive(true);
                        grenadeImage[4].gameObject.SetActive(false);
                        break;
                    case 3:
                        grenadeImage[0].gameObject.SetActive(true);
                        grenadeImage[1].gameObject.SetActive(true);
                        grenadeImage[2].gameObject.SetActive(true);
                        grenadeImage[3].gameObject.SetActive(false);
                        grenadeImage[4].gameObject.SetActive(false);
                        break;
                    case 2:
                        grenadeImage[0].gameObject.SetActive(true);
                        grenadeImage[1].gameObject.SetActive(true);
                        grenadeImage[2].gameObject.SetActive(false);
                        grenadeImage[3].gameObject.SetActive(false);
                        grenadeImage[4].gameObject.SetActive(false);
                        break;
                    case 1:
                        grenadeImage[0].gameObject.SetActive(true);
                        grenadeImage[1].gameObject.SetActive(false);
                        grenadeImage[2].gameObject.SetActive(false);
                        grenadeImage[3].gameObject.SetActive(false);
                        grenadeImage[4].gameObject.SetActive(false);
                        break;
                    case 0:
                        grenadeImage[0].gameObject.SetActive(false);
                        grenadeImage[1].gameObject.SetActive(false);
                        grenadeImage[2].gameObject.SetActive(false);
                        grenadeImage[3].gameObject.SetActive(false);
                        grenadeImage[4].gameObject.SetActive(false);
                        break;
                }
            }
            else
            {
                grenadeImage = GameObject.FindGameObjectWithTag("GrenadeAmmo").GetComponentsInChildren<Image>(true);
            }
        }
    }

    private void ChangeWeaponIndex(int index)
    {
        selectedWeaponIndex = index;
        // Reset the weapon to the first in the list when we reach the end of the list
        if (selectedWeaponIndex >= weapons.Count)
        {
            selectedWeaponIndex = 0;
        }
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == selectedWeaponIndex)
            {
                weapons[i].gameObject.SetActive(true); // Sets the selected weapon to be active
                weapons[i].SetAmmo(); // Displays ammo in the selected weapon  
                StartCoroutine(weapons[i].DrawGun());
            }
            else
            {
                weapons[i].gameObject.SetActive(false); // Sets the weapons not selected to false
            }
        }
    }

    public void GrenadePickup(int amount)
    {
        grenadeCount += amount; // Adds the amount of grenades picked up
        // If this puts us over the limit keep it at the limit
        if (grenadeCount > 5)
        {
            grenadeCount = 5;
        }
    }

}
