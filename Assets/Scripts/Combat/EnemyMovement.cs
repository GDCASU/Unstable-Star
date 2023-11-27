using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour // Dictates Enemy Behaviours
{
    [Header("Physics Variables")]
    public float acceleration = 0.008f;
    Vector3 minVelocity = new Vector3(0f, 0f, 0f);
    Vector3 maxVelocity;
    Vector3 currentVelocity;

    [Header("Enter Exit Bools")]
    public bool enterScreen;
    public bool exitScreen;
    bool inScreen;

    [Header("Inputted Objects")]
    public Camera mainCamera;

    void Start()
    {
        GameObject gameObject = GetComponent<GameObject>();
        enterScreen = true;
        maxVelocity = new Vector3(0f, acceleration * 20f, 0f);
    }
    void Update()
    { 
        if (enterScreen)
        {
            EnterScreen();
        }
        if (exitScreen)
        {
            ExitScreen();
            enterScreen = false; // makes sure exit and enter don't play at same time
        }

        if (mainCamera.WorldToScreenPoint(gameObject.transform.position).y <= mainCamera.pixelHeight)
        {
            // variable that tells system when ship is in camera view
            inScreen = true;
        }
    }
    public void EnterScreen() // method that causes ship to enter screen
    {
        if (maxVelocity != minVelocity)
        {
            currentVelocity = Vector3.Lerp(maxVelocity, minVelocity, acceleration);
            gameObject.transform.Translate(currentVelocity);
            maxVelocity = currentVelocity;
        }
        else
        {
            enterScreen = false;
            minVelocity = new Vector3(0f, 0f, 0f);
            maxVelocity = new Vector3(0f, .3f, 0f);
        }
    }
    public void ExitScreen() // method that causes ship to exit screen
    {
        if (maxVelocity != currentVelocity)
        {
            currentVelocity = Vector3.Lerp(maxVelocity, minVelocity, 1-acceleration);
            gameObject.transform.Translate(currentVelocity);
            minVelocity = currentVelocity;
        }
        else
        {
            exitScreen = false;
            minVelocity = new Vector3(0f, 0f, 0f);
            maxVelocity = new Vector3(0f, .3f, 0f);
        }
    }
}
