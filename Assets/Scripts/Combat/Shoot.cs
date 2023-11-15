using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> Script/Class that allows any object to shoot projectiles </summary>
public class Shoot : MonoBehaviour
{
    [Header("Shoot.cs Variables")]
    [SerializeField] private GameObject AnchorObject;
    [SerializeField] private bool TestShoot = false;

    //Local Variables
    private int projectileLayer;
    private Weapon currWeapon = new Pistol(null, 0, 0, "NULL"); //Needs to start null
    private GameObject projectileContainer; //Projectile storage in hierarchy
    private Dictionary<Weapon, Action> WeaponCalls = new();

    private void Start()
    {
        //Stores projectiles in "Container For Projectiles"
        projectileContainer = GameObject.Find("Container For Projectiles");

        //Sets the layer of the bullet depending on if its the player shooting or the enemy
        switch (this.gameObject.tag)
        {
            case "Player":
                //The player is shooting
                projectileLayer = LayerMask.NameToLayer("Projectiles Player");
                LoadPlayerWeaponData();
                break;
            case "Enemy":
                //The Enemy is shooting
                projectileLayer = LayerMask.NameToLayer("Projectiles Enemies");
                LoadEnemyWeaponData();
                break;
            default:
                Debug.Log("ERORR!!! AN OBJECT THAT IS SHOOTING IS UNTAGGED AND/OR UNDEFINED IN SHOOT.CS");
                break;
        }
    }

    //Testing loop
    private void Update()
    {
        if (TestShoot)
        {
            ShootCurrentWeapon();
            TestShoot = false;
        }
    }

    //Load Player weapons
    private void LoadPlayerWeaponData()
    {
        //Player
        WeaponCalls[WeaponData.Instance.PlayerPistol] = SingleShotBehaviour;
        WeaponCalls[WeaponData.Instance.PlayerBirdshot] = FanShotBehaviour;
        WeaponCalls[WeaponData.Instance.PlayerBuckshot] = OffsetBehaviour;
        //Set default Weapon
        currWeapon = WeaponData.Instance.PlayerPistol;
    }

    //Load Enemies default weapons
    //NOTE: it might be necessary to edit this if we have many enemies with many different weapons
    private void LoadEnemyWeaponData()
    {
        //Enemies
        WeaponCalls[WeaponData.Instance.defaultEnemPistol] = SingleShotBehaviour;
        //Set default weapon
        currWeapon = WeaponData.Instance.defaultEnemPistol;
    }

    #region SHOOT SCRIPT API

    /* -----------------------------------------------------------------------
                                SHOOT SCRIPT API

        This section contains all relevant functions that should be
        accessed by other objects, like for example the player controller.
      ----------------------------------------------------------------------- */

    /// <summary> 
    /// Set the Weapon of the entity, inputWeapon must already exists within Dictionary 
    /// </summary>
    public void SetWeaponFire(Weapon inputWeapon)
    {
        currWeapon = inputWeapon;
    }

    /// <summary> Gets the current weapon this entity is holding </summary>
    public Weapon GetCurrWeapon()
    {
        return currWeapon;
    }

    /// <summary> Makes the object shoot its current weapon </summary>
    public void ShootCurrentWeapon()
    {
        WeaponCalls[currWeapon].Invoke();
    }

    /// <summary> Add a new weapon to the arsenal, DOES NOT SET IT TO CURRENT WEAPON </summary>
    public void AddWeaponToShoot(Weapon weapon, int shootingBehaviour)
    {
        /* Number Index:
         * 1 = One shot Behaviour
         * 2 = Fanning triple shot Behaviour
         * 3 = Offset Triple shot Behaviour
         */

        //First check if this weapon is already on the dictionary, to prevent errors
        if (WeaponCalls.ContainsKey(weapon))
        {
            Debug.Log("ERROR! WEAPON CALLS DICTIONARY ALREADY HAD A WEAPON OF THIS TYPE");
            Debug.Log("You tried to add weapon: " + weapon.GetName());
            return;
        }

        //Else, assign it a spawning behaviour
        switch (shootingBehaviour)
        {
            case 1:
                WeaponCalls[weapon] = SingleShotBehaviour;
                break;
            case 2:
                WeaponCalls[weapon] = FanShotBehaviour;
                break;
            case 3:
                WeaponCalls[weapon] = OffsetBehaviour;
                break;
            default:
                Debug.Log("ERROR! UKNOWN behaviourType NUMBER PASSED IN AddWeaponToShoot()");
                break;
        }

        //If its the player adding a new weapon, also add it to PlayerWeaponList
        if (this.gameObject.CompareTag("Player"))
        {
            WeaponData.Instance.PlayerWeaponList.Add(weapon);
        }
    }

