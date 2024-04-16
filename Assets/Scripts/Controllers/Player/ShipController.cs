using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Player Specific")]
    [SerializeField] private float speed = 60.0f;
    [SerializeField] private GameObject playerCopyPrefab;

    [Header("Anchor Point")]
    [SerializeField] private GameObject LaserSightAnchor;

    // Player Data
    private Player playerScript;
    private readonly float verticalOffset = 2; // Offset used for upper screen boundary

    // Local Variables
    private Animator animComponent;
    private GameObject currentCopyPlayer;
    private Coroutine checkIfOffScreen;
    private Vector3 viewPos;
    private Vector3 translationVector;
    private Vector2 movementVector;
    private Vector2 screenBounds;
    private int TEMPLOCK; // Add weapon locker
    private bool playerCopyExists = false;

    // Add functions so they listen to their respective events, as well as calculate screen size
    private void Awake()
    {
        // Input Events
        PlayerInput.OnSwitchToNextWeapon += DoSwitchToNextWeapon;
        PlayerInput.OnSwitchToNextAbility += DoSwitchToNextAbility;
        PlayerInput.OnRotateAim += RotateAim;
        PlayerInput.OnShootWeapon += ShootPlayerWeapon;
        PlayerInput.OnUseAbility += UsePlayerAbility;

        // Get the boundary limits of the play space
        // FIXME: Is this the best way to do this?
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
            Camera.main.transform.position.z));
    }

    // Remove Input Events if object is destroyed
    private void OnDestroy()
    {
        PlayerInput.OnSwitchToNextWeapon -= DoSwitchToNextWeapon;
        PlayerInput.OnSwitchToNextAbility -= DoSwitchToNextAbility;
        PlayerInput.OnRotateAim -= RotateAim;
        PlayerInput.OnShootWeapon -= ShootPlayerWeapon;
        PlayerInput.OnUseAbility -= UsePlayerAbility;
    }

    private void Start()
    {
        // Get components
        playerScript = GetComponent<Player>();
        animComponent = GetComponent<Animator>();
    }

    void Update()
    {
        // Update position of player
        movePlayer();
    }

    void movePlayer()
    {
        // Move player according to input
        movementVector = PlayerInput.instance.movementInput;
        Debug.Log(PlayerInput.instance.movementInput);
        translationVector = speed * Time.deltaTime * movementVector;
        transform.Translate(translationVector);

        // Handle Movement animation
        animComponent.SetFloat("moveDirection", movementVector.x);
        if (movementVector.x != 0)
            animComponent.SetBool("isMoving", true);
        else
            animComponent.SetBool("isMoving", false);


        // Copy Movement over to copy player if they exist
        if (currentCopyPlayer != null) { currentCopyPlayer.transform.Translate(translationVector); }

        // Stop the player if at the top and bottom of the screen
        viewPos = this.transform.position;
        float newY = Mathf.Clamp(viewPos.y, screenBounds.y + 2 * verticalOffset, (screenBounds.y * -1) - verticalOffset);
        viewPos.y = newY;
        this.transform.position = viewPos;

        // Copy Y coordinate clamp over to copy player if they exist
        if (currentCopyPlayer != null)
        { 
            Vector3 copyPos = currentCopyPlayer.transform.position;
            copyPos.y = newY;
            currentCopyPlayer.transform.position = copyPos;
        }
    }

    private void RotateAim()
    {
        //Store the values (There should be a more efficient way to do this, but oh well)
        float xVal = this.gameObject.transform.rotation.eulerAngles.x;
        float yVal = this.gameObject.transform.rotation.eulerAngles.y;
        float zVal = this.gameObject.transform.rotation.eulerAngles.z;

        //Add the offset provided by the controller
        zVal += PlayerInput.instance.shootAngleInput;

        //Update Laser Sight rotation
        LaserSightAnchor.transform.rotation = Quaternion.Euler(xVal, yVal, zVal);
    }

    // Detect Player approaching the edge of the screen
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Screen Bounds") && !playerCopyExists)
        {
            // Create the copy player at the other side of the screen
            Vector3 pos = this.transform.position;
            Quaternion rotation = this.transform.rotation;
            currentCopyPlayer = Instantiate(playerCopyPrefab, pos, rotation);
            pos = currentCopyPlayer.transform.position;
            pos.x *= -1;
            currentCopyPlayer.transform.position = pos; // Agh unity why wont you let me edit x directly :c
            playerCopyExists = true;

            // Start Checking if the copy has went off screen
            checkIfOffScreen = StartCoroutine(CheckWhoIsCloser());
        }
    }

    // Detect when the copy player leaves the collider boxes and delete it
    private void OnTriggerExit(Collider other)
    {
        if (currentCopyPlayer != null)
        {
            StopCoroutine(checkIfOffScreen);
            Destroy(currentCopyPlayer);
            playerCopyExists = false;
        }
    }

    private IEnumerator CheckWhoIsCloser()
    {
        // The lower absolute value of x between the two objects should be the player
        float xPosPlayer;
        float xPosCopy;

        // Run until the coroutine is stopped
        while (true)
        {
            xPosPlayer = Mathf.Abs(this.gameObject.transform.position.x);
            xPosCopy = Mathf.Abs(currentCopyPlayer.transform.position.x);

            // If the copy player is closer to the center of the screen, swap the player with the copy
            if (xPosCopy < xPosPlayer)
            {
                Vector3 tempPos = this.gameObject.transform.position;
                this.gameObject.transform.position = currentCopyPlayer.transform.position;
                currentCopyPlayer.transform.position = tempPos;
            }

            // Wait a frame
            yield return null;
        }
   
    }

    // FUNCTIONS HERE ARE SUSCRIBED TO EVENT DATA RAISERS
    private void ShootPlayerWeapon()
    {
        playerScript.ShootWeapon();
    }

    private void UsePlayerAbility()
    {
        playerScript.UseAbility();
    }

    private void DoSwitchToNextWeapon()
    {
        playerScript.SwitchToNextWeapon();
    }

    private void DoSwitchToNextAbility()
    {
        playerScript.SwitchToNextAbility();
    }
}