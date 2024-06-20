using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectRatio : MonoBehaviour
{
    // Set the desired aspect ratio (4:3 in this case)
    public float targetAspect = 4.0f / 3.0f;

    private void Update()
    {
        Camera camera = Camera.main;

        // Get the current screen aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Calculate the scale height to fit the target aspect ratio
        float scaleHeight = windowAspect / targetAspect;

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
        else // If the scaled height is greater than the current height, add pillarboxing
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
}
}
