using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    void Awake()
    {
        References.canvas = gameObject; // Making the canvas availiable in References 
    }
}
