using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPickup : MonoBehaviour
{
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Storing the objects rigidbody to be accessed later
    }
}
