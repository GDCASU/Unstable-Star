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
    public GameObject prefab;
    public string sName;
    public WeaponTypes behaviour;
    public SoundTag sound;
    public float speed;
    public float shootCooldownTime;
    public bool isOnCooldown;
    public int damage;
}

/// <summary>
/// A general weapon class used for translating scripted weapons to objects.
/// It will need its behaviour specified
/// </summary>
[Serializable]
public class GeneralWeapon : Weapon
{
    /// <summary> Default Constructor </summary>
    public GeneralWeapon(GameObject prefab, SoundTag sound, WeaponTypes behaviour, float speed, int damage, string name, float timeBetweenShots)
    {
        this.prefab = prefab;
        this.sound = sound;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.shootCooldownTime = timeBetweenShots;
        this.behaviour = behaviour;
        this.isOnCooldown = false;
    }

    // Shortcut constructor to make a NULL weapon
    public GeneralWeapon()
    {

    }
}

/* 

###########################################################################

 **** THE USAGE OF CUSTOM CLASSES FOR EACH WEAPON HAS BEEN DEPRECATED ****
  
 From now on, only one general class is used for weapons, [GeneralWeapon].
   The parent Weapon Class and the other children classes are kept for 
   debugging purposes, as to allow people to create a weapon within a 
      script without having to go through the hassle of creating a 
       scriptable object. However, weapons created from a script 
             **SHOULDNT** be permanent implementations.

###########################################################################

*/

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
        this.shootCooldownTime = timeBetweenShots;
        this.behaviour = WeaponTypes.Pistol;
        this.sound = SoundTag.tempRayGunFire;
        this.isOnCooldown = false;
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
        this.shootCooldownTime = timeBetweenShots;
        this.behaviour = WeaponTypes.Birdshot;
        this.sound = SoundTag.tempRayGunFire;
        this.isOnCooldown = false;
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
        this.shootCooldownTime = timeBetweenShots;
        this.behaviour = WeaponTypes.Buckshot;
        this.sound = SoundTag.tempRayGunFire;
        this.isOnCooldown = false;
    }
}
