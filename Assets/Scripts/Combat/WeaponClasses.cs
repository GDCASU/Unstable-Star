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

// Enum used for color selection of the bullets
public enum BulletColors
{
    Red,
    Green,
    Yellow,
    NULL // Used for null weapon checks
}

/// <summary> Abstract class of all weapon types </summary>
[Serializable]
public abstract class Weapon
{
    public BulletColors color;
    public BehaviourTypes behaviour;
    public string sName;
    public float speed;
    public float shootCooldown;
    public int damage;
}

/// <summary> The Pistol Weapon Class </summary>
[Serializable]
public class Pistol : Weapon
{
    /// <summary> Default Constructor </summary>
    public Pistol(BulletColors color, float speed, int damage, string name, float timeBetweenShots)
    {
        this.color = color;
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
    public Birdshot(BulletColors color, float speed, int damage, string name, float timeBetweenShots)
    {
        this.color = color;
        this.speed = speed;
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
    public Buckshot(BulletColors color, float speed, int damage, string name, float timeBetweenShots)
    {
        this.color = color;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.shootCooldown = timeBetweenShots;
        this.behaviour = BehaviourTypes.TripleOffset;
    }
}
