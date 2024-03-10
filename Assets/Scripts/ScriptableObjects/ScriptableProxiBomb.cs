using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the data for a Proximity Bomb ability </summary>
[CreateAssetMenu(fileName = "ScriptableProxiBomb", menuName = "ScriptableObjects/Abilities/Proximity Bomb")]
public class ScriptableProxiBomb : ScriptableObject
{
    // Variables
    [Header("Proximity Bomb Settings")]
    public string sName;
    public GameObject bombPrefab;
    public int charges;
    public float cooldownTime;
    public float bombRadius;
    public int damage;

    /// <summary>
    /// Helper function to construct an Ability object from stored data
    /// </summary>
    public Ability GetAbilityObject()
    {
        return new ProximityBombAbility(sName, bombPrefab, bombRadius, cooldownTime, charges, damage);
    }
}
