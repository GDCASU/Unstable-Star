using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

/// <summary> Component that allows any object to shoot projectiles </summary>
public class ShootComponent : MonoBehaviour
{
    // Settings
    [Header("Settings")]
    [SerializeField] private GameObject AnchorObject;

    //Local Variables
    private int projectileLayer;
    private Coroutine gatlingRoutine = null;
    private Coroutine LaserRoutine;

    // Initial setup
    private void Start()
    {
        //Sets the layer of the bullet depending on if its the player shooting or the enemy
        switch (this.gameObject.tag)
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

    /// <summary> Makes the object shoot its current weapon, returns true if successful </summary>
    public bool ShootWeapon(Weapon inputWeapon)
    {
        // Dont shoot the weapon if its a null type
        if (inputWeapon.weaponType == WeaponTypes.NULL) return false;
        
        // Else fire with the programmed behaviour
        switch (inputWeapon.weaponType)
        {
            case WeaponTypes.Pistol:
                SingleShotBehaviour(inputWeapon);
                break;
            case WeaponTypes.Buckshot:
                TripleOffsetBehaviour(inputWeapon);
                break;
            case WeaponTypes.Birdshot:
                FanShotBehaviour(inputWeapon);
                break;
            case WeaponTypes.Gatling:
                GatlingBehaviour(inputWeapon);
                break;
            case WeaponTypes.Laser:
                LaserBehaviour(inputWeapon);
                break;
            default:
                Debug.LogError("ERROR! Weapon Behaviour Instruction undefined/not implemented, thrown in ShootComponent.cs");
                break;
        }

        return true;
    }

    // Weapon cooldown routine
    private IEnumerator ShootingCooldown(Weapon input)
    {
        input.isOnCooldown = true;
        float timePassed = input.shootCooldownTime;

        // Cooldown timer
        while (timePassed > 0f)
        {
            timePassed -= Time.deltaTime;
            input.timeLeftInCooldown = timePassed;
            yield return null; // Wait a frame
        }

        input.isOnCooldown = false;
        input.timeLeftInCooldown = 0f;
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
        // Dont fire if weapon is on cooldown
        if (weapon.isOnCooldown) return;
        //Create the Projectile
        DefaultSpawn(weapon);
        // Play its sound
        SoundManager.instance.PlaySound(weapon.sound);
        //Start cooldown
        StartCoroutine(ShootingCooldown(weapon));
    }

    /// <summary> spawns the bullet in a fan style shot -> \ | / </summary>
    private void FanShotBehaviour(Weapon weapon)
    {
        // Dont fire if weapon is on cooldown
        if (weapon.isOnCooldown) return;
        //Create the Projectile
        AddedAngleSpawn(weapon, 30f);
        AddedAngleSpawn(weapon, -30f);
        OffsetSpawn(weapon, 0, 1);
        // Play its sound
        SoundManager.instance.PlaySound(weapon.sound);
        //Start cooldown
        StartCoroutine(ShootingCooldown(weapon));
    }

    /// <summary> TripleOffsetBehaviour: 3 separated but same direction shots </summary>
    private void TripleOffsetBehaviour(Weapon weapon)
    {
        // Dont fire if weapon is on cooldown
        if (weapon.isOnCooldown) return;
        //Create the Projectile
        OffsetSpawn(weapon, 0, 1);
        OffsetSpawn(weapon, 2, -2);
        OffsetSpawn(weapon, -2, -2);
        // Play its sound
        SoundManager.instance.PlaySound(weapon.sound);
        //Start cooldown
        StartCoroutine(ShootingCooldown(weapon));
    }

    /// <summary> GatlingBehaviour: rapid fire shots that offset after each fire </summary>
    private void GatlingBehaviour(Weapon input)
    {
        // Dont enter routine if already running
        if (gatlingRoutine != null) return;

        // Check if player or enemy
        if (input.isEnemy)
        {
            gatlingRoutine = StartCoroutine(GatlingRoutine(input));
            return;
        }

        // Else, Its the player, start the windup and check for button release
        gatlingRoutine = StartCoroutine(GatlingRoutine(input));
        StartCoroutine(isShootingHeld(input));
    }

    // This Routine will be stopped if the button is released
    private IEnumerator GatlingRoutine(Weapon input)
    {
        float timeLeft = input.warmupTime;
        input.timeLeftInCooldown = 0f;

        // Warm up timer
        // TODO: Missing warm up sound
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            input.timeLeftInCooldown = timeLeft;
            yield return null;
        }
        input.timeLeftInCooldown = 0f;

        // Warmup finshed, start shooting until stopped
        WaitForSeconds shotsTime = new WaitForSeconds(input.shootCooldownTime);
        List<float> randOffsets = new List<float>();
        randOffsets.Add(-0.5f);
        randOffsets.Add(0.5f);
        float previousVal = 0;
        float currVal;
        int index;

        // Decide which loop to go to depending if player or enemy
        if (input.isEnemy) { goto isEnemyShooting; }

        // Else, do player routine
        // NOTE: if we want left to right movement, the list can be turned into a queue
        while (true)
        {
            // Get a random index
            index = UnityEngine.Random.Range(0, randOffsets.Count);
            // Get a value from a random offset and remove it from array
            currVal = randOffsets[index];
            randOffsets.RemoveAt(index);
            // Spawn a bullet with this offset
            OffsetSpawn(input, currVal, 0f);
            // Play sound
            SoundManager.instance.PlaySound(input.sound);
            // Add back previous value
            randOffsets.Add(previousVal);
            previousVal = currVal;
            // Wait for time between shots
            yield return shotsTime;
        }

        // Label for jumping
        isEnemyShooting:
        timeLeft = input.shootingStayTime;
        // FIXME: Does this do the timer correcly?
        while (timeLeft >= 0f)
        {
            // Get a random index
            index = UnityEngine.Random.Range(0, randOffsets.Count);
            // Get a value from a random offset and remove it from array
            currVal = randOffsets[index];
            randOffsets.RemoveAt(index);
            // Spawn a bullet with this offset
            OffsetSpawn(input, currVal, 0f);
            // Play sound
            SoundManager.instance.PlaySound(input.sound);
            // Add back previous value
            randOffsets.Add(previousVal);
            previousVal = currVal;
            // Reduce timer
            timeLeft -= input.shootCooldownTime;
            // Wait for time between shots
            yield return shotsTime;
        }

        // Shooting time finished, start warming up again
        timeLeft = input.warmupTime;
        input.timeLeftInCooldown = 0f;
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            input.timeLeftInCooldown = timeLeft;
            yield return null;
        }
        input.timeLeftInCooldown = 0f;

