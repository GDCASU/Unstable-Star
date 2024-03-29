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
    public FMODUnity.EventReference mainSound;
    public float speed;
    public float shootCooldownTime;
    public bool isOnCooldown;
    public int damage;

    // Gatling variables
    public float warmupTime;
    public float warmupCounter;

    // Laser variables
    public GameObject chargingSpherePrefab;
    public float maxChargeUpTime;
    public float chargeTimeCounter;
    public int minDamage;
    public int maxDamage;

    // Variable to override behaviour for enemies
    // EX: Make enemies always charge laser gun to full charge before firing
    public bool isEnemy;
    public float shootingStayTime;

    // This variable should be useful for UI
    public float timeLeftInCooldown;
    public Sprite weaponIcon;
    public string description;
}

/// <summary>
/// A general weapon class used for translating scripted weapons to objects.
/// It will need its behaviour specified
/// </summary>
[Serializable]
public class GeneralWeapon : Weapon
{
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
<<<<<<< Updated upstream
    public Pistol(GameObject prefab, Sprite weaponIcon, FMODUnity.EventReference mainSound, float speed, int damage, string name, float shootCooldownTime)
=======
    public Pistol(GameObject prefab, Sprite weaponIcon, SoundTag sound, float speed, int damage, string name, float shootCooldownTime, string description)
>>>>>>> Stashed changes
    {
        this.prefab= prefab;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.shootCooldownTime = shootCooldownTime;
        this.mainSound = mainSound;
        this.weaponIcon = weaponIcon;
        this.description = description;
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
<<<<<<< Updated upstream
    public Birdshot(GameObject prefab, Sprite weaponIcon, FMODUnity.EventReference mainSound, float speed, int damage, string name, float timeBetweenShots)
=======
    public Birdshot(GameObject prefab, Sprite weaponIcon, SoundTag sound, float speed, int damage, string name, float timeBetweenShots, string description)
>>>>>>> Stashed changes
    {
        this.speed = speed;
        this.prefab = prefab;
        this.damage = damage;
        this.sName = name;
        this.shootCooldownTime = timeBetweenShots;
        this.mainSound = mainSound;
        this.weaponIcon = weaponIcon;

        // Static sets
        this.timeLeftInCooldown = 0f;
        this.weaponType = WeaponTypes.Birdshot;
        this.isOnCooldown = false;
        this.description = description;
    }
}

/// <summary> The Buckshot Weapon Class </summary>
[Serializable]
public class Buckshot : Weapon
{
    //Default Constructor
<<<<<<< Updated upstream
    public Buckshot(GameObject prefab, Sprite weaponIcon, FMODUnity.EventReference mainSound, float speed, int damage, string name, float timeBetweenShots)
=======
    public Buckshot(GameObject prefab, Sprite weaponIcon, SoundTag sound, float speed, int damage, string name, float timeBetweenShots, string description)
>>>>>>> Stashed changes
    {
        this.speed = speed;
        this.prefab = prefab;
        this.damage = damage;
        this.sName = name;
        this.shootCooldownTime = timeBetweenShots;
        this.mainSound = mainSound;
        this.weaponIcon = weaponIcon;

        // Static sets
        this.timeLeftInCooldown = 0f;
        this.weaponType = WeaponTypes.Buckshot;
        this.isOnCooldown = false;
        this.description = description;
    }
}

/// <summary> The Laser Weapon Class </summary>
[Serializable]
public class LaserGun : Weapon
{
    //Default Constructor
<<<<<<< Updated upstream
    public LaserGun(string name, GameObject prefab, GameObject chargingSpherePrefab, Sprite weaponIcon, FMODUnity.EventReference mainSound, int minDamage, int maxDamage, float cooldownTime, float maxChargeUpTime, bool isEnemy)
=======
    public LaserGun(string name, GameObject prefab, GameObject chargingSpherePrefab, Sprite weaponIcon, SoundTag sound, int minDamage, int maxDamage, float cooldownTime, float maxChargeUpTime, bool isEnemy, string description)
>>>>>>> Stashed changes
    {
        this.prefab = prefab;
        this.chargingSpherePrefab = chargingSpherePrefab;
        this.sName = name;
        this.shootCooldownTime = cooldownTime;
        this.mainSound = mainSound;
        this.weaponIcon = weaponIcon;
        this.isEnemy = isEnemy;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.maxChargeUpTime = maxChargeUpTime;

        // Static sets
        this.timeLeftInCooldown = 0f;
        this.weaponType = WeaponTypes.Laser;
        this.isOnCooldown = false;
        this.description = description;
    }
}

/// <summary> The Gatling Weapon Class </summary>
[Serializable]
public class GatlingGun : Weapon
{
    //Default Constructor
<<<<<<< Updated upstream
    public GatlingGun(GameObject prefab, Sprite weaponIcon, FMODUnity.EventReference mainSound, float speed, float warmupTime, int damage, string name, float timeBetweenShots, bool isEnemy, float shootingStayTime)
=======
    public GatlingGun(GameObject prefab, Sprite weaponIcon, SoundTag sound, float speed, float warmupTime, int damage, string name, float timeBetweenShots, bool isEnemy, float shootingStayTime, string description)
>>>>>>> Stashed changes
    {
        this.speed = speed;
        this.prefab = prefab;
        this.damage = damage;
        this.sName = name;
        this.shootCooldownTime = timeBetweenShots;
        this.mainSound = mainSound;
        this.weaponIcon = weaponIcon;
        this.warmupTime = warmupTime;
        this.isEnemy = isEnemy;
        this.description = description;

        // If this is an enemy entity, then set the shooting stay time
        if (isEnemy) this.shootingStayTime = shootingStayTime;

        // Static sets
        this.timeLeftInCooldown = 0f;
        this.warmupCounter = 0f;
        this.weaponType = WeaponTypes.Gatling;
        this.isOnCooldown = false;
    }
}
