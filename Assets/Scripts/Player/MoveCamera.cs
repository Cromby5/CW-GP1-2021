using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] private Transform cameraPos;
    private void Awake()
    {
        References.camHolder = gameObject; // Set this to be the camera holder in References
        cameraPos = References.camPos; // Get the camera position from References
    }

    void Update()
    {
        // If the camera position exists put this camera holder to the camera position
        if (cameraPos != null)
        {
            transform.position = cameraPos.position;
        }
        // Checking for any camera changes
        cameraPos = References.camPos;
    }

}
