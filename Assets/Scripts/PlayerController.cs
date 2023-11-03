using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 60.0f;
    private float xRange = 23.5f;

    Vector2 movementVector;

    void Update()
    {
        movePlayer();
    }

    void movePlayer()
    {
        //puts player on other side of screen if touching side walls
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector2(xRange, transform.position.y);
        }
        if (transform.position.x > xRange)
        {
            transform.position = new Vector2(-xRange, transform.position.y);
        }

        movementVector = PlayerInput.instance.movementInput;
        transform.Translate(movementVector * Time.deltaTime * speed);
    }
}