/*
 * By: Aaron Huggins
 * 
 * Note:
 * 
 * This was a short and easy way for me to hook up the skip button to the input system, get rid of this
 * if you have time to properly implement the cutscene's ui input system interaction
 * (PLEASE)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkipButtonWrapper : MonoBehaviour
{
    Button btnSkip;

    private void Start()
    {
        btnSkip = GetComponent<Button>();

        PlayerInput.OnPauseGame += SkipClicked;
    }

    private void OnDestroy()
    {
        PlayerInput.OnPauseGame -= SkipClicked;
    }

    void SkipClicked()
    {
        btnSkip.onClick.Invoke();
    }
}
