using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacobPlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float xRange = 10;
    Vector2 movementVector = PlayerInput.instance.movementInput;

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
        //don't go pass walls
        /*
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector2(-xRange, transform.position.y);
        }
        if (transform.position.x > xRange)
        {
            transform.position = new Vector2(xRange, transform.position.y);
        }
        */
        transform.Translate(movementVector * Time.deltaTime * speed);
    }
}
