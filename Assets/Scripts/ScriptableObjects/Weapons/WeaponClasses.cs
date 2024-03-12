using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum declared for all weapon classes to identify their weapon types
public enum WeaponTypes
{
    NULL,
    Pistol,
    Birdshot,
    Buckshot,
    Gatling,
    Laser
}

/// <summary> Abstract class of all weapon types </summary>
[Serializable]
public abstract class Weapon
{
    // Common variables
    public GameObject prefab;
    public string sName;
    public WeaponTypes weaponType;
    public SoundTag sound;
    public float speed;
    public float shootCooldownTime;
    public bool isOnCooldown;
    public int damage;

    // Gatling variables
    public float warmupTime;

    // Laser variables
    public float maxChargeUpTime;
    public int minDamage;
    public int maxDamage;

    // Variable to override behaviour for enemies
    // EX: Make enemies always charge laser gun to full charge before firing
    public bool isEnemy;

    // This variable should be useful for UI
    public float timeLeftInCooldown;
    public Sprite weaponIcon;
}

/// <summary>
/// A general weapon class used for translating scripted weapons to objects.
/// It will need its behaviour specified
/// </summary>
[Serializable]
public class GeneralWeapon : Weapon
{
    //Default Constructor
    //public GeneralWeapon(GameObject prefab, SoundTag sound, WeaponTypes behaviour, float speed, int damage, string name, float timeBetweenShots)
    //{
    //    this.prefab = prefab;
    //    this.sound = sound;
    //    this.speed = speed;
    //    this.damage = damage;
    //    this.sName = name;
    //    this.shootCooldownTime = timeBetweenShots;
    //    this.behaviour = behaviour;
    //    this.isOnCooldown = false;
    //}

    // Shortcut constructor to make a NULL weapon
    public GeneralWeapon(string name)
    {
        this.sName = name;
        this.prefab = null;
        this.weaponType = WeaponTypes.NULL;
    }
}

/// <summary> The Pistol Weapon Class </summary>
[Serializable]
public class Pistol : Weapon
{
    //Constructor
    public Pistol(GameObject prefab, Sprite weaponIcon, SoundTag sound, float speed, int damage, string name, float shootCooldownTime)
    {
        this.prefab= prefab;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.shootCooldownTime = shootCooldownTime;
        this.sound = sound;
        this.weaponIcon = weaponIcon;

        // Static sets
        this.timeLeftInCooldown = 0f;
        this.weaponType = WeaponTypes.Pistol;
        this.isOnCooldown = false;
    }
}

/// <summary> The Birdshot Weapon Class </summary>
[Serializable]
public class Birdshot : Weapon
{
    //Default Constructor
    public Birdshot(GameObject prefab, Sprite weaponIcon, SoundTag sound, float speed, int damage, string name, float timeBetweenShots)
    {
        this.speed = speed;
        this.prefab = prefab;
        this.damage = damage;
        this.sName = name;
        this.shootCooldownTime = timeBetweenShots;
        this.sound = sound;
        this.weaponIcon = weaponIcon;

        // Static sets
        this.timeLeftInCooldown = 0f;
        this.weaponType = WeaponTypes.Birdshot;
        this.isOnCooldown = false;
    }
}

/// <summary> The Buckshot Weapon Class </summary>
[Serializable]
public class Buckshot : Weapon
{
    //Default Constructor
    public Buckshot(GameObject prefab, Sprite weaponIcon, SoundTag sound, float speed, int damage, string name, float timeBetweenShots)
    {
        this.speed = speed;
        this.prefab = prefab;
        this.damage = damage;
        this.sName = name;
        this.shootCooldownTime = timeBetweenShots;
        this.sound = sound;
        this.weaponIcon = weaponIcon;

        // Static sets
        this.timeLeftInCooldown = 0f;
        this.weaponType = WeaponTypes.Buckshot;
        this.isOnCooldown = false;
    }
}

/// <summary> The Laser Weapon Class </summary>
[Serializable]
public class LaserGun : Weapon
{
    //Default Constructor
    public LaserGun(GameObject prefab, Sprite weaponIcon, SoundTag sound, float speed, int damage, string name, float timeBetweenShots, bool isEnemy = true)
    {
        this.speed = speed;
        this.prefab = prefab;
        this.damage = damage;
        this.sName = name;
        this.shootCooldownTime = timeBetweenShots;
        this.sound = sound;
        this.weaponIcon = weaponIcon;
        this.isEnemy = isEnemy;

        // Static sets
        this.timeLeftInCooldown = 0f;
        this.weaponType = WeaponTypes.Buckshot;
        this.isOnCooldown = false;
    }
}

/// <summary> The Gatling Weapon Class </summary>
[Serializable]
public class GatlingGun : Weapon
{
    //Default Constructor
    public GatlingGun(GameObject prefab, Sprite weaponIcon, SoundTag sound, float speed, float warmupTime, int damage, string name, float timeBetweenShots, bool isEnemy = true)
    {
        this.speed = speed;
        this.prefab = prefab;
        this.damage = damage;
        this.sName = name;
        this.shootCooldownTime = timeBetweenShots;
        this.sound = sound;
        this.weaponIcon = weaponIcon;
        this.warmupTime = warmupTime;
        this.isEnemy = isEnemy;

        // Static sets
        this.timeLeftInCooldown = 0f;
        this.weaponType = WeaponTypes.Gatling;
        this.isOnCooldown = false;
    }
}
