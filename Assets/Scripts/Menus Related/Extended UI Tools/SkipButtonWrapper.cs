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
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class SkipButtonWrapper : MonoBehaviour
{
    Button btnSkip;

    private void Start()
    {
        btnSkip = GetComponent<Button>();
    }

    private void OnEnable()
    {
        PlayerInput.OnCancel += SkipClicked;
    }

    private void OnDestroy()
    {
        PlayerInput.OnCancel -= SkipClicked;
    }

    private void OnDisable()
    {
        PlayerInput.OnCancel -= SkipClicked;
    }

    void SkipClicked()
    {
        btnSkip.onClick.Invoke();
    }
}