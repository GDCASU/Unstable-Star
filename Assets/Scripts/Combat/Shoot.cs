using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    //Dictionary to access the prefab objects of all projectile types
    [Header("Laser Data GameObject")]
    [SerializeField] private GameObject DataPrefab;

    [Header("Laser Prefabs")]
    [SerializeField] private List<string> laserChoices;
    [SerializeField] private string defaultLaserName;
    //TODO: Figure out how to make a selection list of choices instead of an input list

    [Header("Testing Variables")]
    public bool spawn = false;

    private void Awake()
    {
        //Script API Section
        SetLaserFire(defaultLaserName);

        //Behaviour Section
        laserData = DataPrefab.GetComponent<LaserPrefabList>(); //Get data list
        projectileContainer = GameObject.Find("Container For Projectiles"); //Stores projectiles in "Container For Projectiles"
    }

    private void Update()
    {
        //HACK: Testing loop
        if (spawn)
        {
            ShootProjectile();
            spawn = false;
        }
    }

    /* -----------------------------------------------------------------------
                                SHOOT SCRIPT API

        This section contains all relevant functions that should be
        accessed by other objects, like for example the input system.
      -----------------------------------------------------------------------
    */


    //Local variables
    private string sCurrLaser;

    //Assign Laser that is currently being shot
    public void SetLaserFire(string laserType)
    {
        //NOTE: The string must have something contained within the name of the prebab
        //So if you input "Red", it should set the Red Laser.
        //It has to also bee in the list of available lasers, set on the inspector window
        int i;

        //Go through list, find laserType
        for (i = 0; i < laserChoices.Count; i++)
        {
            if (laserChoices[i].Equals(laserType))
            {
                sCurrLaser = laserChoices[i];
                return;
            }
        }
        Debug.Log("ERROR! LaserType String passed in Shoot.cs wasnt found within the available lasers");
    }

    //Function to call to make the object shoot
    //It also creates the laser with its designed behaviour
    public void ShootProjectile()
    {
        BehaviourSpawn(sCurrLaser);
    }


    /* -----------------------------------------------------------------------
                        BEHAVIOUR OF SPAWNED PROJECTILES

        This section contains the behaviour of all bullets/laser when they
        spawn. Should be able to spawn single, double, triple bullets
        Bullets that slow or speed up, anything design needs/wants
      -----------------------------------------------------------------------
    */


    //Local Variables
    private GameObject firedProjectile;
    private ProjectileObject projectileData;
    private LaserPrefabList laserData;

    //Stores all projectiles here, allows to colapse all projectiles within an empty object
    private GameObject projectileContainer;

    //Calls their bullet function for spawning
    public void BehaviourSpawn(string sLaserType)
    {
        //Select The laser Type
        switch (sLaserType)
        {
            case "Pistol":
                SpawnPistol();
                break;
            case "Birdshot":
                SpawnBirdshot();
                break;
            case "Buckshot":
                SpawnBuckshot();
                break;
            default:
                Debug.Log("ERROR! UNEXPECTED BEHAVIOUR TRIGGERED ON ShootSpawningBehaviour");
                break;
        }

    }

    //TODO: Temp description >> Pistol-like laser, only shoots 1 projectile
    private void SpawnPistol()
    {
        //Pistol Data
        float speed = 20;
        int damage = 1;

        //Create the Projectile
        InstantiateProjectile(laserData.PistolBullet, damage, speed);
    }

    // Birdshot: fan style shot -> \ | /
    private void SpawnBirdshot()
    {
        //Birdshot Data
        float speed = 20;
        int damage = 1;

        //Create the Projectile
        InstantiateProjectile(laserData.BirdshotBullet, damage, speed, 30f);
        InstantiateProjectile(laserData.BirdshotBullet, damage, speed, -30f);
        InstantiateProjectile(laserData.BirdshotBullet, damage, speed, 0, 1);
    }

    //Buckshot laser, 3 separated but same direction shots
    private void SpawnBuckshot()
    {
        //Buckshot Data
        float speed = 20;
        int damage = 1;

        //Create the Projectile
        InstantiateProjectile(laserData.BuckshotBullet, damage, speed, 0, 1);
        InstantiateProjectile(laserData.BuckshotBullet, damage, speed, 2, -2);
        InstantiateProjectile(laserData.BuckshotBullet, damage, speed, -2, -2);
    }

    /* Overloaded Function
     * Spawns Projectile, do not call directly, only within the LaserTypes functions
     * HACK: If the ship rotates in any non-z axis, bullets come out weird, but works for now
     * Note: Is it neccessary to add the Z value? if the player cant move away or closer to the camera
     */

    //Shoot straight
    private void InstantiateProjectile(GameObject laser, int damage, float speed)
    {
        firedProjectile = Instantiate(laser, this.transform.position, this.transform.rotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, speed, damage, this.transform.rotation);
    }

    //Add a float to the angle (In degrees)
    private void InstantiateProjectile(GameObject laser, int damage, float speed, float addedAngle)
    {
        //Create Rotation Offset
        //Angle is multiplied by -1 so the rotation in game world makes logical sense
        Quaternion modifiedRotation = this.transform.rotation * Quaternion.Euler(0, 0, (-1 * addedAngle));

        //Spawn Projectile
        firedProjectile = Instantiate(laser, this.transform.position, modifiedRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, speed, damage, modifiedRotation);
    }

    //offset Spawning point
    private void InstantiateProjectile(GameObject laser, int damage, float speed, float offsetX, float offsetY)
    {
        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY);

        //Translate Vector to global space
        Vector3 overridenPosition = this.transform.TransformPoint(point);

        //Spawn Projectile
        firedProjectile = Instantiate(laser, overridenPosition, this.transform.rotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, speed, damage, this.transform.rotation);
    }

    //add to angle and offset spawn point
    private void InstantiateProjectile(GameObject laser, int damage, float speed, float offsetX, float offsetY, float addedAngle)
    {
        //Position Offset -----------------------
        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY);

        //Translate Vector to global space
        Vector3 overridenPosition = this.transform.TransformPoint(point);

        // Create Rotation Offset ---------------
        //Angle is multiplied by -1 so the rotation in game world makes logical sense
        Quaternion modifiedRotation = this.transform.rotation * Quaternion.Euler(0, 0, (-1 * addedAngle));

        //Spawn Projectile
        firedProjectile = Instantiate(laser, overridenPosition, modifiedRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, speed, damage, modifiedRotation);

    }

}
