using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private int jumpForce; // Force to apply to rigidbodys entering this object

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody Rigid = collision.gameObject.GetComponent<Rigidbody>(); // Get the hit objects rigidbody
        if (Rigid != null)
        {
            Rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply an instant jump force to the rigidbody
        }
    }
}
