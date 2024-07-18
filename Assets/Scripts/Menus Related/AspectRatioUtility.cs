using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to preserve aspect ratio of the game and add black bars if not on target
/// </summary>
public class AspectRatioUtility : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float targetRatioX;
    [SerializeField] private float targetRatioY;

    // Local Fields
    private Camera camera;
    private float aspectRatio;
    private float windowAspect;
    private float scaleHeight;
    private float scaleWidth;

    void Start()
    {
        // Set variables
        aspectRatio = targetRatioX / targetRatioY;
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        windowAspect = (float)Screen.width / (float)Screen.height;
        scaleHeight = windowAspect / aspectRatio;
        Adjust();
    }

    public void Adjust()
    {
        // If the scaled height is less than the current height, add letterboxing
        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        // If the scaled height is greater than the current height, add pillarboxing
        else
        {
            scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

    }
}
