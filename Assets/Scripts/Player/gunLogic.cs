using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gunLogic : MonoBehaviour
{

    public Camera cam; // Stores main camera

    [Header("Bullets")]
    [SerializeField]
    private float secondsBetweenShots; // Fire rate setter
    private float secondsSinceLastShot; // A timer for counting the seconds since the last shot
    public int maxBullets; // Stores max bullets for the weapon
    private int bulletsRemaining; // Stores how many bullets the player has left

    public float drawTime;
    private bool canShoot;

    // Recoil Values
    public float verticalRecoil;
    public float horizontalRecoil;

    public GameObject[] bulletHolePrefabs; // List of bullet hole prefabs

    public float force; // How much force the gun will push objects by

    private Text ammoCount; // Text UI element for showing ammo left
    private Text currentGun; // Text UI for showing what gun we have

    public float damage; // Stores damage this weapon does
    public float range;  // Max distance the gun will fire to 

    public float ReloadTime; // How long it takes to reload this weapon
    public float CurrentReload; // Timer of current reload

    public bool Reloading; // Reloading check
    // Audio for gun
    public AudioSource gunShot;
    public AudioSource gunReload; 

    public GameObject muzzleFlash; // Muzzle Flash Object

    public Rigidbody player; // Players rigidbody

    public Animator anim; // Animator of the gun

    public void Awake()
    {
        anim = GetComponent<Animator>(); // Get the animator from the gun
        secondsSinceLastShot = secondsBetweenShots; // Setting our seconds since the last shot to our seconds between shots so we can fire as soon as gun is aquired
        bulletsRemaining = maxBullets; // Our gun starts with the max bullets remaining
        CurrentReload = 0; // 
    }

    public void Start()
    {
        player = References.player.GetComponent<Rigidbody>(); // Get the players rigidbody
        cam = References.camHolder.GetComponentInChildren<Camera>(); // Get the camera holder
        SetAmmo(); // Set Ammo
        StartCoroutine(DrawGun());
    }

    public IEnumerator DrawGun()
    {
        canShoot = false;
        yield return new WaitForSeconds(drawTime);
        canShoot = true;
    }

    public void Update()
    {
        if (Time.timeScale > 0 && canShoot == true)
        {
            secondsSinceLastShot += Time.deltaTime; // Counting up our seconds since the last shot

            anim.SetFloat("Speed", player.velocity.magnitude); // Animator component takes the players speed and puts it in the speed float set in the animator in unity
            // If button is pressed and our seconds since last shot is greater than or equal to or seconds between shots and we have bullets remaining and the game is unpaused     
            if (Input.GetKeyDown(KeyCode.R) && !Reloading && secondsSinceLastShot >= secondsBetweenShots && bulletsRemaining < maxBullets)
            {
                Reloading = true;
                anim.SetTrigger("Reload"); // Trigger reload in the animator 
                gunShot.PlayOneShot(gunReload.clip); // Play gun reload sound
                ammoCount.text = "Reloading"; // Set UI text to "reloading"
            }

            if (Reloading == true)
            {
                CurrentReload += Time.deltaTime;

                if (CurrentReload >= ReloadTime)
                {
                    Reload();
                }
            }
        }
    }
    public void Fire()
    {
        if (secondsSinceLastShot >= secondsBetweenShots && bulletsRemaining > 0 && Time.timeScale == 1 && !Reloading && canShoot == true)
        {
            anim.SetTrigger("Shoot"); // Trigger Shoot in the animatior 
            Reloading = false; 
            CurrentReload = 0;
            muzzleFlash.SetActive(true); // Trigger muzzle Flash
            RayCast(); // Shoot the ray cast bullet
            player.GetComponent<cameraControl>().SetRecoil(verticalRecoil,horizontalRecoil); // Apply recoil to the camera
            StartCoroutine(GunHandler());
        }
    }
    protected virtual void RayCast()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); // Shoots out a ray at the centre of the camera
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,range))
        {
            GameObject choosenBulletHole = bulletHolePrefabs[Random.Range(0, bulletHolePrefabs.Length)]; // Randomize Bullet Hole
            var tempBullet = Instantiate(choosenBulletHole, hit.point, Quaternion.LookRotation(hit.normal)); // Bullet Hole
            tempBullet.transform.parent = hit.transform; // Put the bullet hole on the object by making it a child

            if (hit.transform.gameObject.tag == "ForceAffected")
            {
                var direction = new Vector3(hit.transform.position.x - transform.position.x, hit.transform.position.y - transform.position.y, hit.transform.position.z - transform.position.z); // Determine the direction we should end up shooting the object in
                hit.rigidbody.AddForceAtPosition(force * Vector3.Normalize(direction), hit.point); // Applys a force in the direction of the shot
            }

            EnemyHealthSystem thierHealthSystem = hit.transform.GetComponent<EnemyHealthSystem>(); // Looking for an enemy health system
            if (thierHealthSystem != null)
            {
                thierHealthSystem.TakeDamage(damage); // Thier health system takes the amount of damage we output to them
            }
        }
    }

    public void Reload()
    {
        bulletsRemaining = maxBullets; // Sets our bullets remaining to the max the gun can hold
        ammoCount.text = "Ammo Remaining " + bulletsRemaining + " / " + maxBullets; // Updates and displays current ammo
        Reloading = false;
        CurrentReload = 0; // Reset Reload timer
    }

    public IEnumerator GunHandler()
    {
        // Play Gunshot Audio
        gunShot.PlayOneShot(gunShot.clip);
        // Reset the seconds since the last shot to 0
        secondsSinceLastShot = 0;
        // Remove a bullet
        bulletsRemaining -= 1;
        // Update ammo count
        ammoCount.text = "Ammo Remaining " + bulletsRemaining + " / " + maxBullets;
        yield return new WaitForSeconds(0.2f);
        muzzleFlash.SetActive(false); // Turn off Muzzle Flash
    }

    public void SetAmmo()
    {
        Reloading = false;
        CurrentReload = 0;
        muzzleFlash.SetActive(false); // Disables muzzle flash 
        ammoCount = GameObject.FindGameObjectWithTag("Ammo").GetComponent<Text>(); // Finds the UI element that will contain our ammo count
        ammoCount.text = "Ammo Remaining " + bulletsRemaining + " / " + maxBullets; // Set our ammo remaining to our current bullets remaining
        currentGun = GameObject.FindGameObjectWithTag("GunSelect").GetComponent<Text>(); // Find the UI element that contains our gun name
        currentGun.text = gameObject.name; // Show the guns name
    }
}