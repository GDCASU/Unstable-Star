using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;     // Singleton Instance

    [Header("Input Settings")]
    [SerializeField] public float changeShootAngSpeed = 10.0f;
    [SerializeField] public float maxShootAngle = 30.0f;

    [Header("Debugging")]
    [SerializeField] private bool debug = false;

    // Input-Updated Values
    [HideInInspector] private Vector2 movementInput;
    [HideInInspector] public bool shootInput;
    public float shootAngleInput;

    private PlayerControls playerControls;
    private float signAngleMult = 0;

    private void Awake()    // Handle Singleton
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if((signAngleMult > 0 && shootAngleInput < maxShootAngle) || (signAngleMult < 0 && shootAngleInput > -maxShootAngle)) // Keep delta angle in range of the maxShootAngle
            shootAngleInput += signAngleMult * changeShootAngSpeed * Time.deltaTime;
    }

    public void ToggleControls(bool toggle)     // Toggle the player controls with this method from any script
    {
        if (playerControls == null) 
            return;

        if (toggle)
            playerControls.Enable();
        else
            playerControls.Disable();
    }

    private void Start()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            // Subscribe to input events
            playerControls.ShipControls.Move.performed += i => HandleMovementInput(i);      // perfomed event fires when the button is pressed

            playerControls.ShipControls.Shoot.performed += i => HandleShootingInput(i);     // perfomed event fires when the button is pressed
            playerControls.ShipControls.Shoot.canceled += i => HandleShootingInput(i);      // canceled event fires when the button is released

            playerControls.ShipControls.AngleLeft.performed += i => HandleShootAngleInput(i, false);   // perfomed event fires when the button is pressed 
            playerControls.ShipControls.AngleRight.performed += i => HandleShootAngleInput(i, true);   // perfomed event fires when the button is pressed
            playerControls.ShipControls.AngleLeft.canceled += i => HandleShootAngleInput(i, false);     // perfomed event fires when the button is released
            playerControls.ShipControls.AngleRight.canceled += i => HandleShootAngleInput(i, true);   // perfomed event fires when the button is released
        }

        playerControls.Enable();

    }
    private void HandleMovementInput(InputAction.CallbackContext context)   // Just update the movement vector everytime the player moves
    {
        movementInput = context.ReadValue<Vector2>();
        movementInput.Normalize();
        
        if (debug) Debug.Log(movementInput);
    }

    private void HandleShootingInput(InputAction.CallbackContext context)   // Update a simple boolean value when the shoot input is activated
    {
        if (context.performed)
            shootInput = true;      // Whenever the shooting button is pressed = true
        else
            shootInput = false;     // Whenever the shooting button is released = false

        if (debug) Debug.Log(shootInput);
    }

    private void HandleShootAngleInput(InputAction.CallbackContext context, bool isRight)
    {
        if (context.performed)
        {
            // Code to be fired when the player angles the turret
            if (!isRight)       // angle left button was pressed
            {
                signAngleMult = -1f;
            }
            else                // angle right button was pressed
            {
                signAngleMult = 1f;
            }
        }
        else
            signAngleMult = 0;
    }
}
