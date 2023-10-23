using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer Mixer;
    public void SetLevel(float sliderValue)
    {
        Mixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }
}