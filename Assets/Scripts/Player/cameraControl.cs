using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraControl : MonoBehaviour
{

    public static float sensitivity = 200f; // Sensitivity of the players mouse

    public Transform Cam; // The transform of the camholder that stores the players camera
    public Transform PlayerOrientation; // Where cam is facing
    public Transform GunOrientation; // Where gun is facing

    float xRotation = 0f; // Represents the x rotation
    float yRotation = 0f; // Represents the y rotation

    private float verticalRecoil = 0; // To take in Guns vertical recoil
    private float horizontalRecoil = 0; // To take in Guns horizontal recoil

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks cursor to game window
        Cam = References.camHolder.transform; // The cam holder that contains our main and weapon camera
    }

    void Update()
    {
        float mouseX = horizontalRecoil + Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // Mouse X axis is taken and returned then * by the sensitivity then * Deltatime
        float mouseY =  verticalRecoil + Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; // Mouse Y axis is taken and returned then * by the sensitivity then * Deltatime
        // After adding the recoil set back to 0
        verticalRecoil = 0; 
        horizontalRecoil = 0;

        xRotation -= mouseY; 
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -70f, 70f); // Clamps the camera so it wont move past a certain angle so we can't look straight down or straight up as it will be locked when we hit x angle

        // To get the camera, Gun and players body to face the direction we want to face when moving our mouse
        Cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        GunOrientation.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        PlayerOrientation.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void SensChange (float SensValue)
    {
        sensitivity = SensValue; // Changes sensitivity to the new value passed in
    }

    public void SetRecoil(float vert, float horz)
    {
        verticalRecoil += vert; // Adds the Vertical Recoil

        // To add or subtract the Horizontal Recoil
        int i = Random.Range(0, 2); // Upper limit 2 wont be returned
        if (i == 0)
        {
            horizontalRecoil += horz; // Goes Right
        }
        else if ( i == 1)
        {
            horizontalRecoil = -horz; // Goes Left
        }
    }
}
