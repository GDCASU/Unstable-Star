using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public TestBaseScript testbasescript;
    public static PlayerInput instance;     // Singleton Instance

    [Header("Input Settings")]
    public float changeShootAngSpeed = 30.0f;
    public float maxShootAngle = 30.0f;

    [Header("Debugging")]
    [SerializeField] private bool debug = false;
    public float shootAngleInput; // holds the angle of shooting for the player

    // Input-Updated Values
    [HideInInspector] public Vector2 movementInput; // Vector2 for movement
    [HideInInspector] public bool isShootHeld;       // A boolean that is true when shooting button is held down; false otherwise

    // Local Variables
    /* Shantanu messing arund*/
    private InputAction.CallbackContext cxt;
    private PlayerControls playerControls;
    private Coroutine modifyAngleOfAimRoutine;
    private Coroutine playerShootingRoutine;
    private float signAngleMult = 0;
    private bool leftBoundCheck;
    private bool rightBoundCheck;
    private bool doChangeShootingAngle;

    #region STATIC INPUT EVENTS

    /// <summary> Player's Weapon Shooting event </summary>
    public static event System.Action OnShootWeapon; // Action List

    /// <summary> Player's Ability Use event </summary>
    public static event System.Action OnUseAbility; // Action List

    /// <summary> Player's Switch to next Weapon event </summary>
    public static event System.Action OnSwitchToNextWeapon; // Action List

    /// <summary> Player's Switch to next ability event </summary>
    public static event System.Action OnSwitchToNextAbility; // Action List

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
            playerControls.ShipControls.UseAbility.performed += i => { OnUseAbility?.Invoke(); };

            playerControls.ShipControls.AngleLeft.performed += i => HandleShootAngleInput(i, false);   // perfomed event fires when the button is pressed 
            playerControls.ShipControls.AngleRight.performed += i => HandleShootAngleInput(i, true);   // perfomed event fires when the button is pressed
            playerControls.ShipControls.AngleLeft.canceled += i => HandleShootAngleInput(i, false);     // perfomed event fires when the button is released
            playerControls.ShipControls.AngleRight.canceled += i => HandleShootAngleInput(i, true);   // perfomed event fires when the button is released
            
            playerControls.ShipControls.SwitchNextWeapon.performed += i => { OnSwitchToNextWeapon?.Invoke(); };
            /*Shantanu is Testing out input system*/ playerControls.ShipControls.SwitchNextWeapon.performed += i => testbasescript.Explosion(i);
            playerControls.ShipControls.SwitchNextAbility.performed += i => { OnSwitchToNextAbility?.Invoke(); };
        }

        playerControls.Enable();

    }
    /*Shantanu is Testing out input system*/ private void Explosion(InputAction.CallbackContext context)
    {
        Debug.Log("Explosion");
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
            isShootHeld = true; // Whenever the shooting button is pressed = true

            HandleShootingRoutine(buttonHeld: true);
        }
        else
        {
            isShootHeld = false;     // Whenever the shooting button is released = false

            HandleShootingRoutine(buttonHeld: false);
        }

        if (debug) Debug.Log(isShootHeld);
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

            // Coroutine Handling
            HandleAngleRoutine(buttonHeld: true);
        }
        else
        {
            signAngleMult = 0;

            // Coroutine Handling
            HandleAngleRoutine(buttonHeld: false);
        }

    }

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
            // Bound checking
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
