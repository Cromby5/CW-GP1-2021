using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController controller;
    public float speed = 10f; // Creating and setting our speed value

    Vector3 velocity; // Vector3 to store velocity
    public float gravity = -5f; // Creating and setting our gravity value

    public Transform groundCheck; // Transform under player Used to check if we are grounded
    public float groundDistance = 0.4f; // 
    public LayerMask groundMask; // 
    public bool isGrounded; // True/False storing if we are grounded or not

    public int jumpLimit; // Creating and setting our max jump limit
    private int currentJump; // Storing our current jumps left

    private void Awake()
    {
        References.player = gameObject; // Set the player static variable in references to this gameobject
    }

    void Start()
    {
        controller = GetComponent<CharacterController>(); // Gets our Character Controller component from the object this script is attached to and assigns to controller
        currentJump = jumpLimit; // Setting our current jumps to our jump limit
    }
  
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // 

        if (isGrounded && velocity.y < 0) // 
        {
            velocity.y = -2f; // 
            currentJump = 2; // Resets Jump Limit to 2 
        }

        if (Input.GetButtonDown("Jump") && currentJump > 0) // 
        {
            velocity.y = 5f; // Applys a y velocity to make the player go up (jump)
            currentJump -= 1; // Removes a jump from the player
        }

        float x = Input.GetAxis("Horizontal"); // Gets and returns input on the horizontal axis
        float z = Input.GetAxis("Vertical"); // Gets and returns input on the vertical axis

        Vector3 move = transform.right * x + transform.forward * z; // 
        controller.Move(move * speed * Time.deltaTime); // 

        velocity.y += gravity * Time.deltaTime; // Our vertical (y) velocity has gravity * The delta time added on every frame
        controller.Move(velocity * Time.deltaTime); // 

    }
}
