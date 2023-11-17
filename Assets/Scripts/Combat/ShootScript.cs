using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> Component that allows any object to shoot projectiles </summary>
public class ShootScript : ScriptableObject
{
    //Local Variables
    private Dictionary<string, Action> WeaponDictionary = new();
    private int projectileLayer;
    private GameObject projectileContainer; //Projectile storage in hierarchy
    private GameObject AnchorObject;
    private Weapon weaponReference;

    /// <summary> Creates a ShootScript Object </summary>
    public static ShootScript CreateInstance(GameObject weaponAnchor)
    {
        ShootScript newScript = ScriptableObject.CreateInstance<ShootScript>();
        newScript.InitializeData(weaponAnchor);
        return newScript;
    }

    private void InitializeData(GameObject Anchor)
    {
        //Sets the projectile container, which WeaponData finds at Awake
        projectileContainer = WeaponData.Instance.projectileContainer;

        //Set anchor
        AnchorObject = Anchor;

        //Sets the layer of the bullet depending on if its the player shooting or the enemy
        switch (this.AnchorObject.tag)
        {
            case "Player":
                //The player is shooting
                projectileLayer = PhysicsConfig.Instance.ProjectilesPlayer;
                break;
            case "Enemy":
                //The Enemy is shooting
                projectileLayer = PhysicsConfig.Instance.ProjectilesEnemies;
                break;
            default:
                Debug.Log("ERORR!!! AN OBJECT THAT IS SHOOTING IS UNTAGGED AND/OR UNDEFINED IN SHOOT.CS");
                break;
        }

        //Add all the different spawning behaviours to the dictionary
        //NOTE: All the functions in "Spawning Behaviours" Must be present here
        WeaponDictionary["SingleShot"] = SingleShotBehaviour;
        WeaponDictionary["FanShot"] = FanShotBehaviour;
        WeaponDictionary["TripleOffset"] = TripleOffsetBehaviour;
    }

    /// <summary> Makes the object shoot its current weapon </summary>
    public void ShootWeapon(Weapon inputWeapon)
    {
        weaponReference = inputWeapon;
        WeaponDictionary[weaponReference.BehaviourSpawnType].Invoke();
    }

    #region SPAWNING BEHAVIOURS

    /* -----------------------------------------------------------------------
                        BEHAVIOUR OF SPAWNED PROJECTILES

        This section contains the behaviour of all bullets/laser when they
        spawn. Should be able to spawn single, double, triple bullets
        Bullets that slow or speed up, anything design needs/wants
      ----------------------------------------------------------------------- */

    /// <summary> Only shoots 1 projectile </summary>
    private void SingleShotBehaviour()
    {
        //Create the Projectile
        InstantiateProjectile(weaponReference);
    }

    /// <summary> spawns the bullet in a fan style shot -> \ | / </summary>
    private void FanShotBehaviour()
    {
        //Create the Projectile
        InstantiateProjectile(weaponReference, 30f);
        InstantiateProjectile(weaponReference, -30f);
        InstantiateProjectile(weaponReference, 0, 1);
    }

    /// <summary> TripleOffsetBehaviour: 3 separated but same direction shots </summary>
    private void TripleOffsetBehaviour()
    {
        //Create the Projectile
        InstantiateProjectile(weaponReference, 0, 1);
        InstantiateProjectile(weaponReference, 2, -2);
        InstantiateProjectile(weaponReference, -2, -2);
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
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary> Add a float to the angle (In degrees) </summary>
    private void InstantiateProjectile(Weapon weapon, float addedAngle)
    {
        //Create Rotation Offset
        Quaternion modifiedRotation = computeRotation(addedAngle);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, AnchorObject.transform.position, modifiedRotation, projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);
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
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);
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
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);

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