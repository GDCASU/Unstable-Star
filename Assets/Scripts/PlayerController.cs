using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
<<<<<<< Updated upstream

    private Vector2 playerMovementVector;
    private Vector2 mousePosition;
    public float moveSpeed = 10f;
    // public Rigidbody2D playerRB;
    public Transform playerTF;

    //Todo:  should be assigned in the Start()
    [SerializeField]private float _boundaryXMax;
    [SerializeField] private float _boundaryXMin;
    [SerializeField] private float _boundaryYMax;
    [SerializeField] private float _boundaryYMin;

    //  public Camera camRef;

=======
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float xRange = 70;
    [SerializeField] private float lowerYBound = -28;
    [SerializeField] private float upperYBound = 30;

    Vector2 movementVector;
>>>>>>> Stashed changes

    // Start is called before the first frame update
    void Start()
    {

<<<<<<< Updated upstream



=======
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        playerMovementVector.x = Input.GetAxisRaw("Horizontal");
        playerMovementVector.y = Input.GetAxisRaw("Vertical");
        // mousePosition = camRef.ScreenToWorldPoint(Input.mousePosition);

        playerMovementVector.Normalize();
        MovePlayer(playerMovementVector.x, playerMovementVector.y);

    }



    /// <summary>
    /// Move the player, within the Y bound and jump around the X bound
    /// </summary>
    /// <param name="x">The direction of x axis, more of a velocity(dx) </param>
    /// <param name="y">The direction of y axis, more of a velocity(dy)</param>
    void MovePlayer(float x, float y)
    {

        Vector3 position = playerTF.transform.position;


        //Out of bound Checker
        //if (position.y>= _boundaryYMax&&y>0|| position.y <= _boundaryYMin && y < 0)
        //{
        //    y = 0;
        //}
        //if (position.x >= _boundaryXMax && x > 0 || position.x <= _boundaryXMin && x < 0)
        //{
        //    x = 0;
        //}


        position += new Vector3(x * moveSpeed * Time.deltaTime, y * moveSpeed * Time.deltaTime);


        //if out of y bound, make it on the bound
        if (position.y >= _boundaryYMax)
        {
            position.y = _boundaryYMax;
        }
        if (position.y<= _boundaryYMin)
        {
            position.y = _boundaryYMin;
        }


        //if out of x bound, jump to the other side of bound
        if (position.x >= _boundaryXMax && x > 0) {
            position.x = _boundaryXMin;
        }
        if (position.x <= _boundaryXMin && x < 0) {
            position.x = _boundaryXMax;
        }
        playerTF.transform.position = position;


    }
}
=======
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
>>>>>>> Stashed changes
