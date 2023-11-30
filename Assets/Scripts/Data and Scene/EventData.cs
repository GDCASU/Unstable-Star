using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Collection of static events pertinent to the game </summary>
public class EventData : MonoBehaviour
{
    //Player's Death ----------------------------------------------------------------

    /// <summary> Player's death event </summary>
    public static event System.Action OnPlayerDeath; //Action List
    /// <summary> Triggers all functions subscribed to OnPlayerDeath </summary>
    public static void RaiseOnPlayerDeath() { OnPlayerDeath?.Invoke(); } //Raiser


    //Events used to call an update to the UI, play animations, sounds, etc. --------


    /// <summary> Health Lost Event </summary>
    public static event System.Action OnHealthLost;
    /// <summary> Triggers all functions subscribed to OnHealthLost </summary>
    public static void RaiseOnHealthLost() { OnHealthLost?.Invoke(); }

    /// <summary> Health Added Event </summary>
    public static event System.Action OnHealthAdded;
    /// <summary> Triggers all functions subscribed to OnHealthAdded </summary>
    public static void RaiseOnHealthAdded() { OnHealthAdded?.Invoke(); }

    /// <summary> Shield Damaged Event </summary>
    public static event System.Action OnShieldDamaged;
    /// <summary> Triggers all functions subscribed to OnShieldDamaged </summary>
    public static void RaiseOnShieldDamaged() { OnShieldDamaged?.Invoke(); }

    /// <summary> Shield Broken Event </summary>
    public static event System.Action OnShieldBroken;
    /// <summary> Triggers all functions subscribed to OnShieldBroken </summary>
    public static void RaiseOnShieldBroken() { OnShieldBroken?.Invoke(); }
}
