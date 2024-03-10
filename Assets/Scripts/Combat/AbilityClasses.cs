using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum declared for all ability classes to identify their weapon types
public enum AbilityTypes
{
    NULL,
    ProxiBomb,
    PhaseShift,
    //EMPGrenade // NOTE: Just an idea, probably wont implement
}


// Ability class
public abstract class Ability
{
    // Common Variables
    public string sName;
    public AbilityTypes behaviour;
    public SoundTag sound;
    public float cooldownTime;
    public float durationTime;
    public bool isOnCooldown;

    // Proximity Bomb Variables
    public GameObject bombPrefab;
    public float bombRadius;
    public int damage;

    // Phase Shift Variables
    public Material PhaseShiftMaterial;

    // This variable should be useful for implementing UI
    public float timeLeftInCooldown;
}

/// <summary>
/// A general ability class used for debugging purposes, dont create abilities with this
/// </summary>
public class GeneralAbility : Ability
{
    // Full constructor FIXME: UNFINISHED!!!
    public GeneralAbility(string name, AbilityTypes behaviour, float cooldownTime, float durationTime)
    {
        this.sName = name;
        this.behaviour = behaviour;
        this.cooldownTime = cooldownTime;
        this.durationTime = durationTime;

        // Static sets
        this.isOnCooldown = false;
        timeLeftInCooldown = 0f;
    }

    // Shortcut constructor to make a null ability with a name
    public GeneralAbility(string sName)
    {
        this.sName = sName;
        this.behaviour = AbilityTypes.NULL;
    }
}


// The phase Shift ability class
public class PhaseShiftAbility : Ability
{
    // Constructor
    public PhaseShiftAbility(string name, Material phaseShiftMaterial, float cooldownTime, float durationTime)
    {
        // FIXME: UNFINISHED!!!
        this.sName = name;
        this.cooldownTime = cooldownTime;
        this.durationTime = durationTime;
        this.PhaseShiftMaterial = phaseShiftMaterial;

        // Static sets
        this.behaviour = AbilityTypes.PhaseShift;
        this.isOnCooldown = false;
        timeLeftInCooldown = 0f;
    }
}

// The proximity bomb ability class
public class ProximityBombAbility : Ability
{
    // Constructor
    public ProximityBombAbility(string name, GameObject bombPrefab, float bombRadius, float cooldownTime, int damage)
    {
        // FIXME: UNFINISHED!!!
        this.sName = name;
        this.damage = damage;
        this.bombPrefab = bombPrefab;
        this.cooldownTime = cooldownTime;
        this.bombRadius = bombRadius;

        // Static sets
        this.behaviour = AbilityTypes.ProxiBomb;
        this.isOnCooldown = false;
        timeLeftInCooldown = 0f;
    }
}
