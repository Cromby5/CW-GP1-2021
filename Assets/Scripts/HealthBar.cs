using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image Fill; // Fill Of health Bar

    public void ShowHealth(float fraction)
    {
        Fill.rectTransform.localScale = new Vector3(fraction, 1, 1); // Scales down the fill to display the new health value on the bar
    }
}
