using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the data for a Proximity Bomb ability </summary>
[CreateAssetMenu(fileName = "ScriptableProxiBomb", menuName = "ScriptableObjects/Abilities/Proximity Bomb")]
public class ScriptableProxiBomb : ScriptableAbility
{
    // Variables
    [Header("Proximity Bomb Settings")]
    public string sName;
    public GameObject bombPrefab;
    public Sprite abilityIconActive;
    public Sprite abilityIconInactive;
    public int charges;
    public float cooldownTime;
    public float bombRadius;
    public int damage;
    [TextAreaAttribute]
    public string description;

    public override Ability GetAbilityObject()
    {
        return new ProximityBombAbility(sName, abilityIconActive, abilityIconInactive, bombPrefab, bombRadius, cooldownTime, charges, damage, description);
    }
}
