using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Collection of static events pertinent to the game </summary>
public class EventData : MonoBehaviour
{
    //Events are separated between gains and losses for easier SFX and UI implementations

    #region PLAYER DEATH

    /// <summary> Player's death event </summary>
    public static event System.Action OnPlayerDeath; //Action List
    /// <summary> Triggers all functions subscribed to OnPlayerDeath </summary>
    public static void RaiseOnPlayerDeath() { OnPlayerDeath?.Invoke(); } //Raiser

    #endregion

    #region PLAYER HEALTH

    /// <summary> Health Lost Event </summary>
    public static event System.Action<int> OnHealthLost;
    /// <summary> Triggers all functions subscribed to OnHealthLost </summary>
    public static void RaiseOnHealthLost(int currHealth)
    { OnHealthLost?.Invoke(currHealth); }


    /// <summary> Health Gained Event </summary>
    public static event System.Action<int> OnHealthGained;
    /// <summary> Triggers all functions subscribed to OnHealthGained </summary>
    public static void RaiseOnHealthAdded(int currHealth)
    { OnHealthGained?.Invoke(currHealth); }

    #endregion

    #region PLAYER SHIELD

    /// <summary> Shield Damaged Event </summary>
    public static event System.Action<int> OnShieldDamaged;
    /// <summary> Triggers all functions subscribed to OnShieldDamaged </summary>
    public static void RaiseOnShieldDamaged(int currShield) 
    { OnShieldDamaged?.Invoke(currShield); }


    /// <summary> Shield Point Regenerated Event </summary>
    public static event System.Action<int> OnShieldGained;
    /// <summary> Triggers all functions subscribed to OnShieldPointGained </summary>
    public static void RaiseOnShieldGained(int currShield)
    { OnShieldGained?.Invoke(currShield); }


    /// <summary> Shield Broken Event </summary>
    public static event System.Action<int> OnShieldBroken;
    /// <summary> Triggers all functions subscribed to OnShieldBroken </summary>
    public static void RaiseOnShieldBroken(int currShield) { OnShieldBroken?.Invoke(currShield); }

    #endregion

}