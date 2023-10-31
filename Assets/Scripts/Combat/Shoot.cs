using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Script that allows any object to shoot projectiles
public class Shoot : MonoBehaviour
{
    //List to access the prefab objects of all projectile models
    [Header("Projectile Prefab Data")]
    [SerializeField] private GameObject DataPrefab;

    [Header("Weapon Data")]
    [SerializeField] private List<string> WeaponChoices;
    [SerializeField] private string defaultWeaponName;

    [Header("Current Weapon Info")]
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    //NOTE: In either the player controller or enemy AI script,
    //Both have to define projectile speed and damages
    //Should allow to make more difficult enemies

    [Header("Testing Variables")]
    public bool TestShoot = false;

    private void Awake()
    {
        // Script API Section -----------------------------------
        SetWeaponFire(defaultWeaponName);

        // Behaviour Section ------------------------------------
        BulletData = DataPrefab.GetComponent<ProjectilePrefabList>(); //Get data list
        projectileContainer = GameObject.Find("Container For Projectiles"); //Stores projectiles in "Container For Projectiles"

        //Sets the layer of the bullet depending on if its the player shooting or the enemy
        switch (this.gameObject.tag)
        {
            case "Player":
                //The player is shooting
                projectileLayer = LayerMask.NameToLayer("Projectiles Player");
                break;
            case "Enemy":
                //The Enemy is shooting
                projectileLayer = LayerMask.NameToLayer("Projectiles Enemies");
                break;
            default:
                Debug.Log("ERORR!!! AN OBJECT THAT IS SHOOTING IS UNTAGGED AND/OR UNDEFINED IN SHOOT.CS");
                break;
        }

        //Populate the dictionary with Weapon behaviors
        projectileBehaviors["Pistol"] = SpawnPistol;
        projectileBehaviors["Birdshot"] = SpawnBirdshot;
        projectileBehaviors["Buckshot"] = SpawnBuckshot;
    }

    private void Update()
    {
        //HACK: Testing loop
        if (TestShoot)
        {
            ShootProjectile(1,30f);
            TestShoot = false;
        }
    }

    /* -----------------------------------------------------------------------
                                SHOOT SCRIPT API

        This section contains all relevant functions that should be
        accessed by other objects, like for example the player controller.
      ----------------------------------------------------------------------- */

    //Local variables
    private string sCurrWeapon;

    //Dictionary of function calls
    private Dictionary<string, Action> projectileBehaviors = new();

    //Assign Weapon that is currently being shot
    public void SetWeaponFire(string firingType)
    {
        //NOTE: The string must have something contained within the name of the prefab
        //So if you input "Pistol", it should set the weapon to Pistol.
        //It has to be in the list of available weapons, set on the inspector window
        int i;

        //Go through list, find if firingType is in it
        for (i = 0; i < WeaponChoices.Count; i++)
        {
            if (WeaponChoices[i].Equals(firingType))
            {
                sCurrWeapon = WeaponChoices[i];
                return;
            }
        }
        Debug.Log("ERROR! firingType String passed in Shoot.cs wasnt found within the available firing choices");
    }

    //Function to call to make the object shoot
    //It also creates the bullet with its designed behaviour
    public void ShootProjectile(int damage, float bulletSpeed)
    {
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
        
        if (projectileBehaviors.ContainsKey(sCurrWeapon))
        {
            projectileBehaviors[sCurrWeapon].Invoke();
        }
        else
        {
            Debug.Log("ERROR! UNEXPECTED BEHAVIOR TRIGGERED ON SHOOT.CS/API");
        }
    }

    /* -----------------------------------------------------------------------
                        BEHAVIOUR OF SPAWNED PROJECTILES

        This section contains the behaviour of all bullets/laser when they
        spawn. Should be able to spawn single, double, triple bullets
        Bullets that slow or speed up, anything design needs/wants
      ----------------------------------------------------------------------- */

    //Local Variables
    private GameObject firedProjectile;
    private ProjectileObject projectileData;
    private ProjectilePrefabList BulletData;
    private int projectileLayer;

    //Stores all projectiles here, allows to colapse all projectiles within an empty object
    private GameObject projectileContainer;

    //Pistol: only shoots 1 projectile
    private void SpawnPistol()
    {
        //Create the Projectile
        InstantiateProjectile(BulletData.RedBullet, this.damage, this.bulletSpeed);
    }

    // Birdshot: fan style shot -> \ | /
    private void SpawnBirdshot()
    {
        //Create the Projectile
        InstantiateProjectile(BulletData.YellowBullet, this.damage, this.bulletSpeed, 30f);
        InstantiateProjectile(BulletData.YellowBullet, this.damage, this.bulletSpeed, -30f);
        InstantiateProjectile(BulletData.YellowBullet, this.damage, this.bulletSpeed, 0, 1);
    }

    //Buckshot: 3 separated but same direction shots
    private void SpawnBuckshot()
    {
        //Create the Projectile
        InstantiateProjectile(BulletData.PinkBullet, this.damage, this.bulletSpeed, 0, 1);
        InstantiateProjectile(BulletData.PinkBullet, this.damage, this.bulletSpeed, 2, -2);
        InstantiateProjectile(BulletData.PinkBullet, this.damage, this.bulletSpeed, -2, -2);
    }

    //Overloaded Function, Spawns the projectile

    //Shoot straight
    private void InstantiateProjectile(GameObject bullet, int damage, float speed)
    {
        //Create a quaternion with only the Z axis
        Quaternion zRotation = Quaternion.Euler(0f, 0f, this.transform.rotation.eulerAngles.z);

        //Spawn projectile
        firedProjectile = Instantiate(bullet, this.transform.position, zRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, speed, damage, zRotation);
    }

    //Add a float to the angle (In degrees)
    private void InstantiateProjectile(GameObject bullet, int damage, float speed, float addedAngle)
    {
        //Create Rotation Offset
        Quaternion modifiedRotation = Quaternion.Euler(0f, 0f, this.transform.rotation.eulerAngles.z + addedAngle);

        //Spawn Projectile
        firedProjectile = Instantiate(bullet, this.transform.position, modifiedRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, speed, damage, modifiedRotation);
    }

    //offset Spawning point
    //HACK: If the ship rotates on any non-z axis, bullets come out weird
    private void InstantiateProjectile(GameObject bullet, int damage, float speed, float offsetX, float offsetY)
    {
        //Create a quaternion with only the Z axis
        Quaternion zRotation = Quaternion.Euler(0f, 0f, this.transform.rotation.eulerAngles.z);

        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0f);

        //Translate Vector to global space
        Vector3 overridenPosition = this.transform.TransformPoint(point);

        //Spawn Projectile
        firedProjectile = Instantiate(bullet, overridenPosition, zRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, speed, damage, zRotation);
    }

    //add to angle and offset spawn point
    //HACK: If the ship rotates on any non-z axis, bullets come out weird
    private void InstantiateProjectile(GameObject bullet, int damage, float speed, float offsetX, float offsetY, float addedAngle)
    {
        //Position Offset -----------------------
        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0f);

        //Translate Vector to global space
        Vector3 overridenPosition = this.transform.TransformPoint(point);

        // Create Rotation Offset ---------------
        Quaternion modifiedRotation = Quaternion.Euler(0f, 0f, this.transform.rotation.eulerAngles.z + addedAngle);

        //Spawn Projectile
        firedProjectile = Instantiate(bullet, overridenPosition, modifiedRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, projectileLayer, speed, damage, modifiedRotation);

    }

}