    #endregion

    #region SPAWNING BEHAVIOURS

    /* -----------------------------------------------------------------------
                        BEHAVIOUR OF SPAWNED PROJECTILES

        This section contains the behaviour of all bullets/laser when they
        spawn. Should be able to spawn single, double, triple bullets
        Bullets that slow or speed up, anything design needs/wants
      ----------------------------------------------------------------------- */

    //TODO: Should these behaviours get their own script?

    /// <summary> Only shoots 1 projectile </summary>
    private void SingleShotBehaviour()
    {
        //Create the Projectile
        InstantiateProjectile(currWeapon);
    }

    /// <summary> spawns the bullet in a fan style shot -> \ | / </summary>
    private void FanShotBehaviour()
    {
        //Create the Projectile
        InstantiateProjectile(currWeapon, 30f);
        InstantiateProjectile(currWeapon, -30f);
        InstantiateProjectile(currWeapon, 0, 1);
    }

    /// <summary> OffsetBehaviour: 3 separated but same direction shots </summary>
    private void OffsetBehaviour()
    {
        //Create the Projectile
        InstantiateProjectile(currWeapon, 0, 1);
        InstantiateProjectile(currWeapon, 2, -2);
        InstantiateProjectile(currWeapon, -2, -2);
    }

    #endregion

    #region INSTANTIATE INSTRUCTIONS

    // Overloaded Functions, Spawns the projectile
    // NOTE: The bullets spawn where the anchor points is located, this could be fixed with an offset
    // If necessary, but the entities use long capsule colliders, so its fine now

    /// <summary> Shoot Straight </summary>
    private void InstantiateProjectile(Weapon weapon)
    {
        //Create a quaternion with only the Z axis
        Quaternion zRotation = computeRotation();

        //Spawn projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, AnchorObject.transform.position, zRotation, projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary> Add a float to the angle (In degrees) </summary>
    private void InstantiateProjectile(Weapon weapon, float addedAngle)
    {
        //Create Rotation Offset
        Quaternion modifiedRotation = computeRotation(addedAngle);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, AnchorObject.transform.position, modifiedRotation, projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary>
    /// Offset Spawning point <para />
    /// HACK: If the gameObject rotates on any non-z axis, bullets come out weird
    /// </summary>
    private void InstantiateProjectile(Weapon weapon, float offsetX, float offsetY)
    {
        //Create a quaternion with only the Z axis
        Quaternion zRotation = computeRotation();

        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0f);

        //Translate Vector to global space
        Vector3 overridenPosition = AnchorObject.transform.TransformPoint(point);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, overridenPosition, zRotation, projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary> Add to angle and offset spawn point <para />
    /// HACK: If the gameObject rotates on any non-z axis, bullets come out weird
    /// </summary>
    private void InstantiateProjectile(Weapon weapon, float offsetX, float offsetY, float addedAngle)
    {
        //Position Offset -----------------------
        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0f);

        //Translate Vector to global space
        Vector3 overridenPosition = AnchorObject.transform.TransformPoint(point);

        // Create Rotation Offset ---------------
        Quaternion modifiedRotation = computeRotation(addedAngle);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, overridenPosition, modifiedRotation, projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, weapon.speed, weapon.damage);

    }

    // Helper method for creating the quaternion of rotation.
    // Hopefully makes it easier to read
    private Quaternion computeRotation(float addedAngle = 0f)
    {
        float angle = AnchorObject.transform.rotation.eulerAngles.z + addedAngle;
        Quaternion newRotation = Quaternion.Euler(0f, 0f, angle);
        return newRotation;
    }

    #endregion
}