using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour // Changes camera position based on mouse movement
{

    //public static bool enabled = true;
    public static Vector3 defaultCameraRotation;
    public static Vector3 defaultCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        defaultCameraRotation = Camera.main.transform.eulerAngles;
        defaultCameraPosition = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.transform.position == defaultCameraPosition)
        {
            Camera.main.transform.eulerAngles = getTargetRotation();
        }
        
    }

    public static Vector3 getTargetRotation()
    {
        // Get the mouse position
        float freedom = 6;
        float maxX = Screen.width;
        float midX = maxX / 2;
        float mouseX = Mathf.Max(Mathf.Min(Input.mousePosition.x, maxX), 0);
        float rotation = freedom * Mathf.Sign(mouseX - midX) * Mathf.Pow(Mathf.Abs(((mouseX - midX) / midX)), 2);
        return defaultCameraRotation + new Vector3(0f, rotation, 0f);
    }
}
