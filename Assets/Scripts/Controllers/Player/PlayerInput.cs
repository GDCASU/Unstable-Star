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
    [HideInInspector] public Vector2 movementInput;         // Vector2 for movement
    [HideInInspector] public Vector2 cursorAimScreenPoint;  // Vector2 for cursor point on screen
    [HideInInspector] public Vector3 cursorAimWorldPoint;   // Vector2 for cursor point in world
    [HideInInspector] public bool isShootHeld;              // A boolean that is true when shooting button is held down; false otherwise
    [HideInInspector] public bool isWeaponSwitching;        // Boolean that denotes that the weapon switch animation hasnt finished yet

    // Local Variables

    private PlayerControls playerControls;
    private Coroutine modifyAngleOfAimRoutine;
    private Coroutine playerShootingRoutine;

    private float signAngleMult = 0;
    private bool leftBoundCheck;
    private bool rightBoundCheck;
    private bool doChangeShootingAngle;

    #region SHIP CONTROL EVENTS

    public static event System.Action OnMove;

    /// <summary> Player's Weapon Shooting event </summary>
    public static event System.Action OnShoot;

    public static event System.Action AngleLeft;
    public static event System.Action AngleRight;

    /// <summary> Player's Switch to next Weapon event </summary>
    public static event System.Action OnSwitchToNextWeapon;

    /// <summary> Player's Switch to next ability event </summary>
    public static event System.Action OnSwitchToNextAbility;

    /// <summary> Player's Ability Use event </summary>
    public static event System.Action OnUseAbility;

    /// <summary> Player's Hold Focus Speed </summary>
    public static event System.Action<bool> OnFocusSpeedHeld;

    /// <summary> Player's aim event </summary>
    public static event System.Action OnRotateAim;

    /// <summary> Change the Dialogue to next </summary>
    public static event System.Action OnChangeDialogue;

    /// <summary> Skip the entire dialogue </summary>
    public static event System.Action OnSkipDialogue;

    /// <summary> Pause the game </summary>
    public static event System.Action OnPauseGame;

    /// <summary>
    /// Binds all of the basic ship control functions to their respective events.
    /// </summary>
    void BindBasicShipControlEvents()
    {
        // Subscribe to input events
        playerControls.ShipControls.Move.performed += i => HandleMovementInput(i);      // perfomed event fires when the button is pressed

        playerControls.ShipControls.Shoot.performed += i => HandleShootingInput(i);     // perfomed event fires when the button is pressed
        playerControls.ShipControls.Shoot.canceled += i => HandleShootingInput(i);      // canceled event fires when the button is released

        playerControls.ShipControls.AngleLeft.performed += i => HandleShootAngleInput(i, false);   // perfomed event fires when the button is pressed 
        playerControls.ShipControls.AngleRight.performed += i => HandleShootAngleInput(i, true);   // perfomed event fires when the button is pressed
        playerControls.ShipControls.AngleLeft.canceled += i => HandleShootAngleInput(i, false);     // perfomed event fires when the button is released
        playerControls.ShipControls.AngleRight.canceled += i => HandleShootAngleInput(i, true);   // perfomed event fires when the button is released

        playerControls.ShipControls.SwitchNextWeapon.started += i => HandleOnWeaponSwitch();

        playerControls.ShipControls.UseAbility.performed += i => OnUseAbility?.Invoke();

        playerControls.ShipControls.FocusSpeed.performed += i => OnFocusSpeedHeld?.Invoke(true);
        playerControls.ShipControls.FocusSpeed.canceled += i => OnFocusSpeedHeld?.Invoke(false);

        playerControls.ShipControls.ChangeDialogue.performed += i => OnChangeDialogue?.Invoke();
        //playerControls.ShipControls.SkipDialogue.performed += i => OnSkipDialogue?.Invoke();

        playerControls.ShipControls.PauseGame.started += i => OnPauseGame?.Invoke();
    }

    #endregion

    #region UI EVENTS

    public static event System.Action<Vector2> OnMenuNavigate;
    public static event System.Action<Vector2> OnMousePoint;
    public static event System.Action OnSubmit;
    public static event System.Action OnClick;
    public static event System.Action OnCancel;

    void BindBasicUiEvents()
    {
        playerControls.UI.Navigate.performed += i => OnMenuNavigate?.Invoke(i.ReadValue<Vector2>());
        playerControls.UI.Point.performed += i => OnMousePoint?.Invoke(i.ReadValue<Vector2>());

        playerControls.UI.Cancel.started += i => OnCancel?.Invoke();
        playerControls.UI.Submit.started += i => OnSubmit?.Invoke();
    }

    #endregion

    #region Unity Events

    private void Awake()
    {
        // Handle Singleton
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Control handling
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            BindBasicShipControlEvents();
            BindBasicUiEvents();
        }

        playerControls.Enable();
        ActivateShipControls();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    #endregion

    #region Changing input map

    /// <summary>
    /// Disables current input map and enables opposite. Current input maps: ShipControls, UI.
    /// </summary>
    public void ToggleInputMap()
    {
        if (playerControls.ShipControls.enabled) ActivateUiControls();
        else ActivateShipControls();
    }

    /// <summary>
    /// Disables all input maps and ensures ShipControls mapping is enabled.
    /// </summary>
    public void ActivateShipControls()
    {
        if (debug) Debug.Log("PlayerInput::ActivateShipControls");

        playerControls.UI.Disable();
        playerControls.ShipControls.Enable();
    }

    /// <summary>
    /// Disables all input maps and ensures UI mapping is enabled.
    /// </summary>
    public void ActivateUiControls()
    {
        if (debug) Debug.Log("PlayerInput::ActivateUiControls");

        playerControls.ShipControls.Disable();
        playerControls.UI.Enable();
    }

    #endregion

    #region Event Handlers

    // Toggle the player controls with this method from any script
    public void ToggleControls(bool toggle)
    {
        if (playerControls == null) 
            return;

        if (toggle)
        {
            playerControls.Enable();
            ActivateShipControls();
        }
        else
            playerControls.Disable();

        // Reset movement vector to 0
        movementInput = Vector2.zero;
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
                signAngleMult = -1f;
            else                // angle right button was pressed
                signAngleMult = 1f;

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
        if (this == null) return;

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
        if (this == null) return;

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

    // Handles the weapon switching of the player, does not go through if animation is playing
    private void HandleOnWeaponSwitch()
    {
        // Ignore if weapon switch animation is still running
        if (isWeaponSwitching) return;

        // Else, do raise event
        OnSwitchToNextWeapon?.Invoke();
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
            OnShoot?.Invoke();
            yield return null;
        }
    }

    #endregion
}
