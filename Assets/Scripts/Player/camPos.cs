using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camPos : MonoBehaviour
{
    // Transforms of standing/crouch height of the player
    [SerializeField]
    private Transform standingHeight;
    [SerializeField]
    private Transform crouchHeight;

    public GameObject gunOrientation; // Direction gun is facing
    public LayerMask aboveCheck; // Layermask to ignore players collider and anything else you should be able to stand up in with colliders

    private void Awake()
    {
        References.camPos = standingHeight; // On awake set camera to stand height
    }

    public void CrouchPos()
    {
        References.camPos = crouchHeight; // Set camera position to crouch height
        gunOrientation.transform.localPosition = new Vector3 (0,0.3f,0); // Bring down the gun with the camera 
    }

    public bool StandPos()
    {
        // Check in a sphere that extends above the players head if it is clear to stand back up while crouching
        if (!Physics.CheckSphere(crouchHeight.transform.position, 0.5f,aboveCheck) && References.camPos == crouchHeight)
        {
            References.camPos = standingHeight; // Set camera position to standing height
            gunOrientation.transform.localPosition = new Vector3(0, 0.57f, 0); // Moves gun to the standing height
            return true;
        }
        else
        {
            return false;
        }
    }

    //RaycastHit hitAbove;
    // !Physics.Raycast(crouchHeight.transform.position, transform.TransformDirection(standingHeight.up), out hitAbove, 1f) && References.camPos == crouchHeight
}
