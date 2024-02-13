using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum declared for all classes to identify their weapon types
public enum BehaviourTypes
{
    SingleShot,
    FanShot,
    TripleOffset,
    Gatling // (?)
}

/// <summary> Abstract class of all weapon types </summary>
[Serializable]
public abstract class Weapon
{
    public BehaviourTypes behaviour;
    public GameObject prefab;
    public string sName;
    public float speed;
    public float shootCooldown;
    public int damage;
}

/// <summary>
/// A general weapon class used for translating scripted weapons to objects.
/// It will need its behaviour specified
/// </summary>
public class GeneralWeapon : Weapon
{
    /// <summary> Default Constructor </summary>
    public GeneralWeapon(GameObject prefab, BehaviourTypes behaviour, float speed, int damage, string name, float timeBetweenShots)
    {
        this.prefab = prefab;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.shootCooldown = timeBetweenShots;
        this.behaviour = behaviour;
    }
}

/// <summary> The Pistol Weapon Class </summary>
[Serializable]
public class Pistol : Weapon
{
    /// <summary> Default Constructor </summary>
    public Pistol(GameObject prefab, float speed, int damage, string name, float timeBetweenShots)
    {
        this.prefab= prefab;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.shootCooldown = timeBetweenShots;
        this.behaviour = BehaviourTypes.SingleShot;
    }
}

/// <summary> The Birdshot Weapon Class </summary>
[Serializable]
public class Birdshot : Weapon
{
    /// <summary> Default Constructor </summary>
    public Birdshot(GameObject prefab, float speed, int damage, string name, float timeBetweenShots)
    {
        this.speed = speed;
        this.prefab = prefab;
        this.damage = damage;
        this.sName = name;
        this.shootCooldown = timeBetweenShots;
        this.behaviour = BehaviourTypes.FanShot;
    }
}

/// <summary> The Buckshot Weapon Class </summary>
[Serializable]
public class Buckshot : Weapon
{
    /// <summary> Default Constructor </summary>
    public Buckshot(GameObject prefab, float speed, int damage, string name, float timeBetweenShots)
    {
        this.speed = speed;
        this.prefab = prefab;
        this.damage = damage;
        this.sName = name;
        this.shootCooldown = timeBetweenShots;
        this.behaviour = BehaviourTypes.TripleOffset;
    }
}
