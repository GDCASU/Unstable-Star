using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 60.0f;
    Vector2 movementVector;

    // Local Variables
    private Vector2 screenBounds;
    private float objectHeight = 3;
    private float playerModelHeight;
    private Vector3 viewPos;

    private void Awake()
    {
        // Get the boundary points of the play space
        // FIXME: Is this the best way to do this?
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
            Camera.main.transform.position.z));
    }


    private void Start()
    {

    }

    void Update()
    {
        // Update position of player
        
        movePlayer();
    }

    private void LateUpdate()
    {
        viewPos = this.transform.position;
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + playerModelHeight, screenBounds.y * -1 - playerModelHeight);
        transform.position = viewPos;
    }

    void movePlayer()
    {
        // TODO: Detect Player approaching the edge of the screen




        // Move player according to input
        movementVector = PlayerInput.instance.movementInput;
        transform.Translate(movementVector * Time.deltaTime * speed);
        // Restrict player movement here
        this.transform.position = movementVector;
    }
}