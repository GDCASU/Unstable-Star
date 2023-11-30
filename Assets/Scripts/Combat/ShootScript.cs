using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> Component that allows any object to shoot projectiles </summary>
public class ShootScript : ScriptableObject
{
    //Local Variables
    private int projectileLayer;
    private GameObject AnchorObject;

    /// <summary> Creates a ShootScript Object </summary>
    public static ShootScript CreateInstance(GameObject weaponAnchor)
    {
        ShootScript newScript = ScriptableObject.CreateInstance<ShootScript>();
        newScript.InitializeData(weaponAnchor);
        return newScript;
    }

    private void InitializeData(GameObject Anchor)
    {
        //Set anchor
        AnchorObject = Anchor;

        //Sets the layer of the bullet depending on if its the player shooting or the enemy
        switch (Anchor.tag)
        {
            case "Player":
                //The player is shooting
                projectileLayer = PhysicsConfig.Get.ProjectilesPlayer;
                break;
            case "Enemy":
                //The Enemy is shooting
                projectileLayer = PhysicsConfig.Get.ProjectilesEnemies;
                break;
            default:
                Debug.Log("ERORR!!! AN OBJECT THAT IS SHOOTING IS UNTAGGED AND/OR UNDEFINED IN SHOOTSCRIPT.CS");
                break;
        }
    }

    /// <summary> Makes the object shoot its current weapon </summary>
    public void ShootWeapon(Weapon inputWeapon)
    {
        switch (inputWeapon.behaviour)
        {
            case BehaviourTypes.SingleShot:
                SingleShotBehaviour(inputWeapon);
                break;
            case BehaviourTypes.TripleOffset:
                TripleOffsetBehaviour(inputWeapon);
                break;
            case BehaviourTypes.FanShot:
                FanShotBehaviour(inputWeapon);
                break;
            case BehaviourTypes.Gatling:
                // TODO: Implement here (?)
                break;
            default:
                Debug.LogError("ERROR! Weapon Behaviour Instruction undefined/not implemented, thrown in ShootScript.cs");
                break;
        }
    }

    #region PROJECTILE BEHAVIOURS

    /* -----------------------------------------------------------------------
                        BEHAVIOUR OF SPAWNED PROJECTILES

        This section contains the behaviour of all bullets/laser when they
        spawn. Should be able to spawn single, double, triple bullets
        Bullets that slow or speed up, anything design needs/wants
      ----------------------------------------------------------------------- */

    /// <summary> Only shoots 1 projectile </summary>
    private void SingleShotBehaviour(Weapon weapon)
    {
        //Create the Projectile
        Default(weapon);
    }

    /// <summary> spawns the bullet in a fan style shot -> \ | / </summary>
    private void FanShotBehaviour(Weapon weapon)
    {
        //Create the Projectile
        AddedAngle(weapon, 30f);
        AddedAngle(weapon, -30f);
        WithOffset(weapon, 0, 1);
    }

    /// <summary> TripleOffsetBehaviour: 3 separated but same direction shots </summary>
    private void TripleOffsetBehaviour(Weapon weapon)
    {
        //Create the Projectile
        WithOffset(weapon, 0, 1);
        WithOffset(weapon, 2, -2);
        WithOffset(weapon, -2, -2);
    }

    #endregion

    #region INSTANTIATION FUNCTIONS

    // Overloaded Functions, Spawns the projectile
    // NOTE: The bullets spawn where the anchor points is located, this could be fixed with an offset
    // If necessary, but the entities use long capsule colliders, so its fine now

    /// <summary> Shoot Straight </summary>
    private void Default(Weapon weapon)
    {
        //Create a quaternion with only the Z axis
        Quaternion zRotation = ComputeRotation();

        //Spawn projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, AnchorObject.transform.position, zRotation, WeaponData.Instance.projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary> Add a float to the angle (In degrees) </summary>
    private void AddedAngle(Weapon weapon, float addedAngle)
    {
        //Create Rotation Offset
        Quaternion modifiedRotation = ComputeRotation(addedAngle);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, AnchorObject.transform.position, modifiedRotation, WeaponData.Instance.projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary>
    /// Offset Spawning point <para />
    /// HACK: If the gameObject rotates on any non-z axis, bullets come out weird
    /// </summary>
    private void WithOffset(Weapon weapon, float offsetX, float offsetY)
    {
        //Create a quaternion with only the Z axis
        Quaternion zRotation = ComputeRotation();

        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0f);

        //Translate Vector to global space
        Vector3 overridenPosition = AnchorObject.transform.TransformPoint(point);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, overridenPosition, zRotation, WeaponData.Instance.projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary> Add to angle and offset spawn point <para />
    /// HACK: If the gameObject rotates on any non-z axis, bullets come out weird
    /// </summary>
    private void AngleAndOffset(Weapon weapon, float offsetX, float offsetY, float addedAngle)
    {
        //Position Offset -----------------------
        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0f);

        //Translate Vector to global space
        Vector3 overridenPosition = AnchorObject.transform.TransformPoint(point);

        // Create Rotation Offset ---------------
        Quaternion modifiedRotation = ComputeRotation(addedAngle);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, overridenPosition, modifiedRotation, WeaponData.Instance.projectileContainer.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);

    }

    // Helper method for creating the quaternion of rotation.
    // Hopefully makes it easier to read
    private Quaternion ComputeRotation(float addedAngle = 0f)
    {
        float angle = AnchorObject.transform.rotation.eulerAngles.z + addedAngle;
        Quaternion newRotation = Quaternion.Euler(0f, 0f, angle);
        return newRotation;
    }

    #endregion
}