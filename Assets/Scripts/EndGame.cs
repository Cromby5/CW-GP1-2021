using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private Manager manager; // The game manager
    [SerializeField] private GameObject thisCanvas; 

    private bool canInteract;
    private bool bought; // Tracking if the player has bought the end

    void Start()
    {
        manager = References.manager;
        bought = false;
    }

    void Update()
    {
        // Has the player pressed the interact key while able to press the button with enough score (while the game is playing)
        if (canInteract && manager.totalScore >= 2000 && Input.GetKeyDown(KeyCode.F) && Time.timeScale > 0)
        {
            StartCoroutine(manager.EndGame());
            // Disables canvas popup and wont allow it to be active again
            thisCanvas.SetActive(false);
            bought = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bought == false)
        {
            // Lets the player interact with the end button and brings up the UI showing them what it does
            canInteract = true;
            thisCanvas.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (bought == false)
        {
            // Closes UI and the player can no longer interact with the object
            canInteract = false;
            thisCanvas.SetActive(false);
        }
    }
}
