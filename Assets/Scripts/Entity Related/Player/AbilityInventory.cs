using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract Parent class for scriptable abilities interpretation within the inventory and other systems
public abstract class ScriptableAbility : ScriptableObject
{
    /// <summary> Helper function to construct an Ability object from stored data </summary>
    public abstract Ability GetAbilityObject();
}

/// <summary> Object that will hold the players ability in an array </summary>
public class AbilityInventory : MonoBehaviour
{
    // Singleton
    public static AbilityInventory instance;

    // List that will hold the abilities of the player, and a string array for the inspector
    [SerializeField] private List<string> abilityInventoryStrings = new();
    private List<Ability> abilityInventory = new();

    // Settings
    private readonly int maxAbilityCount = 1; // Limited by UI
    [SerializeField] private int currAbilityIndex = -1; // Will be -1 whenever there are no abilities equipped

    // Debbuging
    private readonly Ability nullAbility = new GeneralAbility("No Ability"); // Helper null reference
    private Ability currAbility = new GeneralAbility("No Ability"); // Starts as null
    
    [SerializeField] private bool doDebugLog;
    [SerializeField] private bool loadInspectorAbility;

    // Target ability to use in gameplay, set in the inspector window for debugging specific abilities
    [SerializeField] private ScriptableAbility inputAbility;

    private void Awake()
    {
        // Handle Singleton
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        // Give the player the ability set in the inspector
        if (loadInspectorAbility && abilityInventory.Count <= 0)
        {
            ClearAbilityInventory();
            AddAbilityToInventory(inputAbility.GetAbilityObject());
        }
    }

    // --------------------------------------------------------------------
    //    Helper functions for modifying/checking the Ability Inventory
    // --------------------------------------------------------------------

    #region SWITCHING ABILITIES

    public void SwitchToNextAbility()
    {
        // If the player has no abilites, then dont attempt a switch
        if (abilityInventory.Count <= 0)
        {
            currAbility = nullAbility;
            currAbilityIndex = -1;
            return;
        }

        // Else, switch to next
        ++currAbilityIndex;

        //Check if we arent at the end of the list 
        if (currAbilityIndex > abilityInventory.Count - 1)
        {
            //Return to start
            currAbility = abilityInventory[0];
            currAbilityIndex = 0;
            return;
        }
        //Else, switch to next in list
        currAbility = abilityInventory[currAbilityIndex];
    }

    /// <summary> Switches to the previous ability in the inventory </summary>
    public void SwitchToPreviousAbility()
    {
        // If the player has no abilities, then dont attempt a switch
        if (abilityInventory.Count <= 0)
        {
            currAbility = nullAbility;
            currAbilityIndex = -1;
            return;
        }

        // Else, go back to previous ability
        --currAbilityIndex;

        //Check if we arent at the start of the list 
        if (currAbilityIndex < 0)
        {
            //Return to end
            int lastIndex = abilityInventory.Count - 1;
            currAbility = abilityInventory[lastIndex];
            currAbilityIndex = lastIndex;
            return;
        }

        //Else, switch to previous ability in list
        currAbility = abilityInventory[currAbilityIndex];
    }

    #endregion

    #region ADDING ABILITIES

    /// <summary> Add an ability, returns true if successful </summary>
    public bool AddAbilityToInventory(Ability inputAbility)
    {
        // Check if the inventory was empty
        if (abilityInventory.Count <= 0)
        {
            // If empty, add ability and set it as current
            abilityInventory.Add(inputAbility);
            abilityInventoryStrings.Add(inputAbility.sName);
            currAbility = abilityInventory[0];
            currAbilityIndex = 0;
            return true;
        }

        // Check if the inventory is not full
        if (abilityInventory.Count >= maxAbilityCount)
        {
            // Its at is max, do not add ability
            if (doDebugLog) Debug.Log("ABILITY INVENTORY ITS ALREADY AT ITS MAX!");
            return false;
        }

        // Else, add this new ability to inventory
        abilityInventory.Add(inputAbility);
        abilityInventoryStrings.Add(inputAbility.sName);
        return true;
    }

    #endregion

    #region REMOVING ABILITIES

