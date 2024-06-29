using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public enum EnemyType
{
    BASIC,
    LASER,
    GATLING
}

public class Enemy : CombatEntity
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float enterSpeed = 20f;
    [SerializeField] protected float arrivalPercentUpScreen = 0.8f;

    //Local Variables
    [Header("Combat")]
    [SerializeField] protected float shootDelay = 1f;

    // Should hold the Stat data of this enemy
    [Header("Scriptable Data Object")]
    [Tooltip("If ignoreDataHolder is true, the enemy will have the data set in the inspector")]
    [SerializeField] private bool ignoreDataHolder;
    [SerializeField] private ScriptableEnemy statsData;

    // Variables used to control state within the enemy
    protected bool canShoot = false;
    protected bool canMove = true;

    // Variables used for entering and exiting screen
    private bool enterScreen = true;
    private bool exitScreen = false;
    private bool inScreen = false;
    protected bool moveDown = true;

    // References
    protected ShootComponent shootComponent;
    protected Weapon currWeapon;
    private float yOffSet;
    protected virtual void Start()
    {
        //Remember to look into the CombatEntity class to see what variables
        //Should be kept track of or re-set here if needed

        //AssignYlocation for final location of the enemy.
      //  AssignYlocation();
        //Initialize Component
        shootComponent = GetComponent<ShootComponent>();

        //Set Stats from data holder if not ignored
        if (!ignoreDataHolder)
        {
            health = statsData.health;
            shield = statsData.shield;
            collisionDamage = statsData.collisionDamage;
            dmgInvulnTime = statsData.dmgInvulnTimeSecs;
        }
        // The weapon must be loaded from data however
        currWeapon = statsData.weapon.GetWeaponObject();
    }



    /// <summary>
    /// Update function that handles the entering and exiting state of the enemy; is designed to be overriden
    /// by a specific enemy class; call the base function and add states after
    /// </summary>
    protected virtual void Update()
    {
        if (enterScreen && moveDown)
            EnterScreenSpace();
        if (exitScreen)
            ExitScreen();

        // Enable boolean values when the enemy gets onto the screen at the right point
        if (!inScreen && Camera.main.WorldToScreenPoint(gameObject.transform.position).y <= Camera.main.pixelHeight)
        {
            inScreen = true;
            canShoot = true;
        }

        if (!inScreen)      // Your enemy states are designed to run below this in a overriden function
            return;
    }

    /// <summary>
    /// Move function designed to be overwritten by a specific enemy script
    /// </summary>
    protected virtual void Move()
    {
        if (!canMove)
            return;
    }

    /// <summary>
    /// Moves the enemy into the screen based on the arrival percentage variable and it's enter speed
    /// </summary>
    protected virtual void EnterScreenSpace()
    {
        transform.Translate(Vector3.down * Time.deltaTime * enterSpeed);
        // Move Down
        moveDown = Camera.main.WorldToViewportPoint(transform.position).y > (arrivalPercentUpScreen);      // Check if should move down again.

        // TODO: Toggle enemy invulnerability here so that the enemy doesn't take damage during decent
    }

    protected virtual void AssignYlocation() {

        yOffSet = transform.localPosition.y;
    }

    public virtual void setArrivalPercentUpScreen(float value)
    {

        arrivalPercentUpScreen = value;
    }



    /// <summary>
    /// Moves the enemy off screen and destoys it afterwards
    /// </summary>
    protected virtual void ExitScreen() // method that causes ship to exit screen
    {
        // TODO: Code for the enemy to exit the play space
        // TODO: Destroy enemy
        // TODO: Do not give the player score or objective points if enemy dies this way
    }

    //UNUSED: Execute instructions for when player dies
    protected override void WhenPlayerDies()
    {
        //Stub
    }

    /// <summary>
    /// Behaviors that trigger when the specific enemy dies
    /// </summary>
    protected override void TriggerDeath()
    {
        EventData.RaiseOnEnemyDeath(gameObject);

        StopAllCoroutines();

        // Explosion Effect
        Instantiate(deathEffectPrefab, this.transform.position, Quaternion.identity);

        // Destroy Enemy Object
        Destroy(gameObject);
    }

    /// <summary>
    /// Disables the canShoot boolean for a short amount of time
    /// </summary>
    protected IEnumerator ShootDelayCo()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    /// <summary>
    /// Waits for the enemy's death event to be called before destoying the enemy object
    /// </summary>
    protected IEnumerator DestroyEnemyAfterCallCo()
    {
        yield return new WaitUntil(() => EventData.RaiseOnEnemyDeath(gameObject));
        Destroy(this.gameObject);
    }

    public ScriptableEnemy GetStatData()
    {
        return statsData;
    }
}
