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
    public Sprite abilityIcon;
    public int charges;
    public float cooldownTime;
    public float bombRadius;
    public int damage;

    public override Ability GetAbilityObject()
    {
        return new ProximityBombAbility(sName, abilityIcon, bombPrefab, bombRadius, cooldownTime, charges, damage);
    }
}
