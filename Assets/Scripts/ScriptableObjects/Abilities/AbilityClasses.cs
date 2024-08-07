using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum declared for all ability classes to identify their weapon types
public enum AbilityTypes
{
    NULL,
    ProxiBomb,
    PhaseShift
}

// Ability Parent Class
public abstract class Ability
{
    // Common Variables
    public string sName;
    public AbilityTypes abilityType;
    public float cooldownTime;
    public float durationTime;
    public bool isOnCooldown;
    public int charges;
    public string description;

    // Proximity Bomb Variables
    public FMODUnity.EventReference bombExplosion;
    public GameObject bombPrefab;
    public float bombRadius;
    public int damage;

    // Phase Shift Variables
    public FMODUnity.EventReference phaseShiftEnter;
    public FMODUnity.EventReference phaseShiftExit;
    public Material PhaseShiftMaterial;
    public GameObject particleEmitter;

    // This variable should be useful for implementing UI
    public float timeLeftInCooldown;
    public Sprite abilityIconActive;
    public Sprite abilityIconInactive;
}

/// <summary>
/// A general ability class used for debugging purposes, dont create abilities with this
/// </summary>
public class GeneralAbility : Ability
{
    // General Constructor?
    public GeneralAbility(string name, AbilityTypes type, float cooldownTime, float durationTime)
    {
        this.sName = name;
        this.abilityType = type;
        this.cooldownTime = cooldownTime;
        this.durationTime = durationTime;

        // Static sets
        this.isOnCooldown = false;
        timeLeftInCooldown = 0f;
        charges = 10;
    }

    // Shortcut constructor to make a null ability with a name
    public GeneralAbility(string sName)
    {
        this.sName = sName;
        this.abilityType = AbilityTypes.NULL;
    }
}


// The phase Shift ability class
public class PhaseShiftAbility : Ability
{
    // Constructor
    public PhaseShiftAbility(string name, Sprite abilityIconActive, FMODUnity.EventReference phaseShiftEnter, FMODUnity.EventReference phaseShiftExit, Sprite abilityIconInactive, Material phaseShiftMaterial, GameObject particleEmitter, int charges, float cooldownTime, float durationTime, string description)
    {
        this.sName = name;
        this.cooldownTime = cooldownTime;
        this.particleEmitter = particleEmitter;
        this.durationTime = durationTime;
        this.PhaseShiftMaterial = phaseShiftMaterial;
        this.charges = charges;
        this.abilityIconActive = abilityIconActive;
        this.abilityIconInactive = abilityIconInactive;
        this.description = description;
        this.phaseShiftEnter = phaseShiftEnter;
        this.phaseShiftExit = phaseShiftExit;
        // Static sets
        this.abilityType = AbilityTypes.PhaseShift;
        this.isOnCooldown = false;
        timeLeftInCooldown = 0f;

    }
}

// The proximity bomb ability class
public class ProximityBombAbility : Ability
{
    // Constructor
    public ProximityBombAbility(string name, Sprite abilityIconActive, FMODUnity.EventReference bombExplosion, Sprite abilityIconInactive, GameObject bombPrefab, float bombRadius, float cooldownTime, int charges, int damage, string description)
    {
        this.sName = name;
        this.damage = damage;
        this.bombPrefab = bombPrefab;
        this.cooldownTime = cooldownTime;
        this.bombRadius = bombRadius;
        this.charges = charges;
        this.abilityIconActive = abilityIconActive;
        this.abilityIconInactive = abilityIconInactive;
        this.description= description;
        this.bombExplosion = bombExplosion;
        // Static sets
        this.abilityType = AbilityTypes.ProxiBomb;
        this.isOnCooldown = false;
        timeLeftInCooldown = 0f;
    }
}
