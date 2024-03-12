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
    public AbilityTypes behaviour;
    public SoundTag sound;
    public float cooldownTime;
    public float durationTime;
    public bool isOnCooldown;
    public int charges;

    // Proximity Bomb Variables
    public GameObject bombPrefab;
    public float bombRadius;
    public int damage;

    // Phase Shift Variables
    public Material PhaseShiftMaterial;
    public GameObject particleEmitter;

    // This variable should be useful for implementing UI
    public float timeLeftInCooldown;
    public Sprite abilityIcon;
}

/// <summary>
/// A general ability class used for debugging purposes, dont create abilities with this
/// </summary>
public class GeneralAbility : Ability
{
    // General Constructor?
    public GeneralAbility(string name, AbilityTypes behaviour, float cooldownTime, float durationTime)
    {
        this.sName = name;
        this.behaviour = behaviour;
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
        this.behaviour = AbilityTypes.NULL;
    }
}


// The phase Shift ability class
public class PhaseShiftAbility : Ability
{
    // Constructor
    public PhaseShiftAbility(string name, Sprite abilityIcon, Material phaseShiftMaterial, GameObject particleEmitter, int charges, float cooldownTime, float durationTime)
    {
        this.sName = name;
        this.cooldownTime = cooldownTime;
        this.particleEmitter = particleEmitter;
        this.durationTime = durationTime;
        this.PhaseShiftMaterial = phaseShiftMaterial;
        this.charges = charges;
        this.abilityIcon = abilityIcon;

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
    public ProximityBombAbility(string name, Sprite abilityIcon, GameObject bombPrefab, float bombRadius, float cooldownTime, int charges, int damage)
    {
        this.sName = name;
        this.damage = damage;
        this.bombPrefab = bombPrefab;
        this.cooldownTime = cooldownTime;
        this.bombRadius = bombRadius;
        this.charges = charges;
        this.abilityIcon = abilityIcon;

        // Static sets
        this.behaviour = AbilityTypes.ProxiBomb;
        this.isOnCooldown = false;
        timeLeftInCooldown = 0f;
    }
}
