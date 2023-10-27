using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script contains the behaviour of all bullets when they spawn.
 * Should be able to spawn single, double, triple bullets
 * Bullets that slow or speed up, anything design needs/wants
 */
public class ShootSpawningBehaviour : MonoBehaviour
{
    [Header("Laser Data GameObject")]
    [SerializeField] private GameObject DataPrefab;

    //Local Variables
    private GameObject firedProjectile;
    private ProjectileObject projectileData;
    private LaserPrefabList laserData;

    //Stores all projectiles here, allows to colapse all projectiles within an empty object
    private GameObject projectileContainer;

    //Creator Data
    private Vector3 creatorPosition;
    private Quaternion creatorRotation;
    private Vector3 creatorLocalPosition;
    private Transform creatorTransform;

    //Get Data List
    private void Awake()
    {
        laserData = DataPrefab.GetComponent<LaserPrefabList>();
        projectileContainer = GameObject.Find("Container For Projectiles");
    }

    //Keeps track of the position and transform of the creator
    private void Update()
    {
        this.creatorPosition = this.transform.position;
        this.creatorRotation = this.transform.rotation;
        this.creatorLocalPosition = this.transform.localPosition;
    }


    //Calls their bullet function for spawning
    public void BehaviourSpawn(string sLaserType)
    {
        //Select The laser Type
        switch (sLaserType)
        {
            case "Red":
                SpawnRedLaser();
                break;
            case "Yellow":
                SpawnYellowLaser();
                break;
            case "Pink":
                SpawnPinkLaser();
                break;
            default:
                Debug.Log("ERROR! UNEXPECTED BEHAVIOUR TRIGGERED ON ShootSpawningBehaviour");
                break;
        }

    }

    //TODO: Temp description >> Pistol-like laser, only shoots 1
    private void SpawnRedLaser()
    {
        //Red Laser Data
        float speed = 12;

        //Create the Projectile
        InstantiateProjectile(laserData.RedLaser, speed);
    }

    //fan style shot -> \ | /
    private void SpawnYellowLaser()
    {
        //Yellow Laser Data
        float speed = 12;

        //Create the Projectile
        //InstantiateProjectile(laserData.YellowLaser, speed);
        InstantiateProjectile(laserData.YellowLaser, speed, 30f);
        InstantiateProjectile(laserData.YellowLaser, speed, -30f);
        InstantiateProjectile(laserData.YellowLaser, speed, 0, 1);
    }

    //Buckshot laser, 3 same direction shots
    private void SpawnPinkLaser()
    {
        //Pink Laser Data
        float speed = 12;

        //Create the Projectile
        InstantiateProjectile(laserData.PinkLaser, speed, 0, 1);
        InstantiateProjectile(laserData.PinkLaser, speed, 2, -2);
        InstantiateProjectile(laserData.PinkLaser, speed, -2, -2);
    }

    /* Overloaded Function
     * Spawns Projectile, do not call directly, only within the LaserTypes functions
     * HACK: If the ship rotates in any non-z axis, bullets come out weird, but works for now
     * Note: Is it neccessary to add the Z value? if the player cant move away or closer to the camera
     */

    //Shoot straight
    private void InstantiateProjectile(GameObject laser, float speed)
    {
        firedProjectile = Instantiate(laser, creatorPosition, creatorRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, speed, creatorRotation);
    }

    //Add a float to the angle (In degrees)
    private void InstantiateProjectile(GameObject laser, float speed, float addedAngle)
    {
        //Create Rotation Offset
        //Angle is multiplied by -1 so the rotation in game world makes logical sense
        Quaternion modifiedRotation = creatorRotation * Quaternion.Euler(0,0, (-1 * addedAngle) );

        //Spawn Projectile
        firedProjectile = Instantiate(laser, creatorPosition, modifiedRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, speed, modifiedRotation);
    }

    //offset Spawning point
    private void InstantiateProjectile(GameObject laser, float speed, float offsetX, float offsetY)
    {
        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0);

        //Translate Vector to global space
        Vector3 overridenPosition = this.transform.TransformPoint(point);

        //Spawn Projectile
        firedProjectile = Instantiate(laser, overridenPosition, creatorRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, speed, creatorRotation);
    }

    //add to angle and offset spawn point
    private void InstantiateProjectile(GameObject laser, float speed, float offsetX, float offsetY, float addedAngle)
    {
        //Position Offset -----------------------
        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0);

        //Translate Vector to global space
        Vector3 overridenPosition = this.transform.TransformPoint(point);

        // Create Rotation Offset ---------------
        //Angle is multiplied by -1 so the rotation in game world makes logical sense
        Quaternion modifiedRotation = creatorRotation * Quaternion.Euler(0, 0, (-1 * addedAngle));

        //Spawn Projectile
        firedProjectile = Instantiate(laser, overridenPosition, modifiedRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, speed, modifiedRotation);

    }
}
