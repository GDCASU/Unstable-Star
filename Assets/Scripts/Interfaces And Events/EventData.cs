using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Place where all events are defined
public class EventData : MonoBehaviour
{
    //Events and the functions that raises them

    //Player's death
    public static event System.Action OnPlayerDeath; //Action List
    public static void RaiseOnPlayerDeath() { OnPlayerDeath?.Invoke(); } //Raiser

    //Events used to call an update to the UI, play animations, sounds, etc.

    //Health Lost
    public static event System.Action OnHealthLost; 
    public static void RaiseOnHealthLost() { OnHealthLost?.Invoke(); } 

    //Health Added
    public static event System.Action OnHealthAdded; 
    public static void RaiseOnHealthAdded() { OnHealthAdded?.Invoke(); }

    //Shield Damaged
    public static event System.Action OnShieldDamaged; 
    public static void RaiseOnShieldDamaged() { OnShieldDamaged?.Invoke(); }

    //Shield Broken
    public static event System.Action OnShieldBroken; 
    public static void RaiseOnShieldBroken() { OnShieldBroken?.Invoke(); }
}
