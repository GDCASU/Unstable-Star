using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    // Reference to the Player Input
    private PlayerInput playerInput;

    private RectTransform crosshairTransform;

    private void Awake()
    {
        playerInput = PlayerInput.instance;
        crosshairTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (crosshairTransform == null)
            return;

        crosshairTransform.anchoredPosition = PlayerInput.instance.cursorAimScreenPoint;
    }
}
