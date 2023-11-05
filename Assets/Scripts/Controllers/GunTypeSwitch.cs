using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Weapon switch and fire utility. TODO: should one day be migrated to PlayerController </summary>
public class GunTypeSwitch : MonoBehaviour
{
    //Local Variables
    private Shoot playerShootScript;
    private int currWeaponIndex;
    private int TEMPLOCK; //Add weapon locker

    private void Awake()
    {
        playerShootScript = GetComponent<Shoot>();
        currWeaponIndex = 0;
        TEMPLOCK = 0;
    }

    // Check for inputs:
    // > Q switches to the next weapon
    // > Left click shoots
    void Update()
    {
        // HACK: I would suggest maybe a switch statement or dictionary for checking these
        // In the future, but this suffices since I (Ian F.) am not working on the player
        // Controller
        
        // Input system for shooting
        if (Input.GetMouseButtonDown(0))
        {
            playerShootScript.ShootCurrentWeapon();
        }

        // Switching guns
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Increase weapon index
            ++currWeaponIndex;
            if (currWeaponIndex >= WeaponData.Instance.PlayerWeaponList.Count) 
            { 
                //Return to start
                currWeaponIndex = 0; 
            }
            //Set the weapon on the player
            playerShootScript.SetWeaponFire(WeaponData.Instance.PlayerWeaponList[currWeaponIndex]);
        }

        //TESTING: ADD NEW WEAPON TO PLAYER ARSENAL
        if ( Input.GetKeyDown(KeyCode.L) && (TEMPLOCK < 1) )
        {
            Buckshot fastBuckshot = new Buckshot(60f, 2, "Fast Buckshot");
            playerShootScript.AddWeaponToShoot(fastBuckshot, 3);
            Debug.Log("ADDED TEST WEAPON TO ARSENAL");
            TEMPLOCK++; //Stops this from being used more than once
        }
    }
}