        // Start shooting again
        goto isEnemyShooting;
    }

    // Routine that will check if the fire button is still being held down
    private IEnumerator isShootingHeld(Weapon input)
    {
        while (PlayerInput.instance.isShootHeld)
        {
            // check every frame if the button has been held
            yield return null;
        }

        // Else, it has been released
        switch(input.weaponType)
        {
            case WeaponTypes.Gatling:
                StopCoroutine(gatlingRoutine);
                gatlingRoutine = null;
                input.timeLeftInCooldown = 0f;
                break;
            case WeaponTypes.Laser:
                //
                break;
            default:
                //
                break;
        }
    }

    /// <summary> LaserBehaviour: TODO: FINISH DESC </summary>
    private void LaserBehaviour(Weapon input)
    {

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
        GameObject firedProjectile = Instantiate(weapon.prefab, AnchorObject.transform.position, zRotation, ProjectileContainer.Instance.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(AnchorObject.tag, projectileLayer, weapon.speed, weapon.damage);
    }

    /// <summary> Add a float to the angle (In degrees) </summary>
    private void AddedAngleSpawn(Weapon weapon, float addedAngle)
    {
        //Create Rotation Offset
        Quaternion modifiedRotation = ComputeRotation(addedAngle);

        //Spawn Projectile
        GameObject firedProjectile = Instantiate(weapon.prefab, AnchorObject.transform.position, modifiedRotation, ProjectileContainer.Instance.transform);
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
        GameObject firedProjectile = Instantiate(weapon.prefab, overridenPosition, zRotation, ProjectileContainer.Instance.transform);
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
        GameObject firedProjectile = Instantiate(weapon.prefab, overridenPosition, modifiedRotation, ProjectileContainer.Instance.transform);
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