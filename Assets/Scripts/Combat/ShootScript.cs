using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> Component that allows any object to shoot projectiles </summary>
public class ShootScript : MonoBehaviour
{
    //Local Variables
    private int projectileLayer;
    private bool onShootingCooldown;
    private GameObject AnchorObject;
    public float bounds = 1.5f;
    public float addedOffset = 0.1f;
    public float currOffset = 0f;

    // Any entity that fires needs to set the anchor point of their weapons.
    // So within their respective entities script. do [shootComponent.InitializeData(WeaponAnchorObj);]
    // NOTE: This also means all weapons fire from the same spot unless offseted, but this can be fixed if necessary
    public void InitializeData(GameObject Anchor)
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
        if (onShootingCooldown)
        {
            return;
        }
        
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
                GatlingGunBehaviour(inputWeapon);
                break;
            default:
                Debug.LogError("ERROR! Weapon Behaviour Instruction undefined/not implemented, thrown in ShootScript.cs");
                break;
        }

        //Start cooldown between shoots of current weapon
        StartCoroutine(ShootingCooldown(inputWeapon.shootCooldown));
    }

    private IEnumerator ShootingCooldown(float time)
    {
        onShootingCooldown = true;
        yield return new WaitForSeconds(time);
        onShootingCooldown = false;
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
        DefaultSpawn(weapon);
    }

    /// <summary> spawns the bullet in a fan style shot -> \ | / </summary>
    private void FanShotBehaviour(Weapon weapon)
    {
        //Create the Projectile
        AddedAngleSpawn(weapon, 30f);
        AddedAngleSpawn(weapon, -30f);
        OffsetSpawn(weapon, 0, 1);
    }

    /// <summary> TripleOffsetBehaviour: 3 separated but same direction shots </summary>
    private void TripleOffsetBehaviour(Weapon weapon)
    {
        //Create the Projectile
        OffsetSpawn(weapon, 0, 1);
        OffsetSpawn(weapon, 2, -2);
        OffsetSpawn(weapon, -2, -2);
    }
    /// <summary> Shoots projectiles very fast (configured when creating the class) with offset that is constantly changing</summary>
    private void GatlingGunBehaviour(Weapon weapon)
    {
        //Create the Projectile
        if(currOffset >= bounds || currOffset <= -bounds){
            addedOffset *= -1;
        }
        WithOffset(weapon, currOffset, 0);
        currOffset += addedOffset;
    }

    #endregion

    #region INSTANTIATION FUNCTIONS

    // Overloaded Functions, Spawns the projectile
    // NOTE: The bullets spawn where the anchor points is located, this could be changed with an offset if necessary

    /// <summary> Shoot Straight </summary>
    private void DefaultSpawn(Weapon weapon)
    {
        //Create a quaternion with only the Z axis
        Quaternion zRotation = ComputeRotation();

        //Spawn projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, AnchorObject.transform.position, zRotation, WeaponData.Instance.ContainerTransform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary> Add a float to the angle (In degrees) </summary>
    private void AddedAngleSpawn(Weapon weapon, float addedAngle)
    {
        //Create Rotation Offset
        Quaternion modifiedRotation = ComputeRotation(addedAngle);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, AnchorObject.transform.position, modifiedRotation, WeaponData.Instance.ContainerTransform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary>
    /// Offset Spawning point <para />
    /// HACK: If the gameObject rotates on any non-z axis, bullets come out weird
    /// </summary>
    private void OffsetSpawn(Weapon weapon, float offsetX, float offsetY)
    {
        //Create a quaternion with only the Z axis
        Quaternion zRotation = ComputeRotation();

        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0f);

        //Translate Vector to global space
        Vector3 overridenPosition = AnchorObject.transform.TransformPoint(point);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, overridenPosition, zRotation, WeaponData.Instance.ContainerTransform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary> Add to angle and offset spawn point <para />
    /// HACK: If the gameObject rotates on any non-z axis, bullets come out weird
    /// </summary>
    private void AngleAndOffsetSpawn(Weapon weapon, float offsetX, float offsetY, float addedAngle)
    {
        //Position Offset -----------------------
        //Create Vector with offset on local space
        Vector3 point = new Vector3(offsetX, offsetY, 0f);

        //Translate Vector to global space
        Vector3 overridenPosition = AnchorObject.transform.TransformPoint(point);

        // Create Rotation Offset ---------------
        Quaternion modifiedRotation = ComputeRotation(addedAngle);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, overridenPosition, modifiedRotation, WeaponData.Instance.ContainerTransform);
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