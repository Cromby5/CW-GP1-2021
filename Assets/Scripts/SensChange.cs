using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensChange : MonoBehaviour
{

    public Slider sensSlide; // Slider for our sensetivity to allow changes
    private float sensValue; // Value of our current sensetivity
    void Start()
    {
        sensSlide = gameObject.GetComponent<Slider>(); // Get slider component
        sensValue = cameraControl.sensitivity; // Set value of sensetivity to the camera sensetivity 
        SensTrack();
    }

    void Update()
    {
        SensTrack();
    }

    void SensTrack()
    {
        sensValue = sensSlide.value; // Setting the sensetivity to the sliders 
        cameraControl.sensitivity = sensValue; // Sets the cameras to the new sens
    }
}
