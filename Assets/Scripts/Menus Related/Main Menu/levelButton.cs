using UnityEngine;

/// <summary>
/// Class placed on the console red arrows to switch the act selected
/// </summary>
public class levelButton : MonoBehaviour
{
    public event System.Action ButtonPressed;
    
    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) ClickButton();
    }

    public void ClickButton()
    {
        ButtonPressed?.Invoke();
    }

    // Aaron's arbitrary change
}
