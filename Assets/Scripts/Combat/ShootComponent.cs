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
    private CombatEntity entityScript;
    private Coroutine gatlingRoutine = null;
    private Coroutine laserRoutine;

    // Initial setup
    private void Start()
    {
        // Get the entity component script
        entityScript = GetComponent<CombatEntity>();

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
        input.timeLeftInCooldown = input.shootCooldownTime;

        // Cooldown timer
        while (input.timeLeftInCooldown > 0f)
        {
            // Invoke the UI event for the weapon
            input.RaiseModifyMeterCooldown(input.shootCooldownTime, input.timeLeftInCooldown);
            input.timeLeftInCooldown -= Time.deltaTime; // Compute time
            yield return null; // Wait a frame
        }
        input.isOnCooldown = false;
        input.timeLeftInCooldown = 0f;

        // Raise cooldown event one last time
        input.RaiseModifyMeterCooldown(input.shootCooldownTime, input.timeLeftInCooldown);
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
        SoundManager.instance.PlaySound(weapon.mainSound);
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
        SoundManager.instance.PlaySound(weapon.mainSound);
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
        SoundManager.instance.PlaySound(weapon.mainSound);
        //Start cooldown
        StartCoroutine(ShootingCooldown(weapon));
    }

    #endregion

    #region GATLING BEHAVIOURS

    /// <summary> GatlingBehaviour: rapid fire shots that offset after each fire </summary>
    private void GatlingBehaviour(Weapon input)
    {
        // Dont enter routine if already running
        if (gatlingRoutine != null) return;

        // Check if player or enemy
        if (input.isEnemy)
        {
            gatlingRoutine = StartCoroutine(EnemyGatlingRoutine(input));
            return;
        }

        // Else, Its the player, start the windup and check for button release
        gatlingRoutine = StartCoroutine(PlayerGatlingRoutine(input));
        StartCoroutine(isShootingHeld(input));
    }

    // This Routine will be stopped if the button is released
    private IEnumerator PlayerGatlingRoutine(Weapon input)
    {
        // Warm up timer
        // TODO: Missing warm up sound
        input.warmupCounter = input.warmupTime;
        while (input.warmupCounter > 0f)
        {
            // Raise UI event
            input.RaiseModifyMeterCharge(input.warmupTime, input.warmupCounter);
            // Compute time
            input.warmupCounter -= Time.deltaTime;
            yield return null;
        }
        input.warmupCounter = 0f;

        // Raise Weapon UI event once more
        input.RaiseModifyMeterCharge(input.warmupTime, input.warmupCounter);

        // Warmup finshed, start shooting until stopped
        WaitForSeconds shotsTime = new WaitForSeconds(input.shootCooldownTime);
        List<float> randOffsets = new List<float>();
        randOffsets.Add(-0.5f);
        randOffsets.Add(0.5f);
        float previousVal = 0;
        float currVal;
        int index;
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
            SoundManager.instance.PlaySound(input.mainSound);
            // Add back previous value
            randOffsets.Add(previousVal);
            previousVal = currVal;
            // Wait for time between shots
            yield return shotsTime;
        }
    }

    // Variant used for enemies
    private IEnumerator EnemyGatlingRoutine(Weapon input)
    {
        // Data for shooting
        float timeLeft;
        WaitForSeconds shotsTime = new WaitForSeconds(input.shootCooldownTime);
        List<float> randOffsets = new List<float>();
        randOffsets.Add(-0.5f);
        randOffsets.Add(0.5f);
        float previousVal = 0;
        float currVal;
        int index;

        // Label for jumping
        ShootingLoop:

        // Warm up
        timeLeft = input.warmupTime;
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            input.warmupCounter = timeLeft;
            yield return null;
        }
        input.warmupCounter = 0f;

        // Shoot for some time specified
        timeLeft = input.shootingStayTime;
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
            SoundManager.instance.PlaySound(input.mainSound);
            // Add back previous value
            randOffsets.Add(previousVal);
            previousVal = currVal;
            // Reduce timer
            timeLeft -= input.shootCooldownTime;
            // Wait for time between shots
            yield return shotsTime;
        }
        // Loop
        goto ShootingLoop;
    }

    // Routine that will check if the fire button is still being held down for the gatling
    private IEnumerator isShootingHeld(Weapon input)
    {
        // Continue charging and firing gatling gun as long as the button is hold and the weapon
        // Hasnt been switched or locked
        while (PlayerInput.instance.isShootHeld && WeaponArsenal.instance.GetCurrentWeapon() == input && !entityScript.isShootingLocked)
        {
            // check every frame for firing conditions
            yield return null;
        }

        // Else, it has been released
        StopCoroutine(gatlingRoutine);
        gatlingRoutine = null;
        input.warmupCounter = 0f;
        // Update UI once shooting stops
        input.RaiseModifyMeterCharge(input.warmupTime, input.warmupTime);
    }

    #endregion

    #region LASER BEHAVIOUR

    /// <summary> LaserBehaviour: Fires a laser from the weapon anchor to the edge of the screen </summary>
    private void LaserBehaviour(Weapon input)
    {
        // Dont enter routine if already running
        if (laserRoutine != null) return;

        // Dont fire if weapon is on cooldown
        if (input.isOnCooldown) return;

        // Check if player or enemy
        if (input.isEnemy)
        {
            laserRoutine = StartCoroutine(EnemeyLaserRoutine(input));
            return;
        }

        // Else, Its the player, start the windup and check for button release
        laserRoutine = StartCoroutine(PlayerLaserRoutine(input));
    }

    // The player routine for the laser fire
    private IEnumerator PlayerLaserRoutine(Weapon input)
    {
        // CHARGING SPHERE PARAMETERS:
        float maxSphereDiameter = 2.5f;
        float chargeSphereKillTime = 0.2f;
        float chargeSphereKillTimeIfCancelled = 0.05f;
        // LASER PARAMETERS:
        float minLaserWidth = 0.5f;
        float maxLaserWidth = 2.5f;
        float timeToFullWidth = 0.05f;
        float timeToZeroWidth = 0.5f;

        // Create the chargeup prefab
        GameObject chargeSphere = Instantiate(input.chargingSpherePrefab, AnchorObject.transform.position, AnchorObject.transform.rotation, AnchorObject.transform);
        chargeSphere.transform.localScale = Vector3.zero;
        // Charge up sphere data
        float rateOfChange = maxSphereDiameter / input.maxChargeUpTime;
        float currentDiameter;
        input.chargeTimeCounter = input.maxChargeUpTime;

        // Charge up the laser while held, not switched and shooting not locked
        while (PlayerInput.instance.isShootHeld && WeaponArsenal.instance.GetCurrentWeapon() == input && !entityScript.isShootingLocked)
        {
            // Update UI
            input.RaiseModifyMeterCharge(input.maxChargeUpTime, input.chargeTimeCounter);
            // Check if charging is finished
            if (input.chargeTimeCounter > 0f)
            {
                input.chargeTimeCounter -= Time.deltaTime;
                currentDiameter = rateOfChange * (input.maxChargeUpTime - input.chargeTimeCounter);
                chargeSphere.transform.localScale = new Vector3(currentDiameter, currentDiameter, currentDiameter);
            }
            else
            {
                input.chargeTimeCounter = 0f;
                currentDiameter = maxSphereDiameter;
                chargeSphere.transform.localScale = new Vector3(currentDiameter, currentDiameter, currentDiameter);
            }
            // Wait a frame
            yield return null;
        }

        // If it was switched or shooting was locked, then cancel shooting
        if (WeaponArsenal.instance.GetCurrentWeapon() != input || entityScript.isShootingLocked)
        {
            laserRoutine = null;
            input.chargeTimeCounter = 0f;
            StartCoroutine(ReduceSphereTillZero(chargeSphere, chargeSphereKillTimeIfCancelled));
            input.RaiseModifyMeterCharge(input.maxChargeUpTime, input.chargeTimeCounter); // Update UI
            yield break;
        }

        // Start reducing the charging sphere as we fire for a better looking animation
        StartCoroutine(ReduceSphereTillZero(chargeSphere, chargeSphereKillTime));

        // Raycast towards end of screen to find distance to edge of screen
        RaycastHit[] raycastArray;
        float distanceToEdge;
        int edgeLayerMask = LayerMask.GetMask("Edge Collider Boxes");

        // FIRST: Calculate distance to edge of gameplay area
        bool didHit = Physics.Raycast(AnchorObject.transform.position, AnchorObject.transform.up, out RaycastHit hitInfo, 200, edgeLayerMask);
        if (didHit) distanceToEdge = hitInfo.distance;
        else
        {
            // Safeguard: This wouldnt break unless the raycast wasnt able to find the edge screen colliders
            Debug.Log("ERROR! LASER GUN WASNT ABLE TO FIND THE EDGE SCREEN COLLIDERS IN THE SCENE! Check code at ShootComponent.cs");
            yield break; 
        }

        // Calculate the width/diameter of the laser based on charge up time
        rateOfChange = (maxLaserWidth - minLaserWidth) / (input.maxChargeUpTime);
        float laserWidth = rateOfChange * (input.maxChargeUpTime - input.chargeTimeCounter) + minLaserWidth;

        // Create the laser with the specified distance
        GameObject laser = Instantiate(input.prefab, AnchorObject.transform.position, AnchorObject.transform.rotation);
        // Extend the laser by a bit so the player cant see the end of the laser
        Vector3 endPos = hitInfo.point + 2 * AnchorObject.transform.up; 
        StartCoroutine(HandleLaserLine(laser, laserWidth, timeToFullWidth, timeToZeroWidth, AnchorObject.transform.position, endPos));

        // Wait a frame so the laser renders
        yield return null;

        // Call SphereCastAll in the direction of the laser and stop at the edge collider
        raycastArray = Physics.SphereCastAll(AnchorObject.transform.position, laserWidth/2, AnchorObject.transform.up, distanceToEdge);

        // Calculate rateOfChange of damage depending on charge time
        rateOfChange = (input.maxDamage - input.minDamage) / (input.maxChargeUpTime);

        // Check all hits
        foreach (RaycastHit currHit in raycastArray)
        {
            // Dont damage the creator
            if (currHit.collider.gameObject.CompareTag(this.gameObject.tag)) continue;

            // Else, attempt to damage if its an entity
            if (currHit.collider.gameObject.TryGetComponent<CombatEntity>(out CombatEntity entity))
            {
                // if the entity is ignoring collisions, then ignore
                if (entity.isIgnoringCollisions) continue;

                // Calculate damage depending on charge time
                int damage = (int)(rateOfChange * (input.maxChargeUpTime - input.chargeTimeCounter) + input.minDamage); // Floor it

                // Deal damage
                entity.TakeDamage(damage, out int dmgRecieved, out Color colorSet);
                HitpointsRenderer.Instance.PrintDamage(currHit.point, dmgRecieved, colorSet);
            }
        }
        // Call cooldown on this laser weapon
        input.chargeTimeCounter = 0f;
        StartCoroutine(ShootingCooldown(input));
        // Laser fire finished
        laserRoutine = null;
    }

    private IEnumerator EnemeyLaserRoutine(Weapon input)
    {
        // CHARGING SPHERE PARAMETERS:
        float maxSphereDiameter = 2.5f;
        float chargeSphereKillTime = 0.2f;
        // LASER PARAMETERS:
        float minLaserWidth = 0.5f;
        float maxLaserWidth = 2.5f;
        float timeToFullWidth = 0.05f;
        float timeToZeroWidth = 0.5f;

        // Pre-declare variables before loop
        GameObject chargeSphere;
        float rateOfChange;
        float currentDiameter;
        float elapsedTime;
        RaycastHit hitInfo;
        RaycastHit[] raycastArray;
        float distanceToEdge;
        int edgeLayerMask;
        int damageVal;
        bool didHit;
        float laserWidth;
        GameObject laser;
        Vector3 endPos;
        WaitForSeconds cooldown = new WaitForSeconds(input.shootCooldownTime);

    // Label for looping
    LoopShooting:

        // Create the chargeup prefab
        chargeSphere = Instantiate(input.chargingSpherePrefab, AnchorObject.transform.position, AnchorObject.transform.rotation, AnchorObject.transform);
        chargeSphere.transform.localScale = Vector3.zero;
        // Charge up sphere data
        rateOfChange = maxSphereDiameter / input.maxChargeUpTime;
        elapsedTime = 0f;

        // Charge up the laser
        while (elapsedTime < input.maxChargeUpTime)
        {
            elapsedTime += Time.deltaTime;
            input.chargeTimeCounter = elapsedTime;
            currentDiameter = rateOfChange * elapsedTime;
            chargeSphere.transform.localScale = new Vector3(currentDiameter, currentDiameter, currentDiameter);
            yield return null; // Wait a frame
        }
        // Finished charging
        input.chargeTimeCounter = input.maxChargeUpTime;
        currentDiameter = maxSphereDiameter;
        chargeSphere.transform.localScale = new Vector3(currentDiameter, currentDiameter, currentDiameter);

        // Start reducing the charging sphere as we fire for a better looking animation
        StartCoroutine(ReduceSphereTillZero(chargeSphere, chargeSphereKillTime));

        // Raycast towards end of screen to find distance to edge of screen
        edgeLayerMask = LayerMask.GetMask("Edge Collider Boxes");

        // FIRST: Calculate distance to edge of gameplay area
        didHit = Physics.Raycast(AnchorObject.transform.position, AnchorObject.transform.up, out hitInfo, 200, edgeLayerMask);
        if (didHit) distanceToEdge = hitInfo.distance;
        else
        {
            // Safeguard: This wouldnt break unless the raycast wasnt able to find the edge screen colliders
            Debug.Log("ERROR! LASER GUN WASNT ABLE TO FIND THE EDGE SCREEN COLLIDERS IN THE SCENE! Check code at ShootComponent.cs");
            yield break;
        }

        // Calculate the width/diameter of the laser based on charge up time
        rateOfChange = (maxLaserWidth - minLaserWidth) / (input.maxChargeUpTime);
        laserWidth = rateOfChange * input.chargeTimeCounter + minLaserWidth;

        // Create the laser with the specified distance
        laser = Instantiate(input.prefab, AnchorObject.transform.position, AnchorObject.transform.rotation);
        // Extend the laser by a bit so the player cant see the end of the laser
        endPos = hitInfo.point + 2 * AnchorObject.transform.up;
        StartCoroutine(HandleLaserLine(laser, laserWidth, timeToFullWidth, timeToZeroWidth, AnchorObject.transform.position, endPos));

        // Wait a frame so the laser renders
        yield return null;

        // Call SphereCastAll in the direction of the laser and stop at the edge collider
        raycastArray = Physics.SphereCastAll(AnchorObject.transform.position, laserWidth / 2, AnchorObject.transform.up, distanceToEdge);

        // Calculate rateOfChange of damage depending on charge time
        rateOfChange = (input.maxDamage - input.minDamage) / (input.maxChargeUpTime);

        // Check all hits
        foreach (RaycastHit currHit in raycastArray)
        {
            // Dont damage the creator
            if (currHit.collider.gameObject.CompareTag(this.gameObject.tag)) continue;

            // Else, attempt to damage if its an entity
            if (currHit.collider.gameObject.TryGetComponent<CombatEntity>(out CombatEntity entity))
            {
                // if the entity is ignoring collisions, then ignore
                if (entity.isIgnoringCollisions) continue;

                // Calculate damage depending on charge time
                damageVal = (int)(rateOfChange * input.chargeTimeCounter + input.minDamage); // Floor it

                // Deal damage
                entity.TakeDamage(damageVal, out int dmgRecieved, out Color colorSet);
                HitpointsRenderer.Instance.PrintDamage(currHit.point, dmgRecieved, colorSet);
            }
        }
        // Cooldown on this laser weapon
        input.chargeTimeCounter = 0f;
        yield return cooldown;
        // Loop back to beginning
        goto LoopShooting;
    }

    // Helper routine to reduce the Charging sphere while firing the laser
    private IEnumerator ReduceSphereTillZero(GameObject chargingSphere, float duration)
    {
        float maxDiameter = chargingSphere.transform.localScale.x;
        float elapsedTime = 0;
        float rateOfChange = chargingSphere.transform.localScale.x / duration;
        float currentDiameter;
        // Start making the sphere smaller across a set duration
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentDiameter = maxDiameter - rateOfChange * elapsedTime;
            chargingSphere.transform.localScale = new Vector3(currentDiameter, currentDiameter, currentDiameter);
            yield return null;
        }
        // Finally, Destroy it
        Destroy(chargingSphere);
    }

    // Helper routine for handling the laser line
    private IEnumerator HandleLaserLine(GameObject laser, float laserWidth, float timeToFullWidth, float timeToZeroWidth, Vector3 startPos, Vector3 endPos)
    {
        // Helper variables
        float elapsedTime = 0f;
        float currentWidth;
        // Get the line renderer component and set the width to zero and the start and end positions
        LineRenderer laserLine = laser.GetComponent<LineRenderer>();
        laserLine.startWidth = 0;
        laserLine.endWidth = 0;
        laserLine.SetPosition(0, startPos);
        laserLine.SetPosition(1, endPos);
        // Compute the rate of change for the width when going from zero width to full
        float rateOfChange = laserWidth / timeToFullWidth;
        // Increase laser line to full width over time specified
        while (elapsedTime < timeToFullWidth)
        {
            // Compute width over time
            elapsedTime += Time.deltaTime;
            currentWidth = rateOfChange * elapsedTime;
            // Set the laser line width
            laserLine.startWidth = currentWidth;
            laserLine.endWidth = currentWidth;
            yield return null; // Wait a frame
        }
        // Once we have reached full width, now go back to zero
        elapsedTime = 0;
        rateOfChange = laserWidth / timeToZeroWidth;
        while (elapsedTime < timeToZeroWidth)
        {
            // Compute width over time
            elapsedTime += Time.deltaTime;
            currentWidth = laserWidth - rateOfChange * elapsedTime;
            // Set the laser line width
            laserLine.startWidth = currentWidth;
            laserLine.endWidth = currentWidth;
            yield return null; // Wait a frame
        }
        // Once finished, destroy the laser line object
        Destroy(laser);
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