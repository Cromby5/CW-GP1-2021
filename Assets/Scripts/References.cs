using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class References
{
    // For objects I would like to reference multiple times all over the place without constantly calling getcomponent on multiple scripts etc
    public static Manager manager;
    public static GameObject canvas;
    public static GameObject player;
    public static GameObject camHolder;
    public static Transform camPos;
 
}

