using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveRB : MonoBehaviour
{
    [Header("Controls")]
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode runKey = KeyCode.LeftShift;
    private KeyCode crouchKey = KeyCode.C;

    [Header("Speed")]
    private float speed; 
    public float walkSpeed; 
    public float crouchSpeed; 
    public float runSpeed;

    //Movement Multipliers
    public float groundMultiply; // Used to multiply the ground movement 
    public float airMultiply; // Used to multiply the air movement 

    [Header("Jump")]
    public float jumpForce; // Force to multiply our jump 
    private bool jumpCheck;
    public float gravity = 9.81f; // Gravity to affect player

    private float playerHeight = 2.010491f; // The players height
    private float crouchHeight = 1f; // The players crouch height

    //Rigidbody Drag 
    [SerializeField] private float groundDrag; // Players Rigidbody Drag to be used on the ground to stop the player sliding all over the place
    [SerializeField] private float airDrag; // Players Rigidbody Drag to be used on the air to help prevent the player moving the same speed and being able to fly through the air at crazy speeds

    private Rigidbody rigidPlayer; // Rigidbody of the player
    private CapsuleCollider playerCollider; // CapsuleCollider of the player
    public camPos playerCam; // Players Camera position (Does not contain the camera only the position the camera will be at)

    Vector3 moveDirection; // Direction the player moves in

    public Transform PlayerOrientation; // The direction the player is facing

    public int jumpLimit; // Creating and setting our max jump limit
    [SerializeField] private int currentJump; // Storing our current jumps left

    [Header("Ground")]
    public bool isGrounded; // True/False storing if we are grounded or not
    public Transform groundCheck; // Transform under player Used to check if we are grounded
    public float groundDistance = 0.4f; // Distance for the players ground check as a radius of a sphere
    public LayerMask groundMask; // Layer mask containing the ground layer

    [Header("Stair Management")]
    // Upper and Lower ray to check if the player can step up
    [SerializeField] private GameObject BoxRayUpper;
    [SerializeField] private GameObject BoxRayLower; 
    [SerializeField] private float stepHeight = 0.3f; // Max height the player can step up by
    [SerializeField] private float stepAmount = 10f; // The ammount the player will go up the steps every call

    private Manager manager;

    private void Awake()
    {
        References.player = gameObject; // This object is stored as the player in the references script
    }

    public enum CharacterState
    {
        // All possible states that the character can be in that will affect areas of movement
        Walking,
        Running,
        Crouching,
        Slowed,
        Speed,
    }

    public CharacterState myState;

    void Start()
    {
        manager = References.manager;
        // Gets the players rigidbody and Capsule Collider
        rigidPlayer = GetComponent<Rigidbody>();  
        playerCollider = GetComponent<CapsuleCollider>();

        myState = CharacterState.Walking; // Set our state to walking
      
        playerHeight = playerCollider.height; // Height is set to the collider height
        crouchHeight = playerHeight / 2; // Crouch height is half of the players normal height

        // To set where upper ray will be to determine what size of step we should be allowed to go up
        BoxRayUpper.transform.localPosition = new Vector3(BoxRayUpper.transform.localPosition.x, BoxRayLower.transform.localPosition.y + stepHeight, BoxRayUpper.transform.localPosition.z);

        currentJump = jumpLimit; // Our current jumps is set to the limit
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Checks the sphere from the groundcheck object position by what the ground distance is set to while checking the object is in the groundmask. returns true/false
            // Switch statement for determining what should happen when the character is in a certain state
            switch (myState)
            {
                case CharacterState.Walking:
                    speed = walkSpeed;
                    break;
                case CharacterState.Running:
                    speed = runSpeed;
                    break;
                case CharacterState.Crouching:
                    speed = crouchSpeed;
                    break;
                case CharacterState.Slowed:
                    speed = crouchSpeed;
                    break;
                case CharacterState.Speed:
                    speed = runSpeed;

                    break;
            }
            if (isGrounded)
            {
                currentJump = jumpLimit; // Resets Jump Limit 
            }

            // Checking for our jump input and if we have a jump stored and not already currently jumping
            if (Input.GetKeyDown(jumpKey) && currentJump > 0 && !jumpCheck)
            {
                jumpCheck = true;
            }
            // Checking for our run input and only allows us to run on the ground if we are walking 
            if (Input.GetKey(runKey) && isGrounded && myState != CharacterState.Crouching && myState != CharacterState.Slowed && myState != CharacterState.Speed)
            {
                myState = CharacterState.Running;
                manager.FovChange(true);
            }
            // Else we are setting the character to walking state if they are not anything else
            else if(myState != CharacterState.Crouching && myState != CharacterState.Slowed && myState != CharacterState.Speed)
            {
                myState = CharacterState.Walking;
                manager.FovChange(false);
            }
            // Checking for our crouch input
            if (Input.GetKey(crouchKey))
            {
                myState = CharacterState.Crouching; // Change state to crouching
                playerCollider.height = crouchHeight; // Collider is dropped down to the crouch height
                playerCollider.center = new Vector3(0f, -0.5f, 0f); // The new center is set for the collider so it matches the model
                playerCam.CrouchPos(); // Call the camera function for crouch position to set the cam and gun positions
            }
            else if (playerCam.StandPos())
            {
                playerCollider.height = playerHeight; // Reset collider to default height
                playerCollider.center = new Vector3(0f, -0.009597272f, 0f); // Resets the center of the collider to its default
                myState = CharacterState.Walking; // Walking state
            }
            MoveInput(); // Checks for movement input
            ChangeDrag(); // Changing the drag
        }
    }

    void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
        JumpMove(); // Applys force of jump if the jump input is detected from update
        MovePlayer(); // Moves the player using the input
        CheckStep(); // Check if their is something the player can step over
        ControlGravity(); // To allow use of own gravity code rather than unitys built in rigidbody gravity
        }
    }
    void MoveInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDirection = PlayerOrientation.forward * z + PlayerOrientation.right * x; // This determines our move direction in the x and z planes (blue/red in inspector) 
    }

    void JumpMove()
    {
        if (jumpCheck)
        {
            jumpCheck = false; // Allows the player to jump again
            rigidPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Adds a instant force on the rigidbody going up defined by our jump force
            currentJump -= 1; // Remove a jump from the player
        }
    }

    void MovePlayer()
    {
        // If grounded apply an Acceleration of the ground force with the move direction and speed 
        if (isGrounded)
        {
            rigidPlayer.AddForce(moveDirection.normalized * speed * groundMultiply, ForceMode.Acceleration);
        }
        // If not grounded apply an Acceleration of the air force with the move direction and speed 
        else if (!isGrounded)
        {
            rigidPlayer.AddForce(moveDirection.normalized * speed * airMultiply, ForceMode.Acceleration);
        }
    }

    void ChangeDrag()
    {
        if (isGrounded)
        {
            rigidPlayer.drag = groundDrag; // Ground drag 
        }
        else if (!isGrounded)
        {
            rigidPlayer.drag = airDrag; // Changing drag in air to prevent air movement being the same speed as the ground
        }
    }

    void ControlGravity()
    {
        Vector3 v3 = rigidPlayer.velocity; // Grabbing the velocity of the rigidbody
        // If grounded the y velocity will be 0 as it won't be falling/rising
        if (isGrounded)
        {
            if (v3.y < 0f)
            {
                v3.y = 0f;
            }
        }
        // Apply Gravity * Time to the y velocity 
        else if (!isGrounded)
        {
            v3.y -= gravity * Time.deltaTime;
        }
        rigidPlayer.velocity = v3; // Rigidbody Velocity set to the calculated velocity
    }

    void CheckStep()
    {
        // Check if we are moving before attempting to go up a step by checking our movement vector is not (0,0,0) to prevent the player from jumping up and down constantly
        if (moveDirection != Vector3.zero)
        {
            // Checks the lower box to see if we hit the ground
            if (Physics.CheckBox(BoxRayLower.transform.position, BoxRayLower.transform.lossyScale, BoxRayLower.transform.rotation, groundMask))
            {
                // Checks the upper box to check the area we want to step to is clear
                if (!Physics.CheckBox(BoxRayUpper.transform.position, BoxRayUpper.transform.lossyScale, BoxRayUpper.transform.rotation, groundMask))
                {
                    rigidPlayer.position -= new Vector3(0f, -stepAmount * Time.deltaTime, 0f); // Set the player to the new position and smooth it out using time.deltatime
                }
            }
        }
    }
}