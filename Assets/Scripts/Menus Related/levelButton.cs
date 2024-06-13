using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class placed on the console red arrows to switch the act selected
/// </summary>
public class levelButton : MonoBehaviour
{
    public event System.Action ButtonPressed;
    
    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Invoke event on button pressed
            ButtonPressed?.Invoke();
        }
    }
}
