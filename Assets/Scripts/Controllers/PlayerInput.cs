using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;     // Singleton Instance

    [Header("Input Settings")]
    public float changeShootAngSpeed = 30.0f;
    public float maxShootAngle = 30.0f;

    [Header("Debugging")]
    [SerializeField] private bool debug = false;
    public float shootAngleInput; // holds the angle of shooting for the player

    // Input-Updated Values
    [HideInInspector] public Vector2 movementInput; // Vector2 for movement
    [HideInInspector] public bool shootInput;       // A boolean that is true when shooting button is held down; false otherwise

    // Local Variables
    private PlayerControls playerControls;
    private Coroutine modifyAngleOfAimRoutine;
    private Coroutine playerShootingRoutine;
    private float signAngleMult = 0;
    private bool leftBoundCheck;
    private bool rightBoundCheck;
    bool doChangeShootingAngle;

    #region STATIC INPUT EVENTS
    // Ian: You could possibly suscribe to input events within their respective scripts
    // But I wanted to have everything remotely related to input management here

    /// <summary> Player's Weapon Shooting event </summary>
    public static event System.Action OnShootWeapon; // Action List

    /// <summary> Player's Switch to next Weapon event </summary>
    public static event System.Action OnSwitchToNextWeapon; // Action List

    /// <summary> Player's Switch to previous Weapon event </summary>
    public static event System.Action OnSwitchToPreviousWeapon; // Action List

    /// <summary> Player's Rotate Aim Event </summary>
    public static event System.Action OnRotateAim; // Action List

    #endregion

    private void Awake()    
    {
        // Handle Singleton
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
        // IAN: I migrated this code into the angle change handler, not sure if you will like that Ryan
        
        /*

        // Keep delta angle in range of the maxShootAngle
        leftBoundCheck = (signAngleMult > 0) && (shootAngleInput < maxShootAngle); // False if at limit of left
        rightBoundCheck = (signAngleMult < 0) && (shootAngleInput > -maxShootAngle); // False if at limit of right
        doChangeShootingAngle = leftBoundCheck || rightBoundCheck;

        if (doChangeShootingAngle)
        {
            // Modify the shooting angle of the player
            shootAngleInput += signAngleMult * changeShootAngSpeed * Time.deltaTime;
        }

        */
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
            
            // CHECK IAN: I added these new controls, but im not sure if I did them correctly
            playerControls.ShipControls.SwitchNextWeapon.performed += i => { OnSwitchToNextWeapon?.Invoke(); };
            playerControls.ShipControls.SwitchPreviousWeapon.performed += i => { OnSwitchToPreviousWeapon?.Invoke(); };
            // END OF ADDED CODE -------------------------------

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
        {
            shootInput = true; // Whenever the shooting button is pressed = true

            // CHECK IAN: Coroutine handling of shooting
            HandleShootingRoutine(buttonHeld: true);
            // END OF ADDED CODE --------------------------------------
        }
        else
        {
            shootInput = false;     // Whenever the shooting button is released = false

            // CHECK IAN: Coroutine handling of shooting
            HandleShootingRoutine(buttonHeld: false);
            // END OF ADDED CODE --------------------------------------
        }

        if (debug) Debug.Log(shootInput);
    }

    private void HandleShootAngleInput(InputAction.CallbackContext context, bool isRight)
    {
        if (context.performed)
        {
            // Code to be fired when the player angles the turret
            if (isRight)       // angle left button was pressed
            {
                signAngleMult = -1f;
            }
            else                // angle right button was pressed
            {
                signAngleMult = 1f;
            }

            // IAN: Coroutine Handling code
            HandleAngleRoutine(buttonHeld: true);
            // END OF NEW CODE ----------------------------
        }
        else
        {
            signAngleMult = 0;

            // IAN: Coroutine Handling code
            HandleAngleRoutine(buttonHeld: false);
            // END OF NEW CODE ----------------------------
        }

    }

    // IAN: I added these coroutines To avoid using update on the ShipController.cs ------------------------

    // Handles the coroutine related to shooting input
    private void HandleShootingRoutine(bool buttonHeld)
    {
        if (buttonHeld)
        {
            // Start shooting while the button is held
            if (playerShootingRoutine == null)
            {
                playerShootingRoutine = StartCoroutine(PlayerShooting());
            }
        }
        else
        {
            // Stop shooting after button release
            if (playerShootingRoutine != null)
            {
                StopCoroutine(playerShootingRoutine);
                playerShootingRoutine = null;
            }
        }
    }

    // Handles the coroutine related to changing the shooting angle
    private void HandleAngleRoutine(bool buttonHeld)
    {
        if (buttonHeld)
        {
            // Start changing the angle of aim while the button is held
            if (modifyAngleOfAimRoutine == null)
            {
                modifyAngleOfAimRoutine = StartCoroutine(ModifyAngleOfAim());
            }
        }
        else
        {
            // Stop changing the angle of shooting
            if (modifyAngleOfAimRoutine != null)
            {
                StopCoroutine(modifyAngleOfAimRoutine);
                modifyAngleOfAimRoutine = null;
            }
        }
    }


    // Coroutines that modifies the angle of shooting while key is pressed
    private IEnumerator ModifyAngleOfAim()
    {
        // This Coroutine will be stopped by the Angle Handler
        while (true)
        {
            // IAN: Here's the code I migrated from the update function
            leftBoundCheck = (signAngleMult > 0) && (shootAngleInput < maxShootAngle); // False if at limit of left
            rightBoundCheck = (signAngleMult < 0) && (shootAngleInput > -maxShootAngle); // False if at limit of right
            doChangeShootingAngle = leftBoundCheck || rightBoundCheck;

            if (doChangeShootingAngle)
            {
                // Modify the shooting angle of the player
                shootAngleInput += signAngleMult * changeShootAngSpeed * Time.deltaTime;
                OnRotateAim?.Invoke(); // Calls the function responsable for rotating the aim inside [ShipController.cs]
            }
            yield return null;
        }
    }

    // Coroutines that shoots continuously while key is pressed
    private IEnumerator PlayerShooting()
    {
        // This Coroutine will be stopped by the Shoot Input Handler
        while (true)
        {
            OnShootWeapon?.Invoke();
            yield return null;
        }
    }
}
