using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public bool paused; // True/False for being paused

    public Slider fovSlider; // Stores our fov slider
    private float runFov;
    public Camera cam; // Stores our main camera

    public Camera deathCam; // Stores our camera to switch to upon death
    // Cameras audioListeners
    public AudioListener mainListen;
    public AudioListener deathListen;

    public GameObject gameUi; // GameUI canvas elements 
    public GameObject pauseUi; // PauseUI canvas elements 
    public GameObject deathUi; // DeathUI canvas elements 
    public GameObject WinUi; // WinUi canvas elements 

    public Image img; // Image for death cam transition

    public GameObject PlayerPrefab; 

    private float respawnTimer;
    public float maxRespawnTime;

    public Text respawningTxt;
    public Text scoreTxt;

    public int totalScore;

    private void Awake()
    {
        References.manager = this; // Set this as the manager in References
    }

    void Start()
    {
        // Disable all Ui elements that are not the players hud
        pauseUi.gameObject.SetActive(false); 
        deathUi.gameObject.SetActive(false); 
        WinUi.gameObject.SetActive(false); 
        gameUi.gameObject.SetActive(true);
        // Enables the main camera and its audio listener
        cam.enabled = true; 
        mainListen.enabled = true;
        // Disables the death camera and its audio listener
        deathListen.enabled = false; 
        deathCam.enabled = false;
        respawnTimer = maxRespawnTime;
        scoreTxt.text = "Score: " + totalScore; // Shows score on the ui
        paused = false; // Unpause by setting this to false
        cam.fieldOfView = fovSlider.value; // Set FOV to sliders default FOV value
        runFov = fovSlider.value + 10; // Run fov is always 10 more than fov set by user
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !paused) // If P key is pressed and the game is not paused we will pause the game
        {
            PauseGame();
            return;
        }
        if (Input.GetKeyDown(KeyCode.P) && paused) // If P key is pressed and the game is paused we will unpause the game
        {
            UnPauseGame();
        }
        if (References.player != null)
        {
            // Do Nothing
        }
        else
        {
            StartCoroutine(FadeImage(true)); // Fades the image 
            cam.enabled = false; // Disables main camera
            mainListen.enabled = false; // Disables main camera audio listener
            deathCam.enabled = true; // Enables death cam
            deathListen.enabled = true; // Enables death cam audio listener
            gameUi.gameObject.SetActive(false); // Disable the player HUD
            deathUi.gameObject.SetActive(true); // Enable the death UI

            maxRespawnTime -= Time.deltaTime;
            float seconds = Mathf.FloorToInt(maxRespawnTime % 60) + 1; // Converts to seconds to be displayed as int instead of float
            respawningTxt.text = "Respawning " + seconds; // Display the amount of seconds to respawn
            if (maxRespawnTime <= 0)
            {
                Respawn();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // TimeScale is set to 0 meaning no time will pass
        paused = !paused; // Sets paused to true
        Cursor.lockState = CursorLockMode.None; // Cursor unlocked for player to use cursor on menu items  
        // Enables the pause UI and disables the games UI
        pauseUi.gameObject.SetActive(true); // Enables pause menu
        gameUi.gameObject.SetActive(false); // Disable Player HUD
        
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1; // TimeScale is set to 1 meaning time will pass as normal
        paused = !paused; // Sets paused to false
        Cursor.lockState = CursorLockMode.Locked; // Cursor locked
        // Enables the game UI and disables the pause UI
        pauseUi.gameObject.SetActive(false);
        gameUi.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit game
    }

    public IEnumerator EndGame()
    {
        // Game over message
        WinUi.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f); 
        // Resets the game scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Respawn()
    {
        Instantiate(PlayerPrefab, transform.position, transform.rotation); // Create new player at the managers postion (change to spawn point needed)
        cam.enabled = true; // Enables cam again
        mainListen.enabled = true; // Enables cam listener
        deathCam.enabled = false; // Disable overhead deathcam
        deathListen.enabled = false; // Disable deathcam listener
        totalScore -= 500; // Remove 500 score
        scoreTxt.text = "Score: " + totalScore; // Display new score
        gameUi.gameObject.SetActive(true); // Enable player HUD
        deathUi.gameObject.SetActive(false); // Disable death ui
        maxRespawnTime = respawnTimer; // Reset respawn timer
    }
    public void FovChange(bool isRunning)
    {
        if (isRunning)
        {
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, runFov, Time.deltaTime * 100); // Moves towarads the run fov smoothly then caps out at the runfov
        }
        else
        {
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, fovSlider.value, Time.deltaTime * 100); // Zooms back in to the normal fov the player has
            runFov = fovSlider.value + 10; // Setting run fov
        }
    }

    public void AddScore(int score)
    {
        // Keeping track of and displaying the players score
        totalScore += score;
        scoreTxt.text = "Score: " + totalScore;
    }
    IEnumerator FadeImage(bool fadeAway)
    {
        // Fade from opaque to transparent
        if (fadeAway)
        {
            // Loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        // Fade from transparent to opaque
        else
        {
            // Loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // Set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }

}
