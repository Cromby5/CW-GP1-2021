using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public Camera cam; // Stores main camera

    [SerializeField] private Transform holdPoint; // Point where the object will be held

    public bool currentlyHolding;
    private CanPickup currentItem;

    [SerializeField] private float pickupDistance; // Distance items can be picked up from

    [SerializeField] private float throwForce; // Force the object can be thrown by

    void Start()
    {
        currentlyHolding = false;
        cam = References.camHolder.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (Time.timeScale > 0 && holdPoint != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentlyHolding)
                {
                    DropObject();
                }
                else
                {
                    CheckObject();
                }
            }
            if (Input.GetButton("Fire1") && currentlyHolding)
            {
                DropObject();
                ThrowObject();
            }
        }
    }

    void CheckObject()
    {
        // Cast out a ray at the centre of the camera and check is this an item we can pick up?
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            var check = hit.transform.GetComponent<CanPickup>();
            if (check)
            {
                currentItem = check;
                GrabObject();
            }
        }
    }
    void GrabObject()
    {
        currentItem.rb.isKinematic = true; // Setting rb to Kinematic prevents the object from acting in a way not intended where the object will not follow the hold point and constantly fall
        currentItem.rb.interpolation = RigidbodyInterpolation.None; // Setting to none to prevent lag/stutter behind our hold point
        currentItem.transform.SetParent(holdPoint); // Hold point is the parent object this object will be attached to

        currentlyHolding = true;
    }
    void DropObject()
    {
        currentItem.rb.isKinematic = false;
        currentItem.transform.SetParent(null); // Unparent the object from the hold point
        currentItem.rb.interpolation = RigidbodyInterpolation.Interpolate; // Setting to interpolate to prevent stutter when object flys through the air / moves around
        currentlyHolding = false;
    }

    void ThrowObject()
    {
        currentItem.rb.AddForce(holdPoint.transform.forward * throwForce, ForceMode.VelocityChange); // Throw the object forward from the hold point instantly, force will be same no matter the mass of the object 
    }
}