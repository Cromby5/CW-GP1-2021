using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurret : MonoBehaviour
{
    public Vector3 targetDir; // Direction we should target towards
    GameObject player; 
    public float speed; // Speed of turret rotation

    public float maxFollowAngle; // Max angle for turret to track from
    public float MaxVisibilityDistance; // How far the turret can see
    
    public Renderer turretBody; // Visible body of the turret
    public GameObject turretArm; // The gun arm of the turret

    [Header("Shooting")]
    public GameObject turretTip; // Tip of gun turret where bullets are spawned from
    public GameObject bulletPrefab; // Stores our Bullet prefab
    public float maxFireAngle; // Angle where the turret attempts to fire at the player
    public float shootTimerMax; // How long before each shot should be taken
    private float shootTimer; // Counting until the turret can fire again

    private bool turretActive; // True/false if turret is active and following the player
    private float dot; // Storing the dot product

    public Light visionCone; // To visualise the turrets vision cone

    void Start()
    {
        player = References.player; // Finding The player
        turretActive = false;
        StartCoroutine("CheckDistance");
    }

    void Update()
    {
        if (player != null)
        {
            if (turretActive)
            {
                // Setting the light to red 
                visionCone.color = Color.red;
                // The turret body / arm are set to turn green
                turretBody.material.SetColor("_Color", new Color32(1, 255, 1, 1)); 
                turretArm.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 1, 0)); 

                targetDir = player.transform.position - transform.position; // Our target direction is going to be the players position - the turrets transform
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDir, speed * Time.deltaTime, 0.0f); // We get the direction to rotate towards the target direction at a defined speed 
                transform.rotation = Quaternion.LookRotation(newDirection); // Carry out the rotation using the newdirection

                if (dot > maxFireAngle)
                {
                    // The turrets body and arm turn red when you are in the angle to be shot at then the timer until the next shot starts ticking 
                    turretBody.material.SetColor("_Color", new Color(1, 0, 0)); 
                    turretArm.GetComponent<Renderer>().material.SetColor("_Color", Color.red); 

                    shootTimer += Time.deltaTime; 

                    if (shootTimer > shootTimerMax)
                    {
                        Shoot();
                    }
                }
            }
            else
            {
                visionCone.color = Color.white; // Vision cone light back to white
                // Turret body and arm are set to black to show it has became inactive and the timer is reset
                turretBody.material.SetColor("_Color", Color.black); 
                turretArm.GetComponent<Renderer>().material.SetColor("_Color", Color.black); 
                shootTimer = 0;
            }
        }
        else
        {
            player = References.player;
        }
    }
    private bool PlayerVisible()
    {
        // If the dot product of our player - the turrets position is greater than our max follow angle return true
        if (player != null)
        {
            dot = Vector3.Dot(transform.forward, (player.transform.position - transform.position).normalized);
            if (dot > maxFollowAngle) 
                return true;
            return false;
        }
        else return false;
    }

    private bool PlayerwithinDistance()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position,transform.position); // Returns the distance between the player and our turret
        if (distance < MaxVisibilityDistance) // We check if the distance returned is less than the max visibillity distance and return true/false
            return true;
        return false;
        }
        else return false;
    }

    private void Shoot()
    {
        // Spawns a bullet at the tip of the gun arm that will be fired out and reset the timer
        Instantiate(bulletPrefab, turretTip.transform.position , turretTip.transform.rotation); 
        shootTimer = 0;
    }
    public IEnumerator CheckDistance()
    {
        // Infinite loop
        for (; ; )
        {
            turretActive = PlayerwithinDistance() ? PlayerVisible() : false; // Checks if the player is within the distance and if the player is visible and returns true if both are true
            yield return new WaitForSeconds(0.1f); // To prevent this from happening every frame
        }
    }
}
