using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

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


    // Start is called before the first frame update
    void Start()
    {




    }

    // Update is called once per frame
    void Update()
    {
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
