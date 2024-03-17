using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract Parent class for scriptable weapon interpretation within the arsenal and other systems
public abstract class ScriptableWeapon : ScriptableObject
{
    /// <summary> Function to build the weapon object and return it </summary>
    public abstract Weapon GetWeaponObject();
}

/// <summary> Object that will hold the players weapon array data </summary>
public class WeaponArsenal : MonoBehaviour
{
    // Singleton
    public static WeaponArsenal instance;

    // List that will hold the weapons of the player, and a string array for the inspector
    [SerializeField] private List<string> weaponArsenalStrings = new();
    public List<Weapon> weaponArsenal = new();

    // Settings
    [SerializeField] private int maxWeaponCount;
    [SerializeField] private int currWeaponIndex = -1; // Will be -1 whenever there are no weapons
    private readonly Weapon nullWeapon = new GeneralWeapon("No Weapons"); // Helper null reference

    // Current weapon
    private Weapon currWeapon = new GeneralWeapon("No Weapons"); // Starts as null

    // Debbuging
    [SerializeField] private bool doDebugLog;
    [SerializeField] private bool loadDefaultArsenal;

    // All Scripted Weapons that have been designed so far
    public ScriptableWeapon PlayerPistol;
    public ScriptableWeapon PlayerBirdshot;
    public ScriptableWeapon PlayerBuckshot;
    public ScriptableWeapon PlayerLaserWep;
    public ScriptableWeapon PlayerGatlingGun;

    private void Awake()
    {
        // Handle Singleton
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        // Give the player the default arsenal if checked
        if (loadDefaultArsenal && weaponArsenal.Count <= 0)
        {
            AddWeaponToArsenal(PlayerPistol.GetWeaponObject());
            AddWeaponToArsenal(PlayerBirdshot.GetWeaponObject());
            AddWeaponToArsenal(PlayerBuckshot.GetWeaponObject());
            AddWeaponToArsenal(PlayerLaserWep.GetWeaponObject());
            AddWeaponToArsenal(PlayerGatlingGun.GetWeaponObject());
        }
    }

    // --------------------------------------------------------------------
    //    Helper functions for modifying/checking the Weapon Arsenal
    // --------------------------------------------------------------------

    #region SWITCHING WEAPONS

    public void SwitchToNextWeapon()
    {
        // If the player has no weapons, then dont attempt a switch
        if (weaponArsenal.Count <= 0)
        {
            currWeapon = nullWeapon;
            currWeaponIndex = -1;
            return;
        }

        // Else, switch to next
        ++currWeaponIndex;

        //Check if we arent at the end of the list 
        if (currWeaponIndex > weaponArsenal.Count - 1)
        {
            //Return to start
            currWeapon = weaponArsenal[0];
            currWeaponIndex = 0;
            return;
        }
        //Else, switch to next in list
        currWeapon = weaponArsenal[currWeaponIndex];
    }

    /// <summary> Switches to the previous weapon in the arsenal </summary>
    public void SwitchToPreviousWeapon()
    {
        // If the player has no weapons, then dont attempt a switch
        if (weaponArsenal.Count <= 0)
        {
            currWeapon = nullWeapon;
            currWeaponIndex = -1;
            return;
        }
        
        // Else, go back to previous weapon
        --currWeaponIndex;

        //Check if we arent at the start of the list 
        if (currWeaponIndex < 0)
        {
            //Return to end
            int lastIndex = weaponArsenal.Count - 1;
            currWeapon = weaponArsenal[lastIndex];
            currWeaponIndex = lastIndex;
            return;
        }

        //Else, switch to previous weapon in list
        currWeapon = weaponArsenal[currWeaponIndex];
    }

    #endregion

    #region ADDING WEAPONS

    /// <summary> Add a Weapon, returns true if successful </summary>
    public bool AddWeaponToArsenal(Weapon inputWeapon)
    {
        // Check if the arsenal was empty
        if (weaponArsenal.Count <= 0)
        {
            // If empty, add weapon and set it as current
            weaponArsenal.Add(inputWeapon);
            weaponArsenalStrings.Add(inputWeapon.sName);
            currWeapon = weaponArsenal[0];
            currWeaponIndex = 0;
            return true;
        }
        
        // Check if the arsenal is not full
        if (weaponArsenal.Count >= maxWeaponCount)
        {
            // Its at is max, do not add weapon
            if (doDebugLog) Debug.Log("WEAPON ARSENAL ITS ALREADY AT ITS MAX!");
            return false;
        }

        // Else, add this new weapon to arsenal
        weaponArsenal.Add(inputWeapon);
        weaponArsenalStrings.Add(inputWeapon.sName);
        return true;
    }

    #endregion

    #region REMOVING WEAPONS

