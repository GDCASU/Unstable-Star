using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Scriptable Object that contains the prefab data to the player entity 
/// </summary>
[CreateAssetMenu(fileName = "ScriptablePlayerData", menuName = "ScriptableObjects/Player")]
public class ScriptablePlayer : ScriptableObject
{
    // Stats of scripted enemy
    [Header("Player Stats")]
    public int maxHealth;
    public int maxShield;
    public float shieldPerSecond;
    public float shieldRegenDelayTime; // in seconds, does not stack with invulnerable time
    public int collisionDamage;
    public float dmgInvulnTimeSecs; // in seconds
}
