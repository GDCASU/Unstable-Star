using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float xRange = 70;
    [SerializeField] private float lowerYBound = -28;
    [SerializeField] private float upperYBound = 30;

    Vector2 movementVector;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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

        //prevents player from going past the bottom of screen
        if (transform.position.y < lowerYBound)
        {
            transform.position = new Vector2(transform.position.x, lowerYBound);
        }
        //prevents player from going past the top of screen
        if (transform.position.y > upperYBound)
        {
            transform.position = new Vector2(transform.position.x, upperYBound);
        }

        movementVector = PlayerInput.instance.movementInput;
        transform.Translate(movementVector * Time.deltaTime * speed);
    }
}