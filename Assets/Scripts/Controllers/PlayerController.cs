using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 60.0f;
    [SerializeField] private GameObject playerCopyPrefab;
    private GameObject currentCopyPlayer;
    private bool playerCopyExists = false;
    private Vector2 movementVector;
    private Vector3 translationVector;
    private Coroutine checkIfOffScreen;

    // Local Variables
    private Vector2 screenBounds;
    private readonly float playerModelHeight = 2;
    private Vector3 viewPos;

    private void Awake()
    {
        // Get the boundary points of the play space
        // FIXME: Is this the best way to do this?
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
            Camera.main.transform.position.z));
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
        translationVector = speed * Time.deltaTime * movementVector;
        transform.Translate(translationVector);

        // Copy Movement over to copy player if they exist
        if (currentCopyPlayer != null) { currentCopyPlayer.transform.Translate(translationVector); }

        // Stop him if at the top and bottom of the screen
        viewPos = this.transform.position;
        float newY = Mathf.Clamp(viewPos.y, screenBounds.y + 2 * playerModelHeight, (screenBounds.y * -1) - playerModelHeight);
        viewPos.y = newY;
        this.transform.position = viewPos;

        // Copy Position over to copy player if they exist
        if (currentCopyPlayer != null)
        { 
            Vector3 copyPos = currentCopyPlayer.transform.position;
            copyPos.y = newY;
            currentCopyPlayer.transform.position = copyPos;
        }
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
}