    /// <summary> Remove a weapon by Object, returns true if successful </summary>
    public bool RemoveWeaponByObject(Weapon targetWeapon)
    {
        string targetName = targetWeapon.sName;
        string currName;

        // Loop through array
        for (int i = 0; i < weaponArsenal.Count; i++)
        {
            currName = weaponArsenal[i].sName;
            if (currName.Equals(targetName))
            {
                // Remove
                weaponArsenal.RemoveAt(i);
                weaponArsenalStrings.RemoveAt(i);

                // If this weapon was equipped, switch to previous
                if (i == currWeaponIndex) SwitchToPreviousWeapon();

                if (doDebugLog) Debug.Log("Successfully removed weapon " + targetName);
                return true;
            }
        }
        // If debug is enabled, print to console that it wasnt found
        if (doDebugLog) Debug.Log("Did not find weapon " + targetName + " In the Arsenal");
        return false;
    }

    /// <summary> Remove a weapon by index, returns true if successful </summary>
    public bool RemoveWeaponByIndex(int index)
    {
        // Check if the index provided is within the array range
        if (index < 0 || index >= weaponArsenal.Count)
        {
            string msg = "<color=red>ERROR! INDEX PROVIDED IS OUTSIDE OF RANGE, OR ARRAY IS EMPTY!\n</color>";
            msg += "<color=yellow>Error thrown on WeaponArsenal at \"RemoveWeaponByIndex\"</color>";
            Debug.Log(msg);
            return false;
        }

        // Else it was a valid index, remove the weapon at that index
        if (doDebugLog)
        {
            string msg = "Successfully removed weapon " + weaponArsenal[index].sName;
            msg += "\nLocated at index = " + index;
            Debug.Log(msg);
        }
        weaponArsenal.RemoveAt(index);
        weaponArsenalStrings.RemoveAt(index);
        // If this weapon was equipped, switch to previous
        if (index == currWeaponIndex) SwitchToPreviousWeapon();
        return true;
    }

    /// <summary> Remove weapon by name, case insensitive, returns true if successful </summary>
    public bool RemoveWeaponByName(string targetName)
    {
        string currName;
        bool isEqual;

        // Loop through array
        for (int i = 0; i < weaponArsenal.Count; i++)
        {
            // Check if they match
            currName = weaponArsenal[i].sName;
            isEqual = string.Equals(targetName, currName, StringComparison.CurrentCultureIgnoreCase);
            if (isEqual)
            {
                // They did match, remove it
                weaponArsenal.RemoveAt(i);
                weaponArsenalStrings.RemoveAt(i);

                // If this weapon was equipped, switch to previous
                if (i == currWeaponIndex) SwitchToPreviousWeapon();

                if (doDebugLog) Debug.Log("Successfully removed weapon " + targetName);
                return true;
            }
        }
        // If debug is enabled, print to console that it wasnt found
        if (doDebugLog) Debug.Log("Did not find weapon " + targetName + " In the Arsenal");
        return false;
    }

    /// <summary> Clear the Weapon Array </summary>
    public void ClearWeaponArsenal()
    {
        weaponArsenal.Clear();
        weaponArsenalStrings.Clear();
        currWeapon = nullWeapon;
        currWeaponIndex = -1;
        if (doDebugLog) Debug.Log("WEAPON ARSENAL CLEARED!");
    }

    #endregion

    #region GETTERS AND SETTERS

    /// <summary>
    /// This function will be important if we want the player to always start with the weapon
    /// that is at index [0] of the array for every scene/level
    /// </summary>
    public void SetCurrentWeaponToFirst()
    {
        // Check if the weapon list isnt empty, if so, reference null weapon
        if (weaponArsenal.Count <= 0)
        {
            currWeapon = nullWeapon;
            currWeaponIndex = -1;
            return;
        }
        // Else, do as normal
        currWeapon = weaponArsenal[0];
        currWeaponIndex = 0;
    }

    /// <summary> Check if the weapon arsenal is empty, returns true if so </summary>
    public bool IsWeaponArsenalEmpty()
    {
        if (weaponArsenal.Count <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary> Returns current weapon </summary>
    public Weapon GetCurrentWeapon()
    {
        return currWeapon;
    }

    /// <summary> Get array weapon by index </summary>
    public Weapon GetWeaponByIndex(int index)
    {
        // Check if the index provided is within the array range
        if (index < 0 || index >= weaponArsenal.Count)
        {
            string msg = "<color=red>ERROR! INDEX PROVIDED IS OUTSIDE OF RANGE, OR ARRAY IS EMPTY!\n</color>";
            msg += "<color=yellow>Error thrown on WeaponArsenal at \"GetWeaponByIndex\"</color>";
            Debug.Log(msg);
            return null;
        }
        // It was valid, return weapon
        return weaponArsenal[index];
    }

    // Get the current list size of the weapon arsenal
    public int GetWeaponListCount()
    {
        return weaponArsenal.Count;
    }

    // Get the current max size of arsenal (set in inspector)
    public int GetMaxArsenalCount()
    {
        return maxWeaponCount;
    }

    #endregion
}