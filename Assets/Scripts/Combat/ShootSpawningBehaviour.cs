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

    //TODO: Temp description >> Pistol-like laser, medium speed, only shoots 1
    private void SpawnRedLaser()
    {
        //Red Laser Data
        float speed = 12;

        //Create the Projectile
        InstantiateProjectile(laserData.RedLaser, speed);
    }

    //Triple 30 degree shot
    private void SpawnYellowLaser()
    {
        //Yellow Laser Data
        float speed = 12;

        //Create the Projectile
        //InstantiateProjectile(laserData.YellowLaser, speed);
        InstantiateProjectile(laserData.YellowLaser, speed, 30f);
        InstantiateProjectile(laserData.YellowLaser, speed, -30f);
    }

    //Fast laser, 3 same direction shots
    private void SpawnPinkLaser()
    {
        //Pink Laser Data
        float speed = 12;

        //Create the Projectile
        InstantiateProjectile(laserData.PinkLaser, speed);
        InstantiateProjectile(laserData.PinkLaser, speed, 1, -1);
        InstantiateProjectile(laserData.PinkLaser, speed, -1, -1);
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
        //Create Spawn Offset
        //FIXME: Offset doesnt work properly when its creator rotates
        Vector3 overridenPosition = new Vector3(creatorPosition.x + offsetX, creatorPosition.y + offsetY, creatorPosition.z);

        //Spawn Projectile
        firedProjectile = Instantiate(laser, overridenPosition, creatorRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, speed, creatorRotation);
    }

    //add to angle and offset spawn point
    private void InstantiateProjectile(GameObject laser, float speed, float offsetX, float offsetY, float addedAngle)
    {
        //Create Spawn Offset
        //FIXME: Offset doesnt work properly when its creator rotates
        Vector3 overridenPosition = new Vector3(creatorPosition.x + offsetX, creatorPosition.y + offsetY, creatorPosition.z);

        //Create Rotation Offset
        //Angle is multiplied by -1 so the rotation in game world makes logical sense
        Quaternion modifiedRotation = creatorRotation * Quaternion.Euler(0, 0, (-1 * addedAngle));

        //Spawn Projectile
        firedProjectile = Instantiate(laser, overridenPosition, modifiedRotation, projectileContainer.transform);
        projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(this.tag, speed, modifiedRotation);

    }
}
