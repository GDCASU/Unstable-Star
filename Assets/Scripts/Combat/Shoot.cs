using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> Script/Class that allows any object to shoot projectiles </summary>
public class Shoot : MonoBehaviour
{
    [Header("Testing Variables")]
    public bool TestShoot = false;

    private void Awake()
    {
        BehaviourAwake();
        APIAwake();

        //Load Weapon References, needs a delay to wait for initialization of WeaponData
        if (WeaponData.Instance == null) { StartCoroutine(DelayedWeaponLoad()); }
        //The Instance was already set, load right away
        else { LoadWeaponData(); }
    }

    /// <summary> Delayed loader for weapon data </summary>
    private IEnumerator DelayedWeaponLoad()
    {
        while (WeaponData.Instance == null)
        {
            //Wait one frame between checks
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //The instance has been set, Load Weapon Data
        LoadWeaponData();
    }

    //Weapon Data loader with a switch between enemies and players, better performance
    private void LoadWeaponData()
    {
        if (isPlayer)
        {
            LoadPlayerWeaponData();
        }
        else
        {
            LoadEnemyWeaponData();
        }
    }

    //Load Player weapons
    private void LoadPlayerWeaponData()
    {
        //Player
        WeaponCalls[WeaponData.Instance.PlayerPistol] = SpawnPistol;
        WeaponCalls[WeaponData.Instance.PlayerBirdshot] = SpawnBirdshot;
        WeaponCalls[WeaponData.Instance.PlayerBuckshot] = SpawnBuckshot;
        //Set default Weapon
        currWeapon = WeaponData.Instance.PlayerPistol;
    }

    //Load Enemies default weapons
    private void LoadEnemyWeaponData()
    {
        //Enemies
        WeaponCalls[WeaponData.Instance.defaultEnemPistol] = SpawnPistol;
        //Set default weapon
        currWeapon = WeaponData.Instance.defaultEnemPistol;
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

    /* -----------------------------------------------------------------------
                                SHOOT SCRIPT API

        This section contains all relevant functions that should be
        accessed by other objects, like for example the player controller.
      ----------------------------------------------------------------------- */

    //Local variables
    private Weapon currWeapon;
    private bool isPlayer;

    //Dictionary of weapon function calls
    private Dictionary<Weapon, Action> WeaponCalls = new();

    //The purpose of this function is to improve travel time in this script
    private void APIAwake()
    {
        //Sets the layer of the bullet depending on if its the player shooting or the enemy
        switch (this.gameObject.tag)
        {
            case "Player":
                //The player is shooting
                isPlayer = true;
                projectileLayer = LayerMask.NameToLayer("Projectiles Player");
                break;
            case "Enemy":
                //The Enemy is shooting
                isPlayer = false;
                projectileLayer = LayerMask.NameToLayer("Projectiles Enemies");
                break;
            default:
                Debug.Log("ERORR!!! AN OBJECT THAT IS SHOOTING IS UNTAGGED AND/OR UNDEFINED IN SHOOT.CS");
                break;
        }
    }

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
    public void AddWeaponToShoot(Weapon weapon, int spawningBehaviour)
    {
        /* Number Index:
         * 1 = Pistol Behaviour
         * 2 = Birdshot Behaviour
         * 3 = Buckshot Behaviour
         */

        //First check if this weapon is already on the dictionary, to prevent errors
        if (WeaponCalls.ContainsKey(weapon))
        {
            Debug.Log("ERROR! WEAPON CALLS DICTIONARY ALREADY HAD A WEAPON OF THIS TYPE");
            Debug.Log("You tried to add weapon: " + weapon.GetName());
            return;
        }

        //Else, assign it a spawning behaviour
        //FIXME: Should we use enums for these objects?
        switch (spawningBehaviour)
        {
            case 1:
                WeaponCalls[weapon] = SpawnPistol;
                break;
            case 2:
                WeaponCalls[weapon] = SpawnBirdshot;
                break;
            case 3:
                WeaponCalls[weapon] = SpawnBuckshot;
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

    /* -----------------------------------------------------------------------
                        BEHAVIOUR OF SPAWNED PROJECTILES

        This section contains the behaviour of all bullets/laser when they
        spawn. Should be able to spawn single, double, triple bullets
        Bullets that slow or speed up, anything design needs/wants
      ----------------------------------------------------------------------- */

    //Local Variables
    private int projectileLayer;

    //Stores all projectiles here, allows to colapse all projectiles within an empty object 
    private GameObject projectileContainer;

    //The purpose of this function is to improve travel time in this script
    private void BehaviourAwake()
    {
        //Stores projectiles in "Container For Projectiles"
        projectileContainer = GameObject.Find("Container For Projectiles"); 
    }

    /// <summary> Pistol: only shoots 1 projectile </summary>
    private void SpawnPistol()
    {
        //Create the Projectile
        InstantiateProjectile(currWeapon);
    }

    /// <summary> Birdshot: fan style shot -> \ | / </summary>
    private void SpawnBirdshot()
    {
        //Create the Projectile
        InstantiateProjectile(currWeapon, 30f);
        InstantiateProjectile(currWeapon, -30f);
        InstantiateProjectile(currWeapon, 0, 1);
    }

    /// <summary> Buckshot: 3 separated but same direction shots </summary>
    private void SpawnBuckshot()
    {
        //Create the Projectile
        InstantiateProjectile(currWeapon, 0, 1);
        InstantiateProjectile(currWeapon, 2, -2);
        InstantiateProjectile(currWeapon, -2, -2);
    }

    //Overloaded Functions, Spawns the projectile

    /// <summary> Shoot Straight </summary>
    private void InstantiateProjectile(Weapon weapon)
    {
        //Create a quaternion with only the Z axis
        Quaternion zRotation = Quaternion.Euler(0f, 0f, this.transform.rotation.eulerAngles.z);

        //Spawn projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, this.transform.position, zRotation, projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, weapon.speed, weapon.damage, zRotation);
    }

    /// <summary> Add a float to the angle (In degrees) </summary>
    private void InstantiateProjectile(Weapon weapon, float addedAngle)
    {
        //Create Rotation Offset
        Quaternion modifiedRotation = Quaternion.Euler(0f, 0f, this.transform.rotation.eulerAngles.z + addedAngle);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, this.transform.position, modifiedRotation, projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, weapon.speed, weapon.damage, modifiedRotation);
    }

    /// <summary>
    /// Offset Spawning point
    /// | HACK: If the ship rotates on any non-z axis, bullets come out weird
    /// </summary>
    private void InstantiateProjectile(Weapon weapon, float offsetX, float offsetY)
    {
        //Create a quaternion with only the Z axis
        Quaternion zRotation = Quaternion.Euler(0f, 0f, this.transform.rotation.eulerAngles.z);

        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0f);

        //Translate Vector to global space
        Vector3 overridenPosition = this.transform.TransformPoint(point);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, overridenPosition, zRotation, projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, weapon.speed, weapon.damage, zRotation);
    }

    //HACK: If the ship rotates on any non-z axis, bullets come out weird
    /// <summary> Add to angle and offset spawn point </summary>
    private void InstantiateProjectile(Weapon weapon, float offsetX, float offsetY, float addedAngle)
    {
        //Position Offset -----------------------
        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0f);

        //Translate Vector to global space
        Vector3 overridenPosition = this.transform.TransformPoint(point);

        // Create Rotation Offset ---------------
        Quaternion modifiedRotation = Quaternion.Euler(0f, 0f, this.transform.rotation.eulerAngles.z + addedAngle);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, overridenPosition, modifiedRotation, projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, weapon.speed, weapon.damage, modifiedRotation);

    }

}
