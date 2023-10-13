using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Vector2 playerMovementVector;
    private Vector2 mousePosition;
    public float moveSpeed = 10f;
    public Rigidbody2D playerRB;
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
        playerRB.MovePosition(playerRB.position + playerMovementVector * moveSpeed * Time.fixedDeltaTime);
    }
}
