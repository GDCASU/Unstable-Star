using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour // Dictates Enemy Behaviours
{
    [Header("Physics Variables")]
    public float acceleration = 0.008f;
    public float moveSpeed = 5f;
    Vector3 minVelocity = new Vector3(0f, 0f, 0f);
    Vector3 maxVelocity;
    Vector3 currentVelocity;

    [Header("Enter Exit Bools")]
    public bool enterScreen;
    public bool exitScreen;
    bool inScreen;

    private float enterSpeed = 20f;
    private bool moveDown = true;
    private bool moveLeft = false;
    private float percentUpScreen = 0.9f;

    private BasicEnemy enemyComponent;          // TODO: really bad code Combat Entity needs another child called Enemy

    void Start()
    {
        enemyComponent = GetComponent<BasicEnemy>();

        enterScreen = true;
        maxVelocity = new Vector3(0f, acceleration * 20f, 0f);
    }
    void Update()
    { 
        if (enterScreen)
        {
            //EnterScreen();
            if (moveDown) EnterScreenSpace();
        }
        if (exitScreen)
        {
            ExitScreen();
            enterScreen = false; // makes sure exit and enter don't play at same time
        }
        if (inScreen)
        {
            Move();
        }

        if (!inScreen && Camera.main.WorldToScreenPoint(gameObject.transform.position).y <= Camera.main.pixelHeight)
        {
            // variable that tells system when ship is in camera view
            inScreen = true;
            enemyComponent.canShoot = true;
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

    private void Move()
    {
        if (moveLeft)
        {
            //transform.Translate(Vector3.left * Time.deltaTime * speed);
            transform.position = transform.position + new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
            moveLeft = Camera.main.WorldToViewportPoint(transform.position).x > 0f;
        }
        else
        {   // Move right
            //transform.Translate(Vector3.right * Time.deltaTime * speed);
            transform.position = transform.position + new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
            moveLeft = Camera.main.WorldToViewportPoint(transform.position).x > 1f;
        }
    }

    public void EnterScreenSpace()
    {
        transform.Translate(Vector3.down * Time.deltaTime * enterSpeed);                        // Move Down.
        moveDown = Camera.main.WorldToViewportPoint(transform.position).y > percentUpScreen;    // Check if should move down again.

        //if (!moveDown) GetComponent<EnemyHealth>().ToggleInvulnerable(false);                    // toggle invulnerable off
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
