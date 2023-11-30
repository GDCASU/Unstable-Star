using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Weapon utilities. TODO: should one day be migrated to PlayerController </summary>
public class TempPlayerGunInput : MonoBehaviour
{
    [Header("Anchor Point")]
    [SerializeField] private GameObject LaserSightAnchor;

    //Local Variables
    private Player playerScript;
    private int TEMPLOCK; //Add weapon locker
    private float xVal;
    private float yVal;
    private float zVal;

    private void Start()
    {
        playerScript = GetComponent<Player>();
        TEMPLOCK = 0;
    }

    // Check for inputs:
    // > Q switches to the next weapon
    // > Left click shoots
    void Update()
    {
        // HACK: Dont do inputs like this, its bad, bad bad bad.
        // Its a temporary solution right now

        //Store the values
        xVal = this.gameObject.transform.rotation.eulerAngles.x;
        yVal = this.gameObject.transform.rotation.eulerAngles.y;
        zVal = this.gameObject.transform.rotation.eulerAngles.z;
        
        //Add the offset provided by the controller
        zVal += PlayerInput.instance.shootAngleInput;

        //Update Laser Sight rotation
        LaserSightAnchor.transform.rotation = Quaternion.Euler(xVal, yVal, zVal);

        // Input system for shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerScript.ShootWeapon();
        }

        // Switching guns
        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerScript.SwitchToNextWeapon();
        }

        //TESTING: ADD NEW WEAPON TO PLAYER ARSENAL
        if ( Input.GetKeyDown(KeyCode.L) && (TEMPLOCK < 1) )
        {
            Pistol lethalPistol = new Pistol(60f, 7, "Lethal Pistol");
            playerScript.AddNewWeapon(lethalPistol);
            Debug.Log("ADDED TEST WEAPON TO ARSENAL");
            TEMPLOCK++; //Stops this from being used more than once
        }
    }
}
