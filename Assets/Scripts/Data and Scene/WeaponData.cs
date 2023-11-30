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


/// <summary> Used by Shoot.cs to access all weapon settings </summary>
public class WeaponData : MonoBehaviour
{
    //Singleton
    [HideInInspector] public static WeaponData Instance;
    
    //All possible bullet prefabs must be added here
    [Header("All Possible Bullet Prefabs")]
    public GameObject RedBullet;
    public GameObject GreenBullet;
    public GameObject YellowBullet;

    //Variables
    [HideInInspector] public GameObject projectileContainer { get; private set; }

    private void Awake()
    {
        //Create Projectile Container
        projectileContainer = Instantiate(new GameObject());
        projectileContainer.name = "Projectile Container";

        //Set Singleton
        Instance = this;
    }

}

/// <summary> Abstract class of all weapon types </summary>
public abstract class Weapon
{
    public GameObject prefab;
    public BehaviourTypes behaviour;
    public string sName;
    public float speed;
    public int damage;
}

/// <summary> The Pistol Weapon Class </summary>
public class Pistol : Weapon
{
    /// <summary> Default Constructor </summary>
    public Pistol(float speed, int damage, string name)
    {
        this.prefab = WeaponData.Instance.RedBullet;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.behaviour = BehaviourTypes.SingleShot;
    }

    /// <summary> Constructor to override prefab of bullet </summary>
    public Pistol(GameObject prefab, float speed, int damage, string name) 
    {
        this.prefab = prefab;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.behaviour = BehaviourTypes.SingleShot;
    }
}

/// <summary> The Birdshot Weapon Class </summary>
public class Birdshot : Weapon
{
    /// <summary> Default Constructor </summary>
    public Birdshot(float speed, int damage, string name)
    {
        this.prefab = WeaponData.Instance.GreenBullet;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.behaviour = BehaviourTypes.FanShot;
    }

    /// <summary> Constructor to override prefab of bullet </summary>
    public Birdshot(GameObject prefab, float speed, int damage, string name)
    {
        this.prefab = prefab;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.behaviour = BehaviourTypes.FanShot;
    }
}

/// <summary> The Buckshot Weapon Class </summary>
public class Buckshot : Weapon
{
    /// <summary> Default Constructor </summary>
    public Buckshot(float speed, int damage, string name)
    {
        this.prefab = WeaponData.Instance.YellowBullet;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.behaviour = BehaviourTypes.TripleOffset;
    }

    /// <summary> Constructor to override prefab of bullet </summary>
    public Buckshot(GameObject prefab, float speed, int damage, string name)
    {
        this.prefab = prefab;
        this.speed = speed;
        this.damage = damage;
        this.sName = name;
        this.behaviour = BehaviourTypes.TripleOffset;
    }
}
