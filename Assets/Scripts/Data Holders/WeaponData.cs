using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Used by Shoot.cs to access all weapon settings </summary>
public class WeaponData : MonoBehaviour
{
    //Singleton
    public static WeaponData Instance;
    
    //All possible bullet prefabs must be added here
    [Header("All Possible Bullet Prefabs")]
    public GameObject RedBullet;
    public GameObject YellowBullet;
    public GameObject PinkBullet;

    //Player Weapons
    public Pistol PlayerPistol;
    public Birdshot PlayerBirdshot;
    public Buckshot PlayerBuckshot;

    //Enemy Weapons
    public Pistol defaultEnemPistol;

    //Create the Weapons
    private void Start()
    {
        //Player Weapons
        PlayerPistol = new Pistol(RedBullet, 30f, 1);
        PlayerBirdshot = new Birdshot(YellowBullet, 30f, 1);
        PlayerBuckshot = new Buckshot(PinkBullet, 30f, 1);

        //Enemy weapons
        defaultEnemPistol = new Pistol(RedBullet, 10f, 1);

        //Set Singleton at last, its here to make fully sure all weapons were loaded
        //Before Any scripts accesses this data

        Instance = this;
    }

}

/// <summary> Abstract class of all weapon types </summary>
public abstract class Weapon
{
    public GameObject prefab;
    public float speed;
    public int damage;
    protected string sName;

    public void SetDamage(int damage) { this.damage = damage; }
    public void SetSpeed(float speed) { this.speed = speed; }
    public void SetPrefab(GameObject prefab) { this.prefab = prefab; }
    public string GetName() { return sName; }

    /// <summary> The Utility Error Message </summary>
    protected void PrintLoadError()
    {
        // If this error pops up in your console, you need to delay
        // The load of your weapons until [WeaponsData.Instance] is set.
        // Reference the last part of the Awake() function call in for how to handle
        // delayed loading or contact Ian Fletcher for help if needed
        Debug.Log("ERROR! ATTEMPTED TO CREATE A WEAPON OBJECT BEFORE WeaponData.Instance WAS INSTANTIATED (Thrown in Weapon Constructor)");
        Debug.Log("Please Open the script WeaponData.cs for more information");
    }
}

/// <summary> The Pistol Weapon Class </summary>
public class Pistol : Weapon
{
    /// <summary> Default Constructor </summary>
    public Pistol(float speed, int damage)
    {
        //Error message utility
        if (WeaponData.Instance == null)
        {
            PrintLoadError();
        }

        this.prefab = WeaponData.Instance.RedBullet;
        this.speed = speed;
        this.damage = damage;
        this.sName = "Pistol";
    }

    /// <summary> Constructor to override prefab of bullet </summary>
    public Pistol(GameObject prefab, float speed, int damage) 
    {
        this.prefab = prefab;
        this.speed = speed;
        this.damage = damage;
        this.sName = "Pistol";
    }
}

/// <summary> The Birdshot Weapon Class </summary>
public class Birdshot : Weapon
{
    /// <summary> Default Constructor </summary>
    public Birdshot(float speed, int damage)
    {
        //Error message utility
        if (WeaponData.Instance == null)
        {
            PrintLoadError();
        }

        this.prefab = WeaponData.Instance.YellowBullet;
        this.speed = speed;
        this.damage = damage;
        this.sName = "Birdshot";
    }

    /// <summary> Constructor to override prefab of bullet </summary>
    public Birdshot(GameObject prefab, float speed, int damage)
    {
        this.prefab = prefab;
        this.speed = speed;
        this.damage = damage;
        this.sName = "Birdshot";
    }
}

/// <summary> The Buckshot Weapon Class </summary>
public class Buckshot : Weapon
{
    /// <summary> Default Constructor </summary>
    public Buckshot(float speed, int damage)
    {
        //Error message utility
        if (WeaponData.Instance == null)
        {
            PrintLoadError();
        }

        this.prefab = WeaponData.Instance.PinkBullet;
        this.speed = speed;
        this.damage = damage;
        this.sName = "Buckshot";
    }

    /// <summary> Constructor to override prefab of bullet </summary>
    public Buckshot(GameObject prefab, float speed, int damage)
    {
        this.prefab = prefab;
        this.speed = speed;
        this.damage = damage;
        this.sName = "Buckshot";
    }
}