    /// <summary> Remove an ability by Object, returns true if successful </summary>
    public bool RemoveAbilityByObject(Ability targetAbility)
    {
        string targetName = targetAbility.sName;
        string currName;

        // Loop through array
        for (int i = 0; i < abilityInventory.Count; i++)
        {
            currName = abilityInventory[i].sName;
            if (currName.Equals(targetName))
            {
                // Remove
                abilityInventory.RemoveAt(i);
                abilityInventoryStrings.RemoveAt(i);

                // If this ability was equipped, switch to previous
                if (i == currAbilityIndex) SwitchToPreviousAbility();

                if (doDebugLog) Debug.Log("Successfully removed ability " + targetName);
                return true;
            }
        }
        // If debug is enabled, print to console that it wasnt found
        if (doDebugLog) Debug.Log("Did not find ability " + targetName + " In the Inventory");
        return false;
    }

    /// <summary> Remove an ability by index, returns true if successful </summary>
    public bool RemoveAbilityByIndex(int index)
    {
        // Check if the index provided is within the array range
        if (index < 0 || index >= abilityInventory.Count)
        {
            string msg = "<color=red>ERROR! INDEX PROVIDED IS OUTSIDE OF RANGE, OR ARRAY IS EMPTY!\n</color>";
            msg += "<color=yellow>Error thrown on AbilityInventory at \"RemoveAbilityByIndex\"</color>";
            Debug.Log(msg);
            return false;
        }

        // Else it was a valid index, remove the ability at that index
        if (doDebugLog)
        {
            string msg = "Successfully removed abilty " + abilityInventory[index].sName;
            msg += "\nLocated at index = " + index;
            Debug.Log(msg);
        }
        abilityInventory.RemoveAt(index);
        abilityInventoryStrings.RemoveAt(index);
        // If this ability was equipped, switch to previous
        if (index == currAbilityIndex) SwitchToPreviousAbility();
        return true;
    }

    /// <summary> Remove ability by name, case insensitive, returns true if successful </summary>
    public bool RemoveAbilityByName(string targetName)
    {
        string currName;
        bool isEqual;

        // Loop through array
        for (int i = 0; i < abilityInventory.Count; i++)
        {
            // Check if they match
            currName = abilityInventory[i].sName;
            isEqual = string.Equals(targetName, currName, StringComparison.CurrentCultureIgnoreCase);
            if (isEqual)
            {
                // They did match, remove it
                abilityInventory.RemoveAt(i);
                abilityInventoryStrings.RemoveAt(i);

                // If this ability was equipped, switch to previous
                if (i == currAbilityIndex) SwitchToPreviousAbility();

                if (doDebugLog) Debug.Log("Successfully removed ability " + targetName);
                return true;
            }
        }
        // If debug is enabled, print to console that it wasnt found
        if (doDebugLog) Debug.Log("Did not find ability " + targetName + " In the Inventory");
        return false;
    }

    /// <summary> Clear the Ability Inventory </summary>
    public void ClearAbilityInventory()
    {
        abilityInventory.Clear();
        abilityInventoryStrings.Clear();
        currAbility = nullAbility;
        currAbilityIndex = -1;
        if (doDebugLog) Debug.Log("ABILITY INVENTORY CLEARED!");
    }

    #endregion

    #region GETTERS AND SETTERS

    /// <summary>
    /// This function will be important if we want the player to always start with the ability
    /// that is at index [0] of the array for every scene/level
    /// </summary>
    public void SetCurrentAbilityToFirst()
    {
        // Check if the ability list isnt empty, if so, reference null ability
        if (abilityInventory.Count <= 0)
        {
            currAbility = nullAbility;
            currAbilityIndex = -1;
            return;
        }
        // Else, do as normal
        currAbility = abilityInventory[0];
        currAbilityIndex = 0;
    }

    /// <summary> Check if the ability inventory is empty, returns true if so </summary>
    public bool IsAbilityInventoryEmpty()
    {
        if (abilityInventory.Count <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary> Returns current ability </summary>
    public Ability GetCurrentAbility()
    {
        return currAbility;
    }

    /// <summary> Get array ability by index </summary>
    public Ability GetAbilityByIndex(int index)
    {
        // Check if the index provided is within the array range
        if (index < 0 || index >= abilityInventory.Count)
        {
            string msg = "<color=red>ERROR! INDEX PROVIDED IS OUTSIDE OF RANGE, OR ARRAY IS EMPTY!\n</color>";
            msg += "<color=yellow>Error thrown on AbilityInventory at \"GetAbilityByIndex\"</color>";
            Debug.Log(msg);
            return null;
        }
        // It was valid, return ability
        return abilityInventory[index];
    }

    // Get the current list size of the ability inventory
    public int GetAbilityListCount()
    {
        return abilityInventory.Count;
    }

    // Get the current max size of inventory (set in inspector)
    public int GetMaxAbilityCount()
    {
        return maxAbilityCount;
    }

    #endregion
}
