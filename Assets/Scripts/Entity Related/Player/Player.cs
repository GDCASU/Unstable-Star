using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script/Class that manages all the stats conected to the player </summary>
public class Player : CombatEntity
{
    //Singleton
    public static Player Instance;

    //Player Related
    [SerializeField] private int MAX_HEALTH = 10;
    [SerializeField] private int MAX_SHIELD = 5;
    [SerializeField] private float shieldPerSecond;
    [SerializeField] private float shieldRegenDelayTime = 3f; // in seconds, does not stack with invulnerable time
    [SerializeField] private bool isShieldBroken;

    //Settings
    [SerializeField] private GameObject WeaponAnchor;
    [SerializeField] private bool IsDebugLogging;

    //Testing
    [SerializeField] private bool DamageTest;
    [SerializeField] private bool HealTest;
    [SerializeField] private bool DeathTest;
    [SerializeField] private bool InvulnerabilityTest;
    [SerializeField] private float shieldFloat;

    //Local variables
    private ShootScript shootComponent;
    private List<Weapon> weaponArsenal = new List<Weapon>();
    private Weapon currWeapon;
    private Coroutine ShieldRoutine;
    private Coroutine isShieldRestoredRoutine;
    private int weaponIndex;
    
    private void Start()
    {
        //Sets the singleton
        Instance = this;

        //Instantiates Components
        shootComponent = GetComponent<ShootScript>();
        shootComponent.InitializeData(WeaponAnchor);

        //Set Stats
        health = MAX_HEALTH;
        shield = MAX_SHIELD;
        shieldFloat = shield;
        shieldPerSecond = 1f; //1 shield per second
        dmgInvulnTime = 1f;

        //Set Variables
        ShieldRoutine = null;
        isShieldRestoredRoutine = null;
        isShieldBroken = false;

        //Add Weapons
        Pistol pistol = new Pistol(30f, 1, "Pistol", 0.2f);
        Birdshot birdshot = new Birdshot(30f, 1, "Birdshot", 0.2f);
        Buckshot buckshot = new Buckshot(30f, 1, "Buckshot", 0.2f);

        weaponArsenal.Add(pistol);
        weaponArsenal.Add(birdshot);
        weaponArsenal.Add(buckshot);
        currWeapon = weaponArsenal[0];
        weaponIndex = 0;
    }

    //Update's only purpose is debugging, everything else runs on
    //Coroutines, independent of update
    private void Update()
    {
        //Testing
        if (DamageTest)
        {
            TakeDamage(1, out int dmgRecieved, out Color colorSet);
            HitpointsRenderer.Instance.PrintDamage(this.transform.position, dmgRecieved, colorSet);
            DamageTest = false;
        }
        if (HealTest)
        {
            AddHealth(1);
            HealTest = false;
        }
        if (InvulnerabilityTest)
        {
            TriggerInvulnerability(10f);
            InvulnerabilityTest = false;
        }
        if (DeathTest && this.ModelObject != null)
        {
            TriggerDeath();
            DeathTest = false;
        }
    }

    #region WEAPON UTILITIES

    /// <summary> Shoots the current weapon the player has selected </summary>
    public void ShootWeapon()
    {
        shootComponent.ShootWeapon(currWeapon);
    }

    /// <summary> Switches to the next weapon in the arsenal, from left to right </summary>
    public void SwitchToNextWeapon()
    {
        //Check if we arent at the end of the list 
        if (weaponIndex >= weaponArsenal.Count - 1)
        {
            //Return to start
            currWeapon = weaponArsenal[0];
            weaponIndex = 0;
            return;
        }

        //Else, switch to next in list
        ++weaponIndex;
        currWeapon = weaponArsenal[weaponIndex];
    }

    /// <summary> Adds a new weapon to the player's arsenal </summary>
    public void AddNewWeapon(Weapon newWeapon)
    {
        weaponArsenal.Add(newWeapon);
    }

    #endregion

    #region STATS MODIFIERS

    /// <summary> Adds health to the player </summary>
    public void AddHealth(int amount)
    {
        health += amount;
        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }
        //Invoke the event signaling a change of health
        if (IsDebugLogging) { Debug.Log("ATTEMPTED TO HEAL THE PLAYER"); }
        EventData.RaiseOnHealthAdded(health);
    }

    //TODO: See if design plans to have buff items or something of the like
    /// <summary> Changes the amount of shield per second regenerated </summary>
    public void SetShieldRegen(float shieldPerSec)
    {
        shieldPerSecond = shieldPerSec;
        if (IsDebugLogging) { Debug.Log("CHANGED SHIELD REGEN AMOUNT TO " + shieldPerSecond); }
    }

    #endregion

    #region DAMAGE HANDLING

    //Damage recieved, declared as per contract with IDamageable interface
    //TODO: Ask design if projectiles should be deleted if colliding against
    //an invulnerable player, or let them phase through (?) 
    public override void TakeDamage(int damageIn, out int dmgRecieved, out Color colorSet)
    {
        //Set these so they return properly if invulnerable
        dmgRecieved = 0;
        //Unless hull damage is recieved, its shield dmg
        colorSet = Color.cyan;
        
        //Dont do anything if invulnerable
        if (isInvulnerable) { return; };

        //Starts iFrames pertinent to damage taken
        TriggerInvulnerability(dmgInvulnTime, ignoreCollisions: true);

        //Variables for checking
        int shieldDmgCheck = shield - damageIn;
        int healthDmgCheck = health + shieldDmgCheck;
        bool triggeredReturn;

        //Explore Damage done to shield
        triggeredReturn = ExploreShieldDmg(shieldDmgCheck, out int ShieldDmgRecieved);
        dmgRecieved = ShieldDmgRecieved;

        //Start checking for when the shield regenerates at least 1 point
        HandleShieldRegen();
        HandleShieldResotredCheck();

        //If no damage is to be passed to hull, return
        if (triggeredReturn) { return; };

        //Else, damage to hull
        DealDamageToHull(healthDmgCheck, out int HullDmgRecieved);
        dmgRecieved = HullDmgRecieved;
        colorSet = Color.red;
    }

    //Explores what happens if we try to damage the shield
    private bool ExploreShieldDmg(int shieldDmgCheck, out int ShieldDmgRecieved)
    {
        //Try to damage the shield, wont execute if its already broken
        if (shieldDmgCheck > 0)
        {
            if (IsDebugLogging) { Debug.Log("DAMAGE RECIEVED TO SHIELD: " + (shield - shieldDmgCheck)); }

            ShieldDmgRecieved = shield - shieldDmgCheck; //Set outgoing vars
            //Update shield numbers
            shield = shieldDmgCheck;
            shieldFloat = (float)shield;
            EventData.RaiseOnShieldDamaged(shield);

            //If the shield was not broken, it means no changes to health, so return and stop here
            //TODO: Ask design if any damage should be negated with shield break, lets say I recieve
            //10 damage with only 3 shield, should the hull loose 7 points?
            //As it is, we negate all damage with shield break
            return true;
        }
        //If value is negative or zero, it means we broke the shield.
        //Wont run if it was already broken before regenerating 1 shield point
        else if (!isShieldBroken)
        {
            if (IsDebugLogging) { Debug.Log("SHIELD WAS BROKEN"); }

            ShieldDmgRecieved = shield; //Set outgoing vars
            //Update vars
            shield = 0;
            shieldFloat = 0;
            isShieldBroken = true;
            EventData.RaiseOnShieldBroken();
            //Return here to negate damage after shield break
            return true;
        }

        //If the shield was already broken, return false to perform damage to Hull Health
        shield = 0; //Resets shield regen progress
        ShieldDmgRecieved = 0;
        return false;
    }

    private void DealDamageToHull(int healthDmgCheck, out int HullDmgRecieved)
    {
        //If we passed everything above, it means damage to the hull
        if (healthDmgCheck > 0)
        {
            if (IsDebugLogging) { Debug.Log("DAMAGE RECIEVED TO HULL: " + (health - healthDmgCheck)); }
            HullDmgRecieved = health - healthDmgCheck; //Set outgoing var
            health = healthDmgCheck;
            EventData.RaiseOnHealthLost(health);
            return;
        }
        
        //else, the player died
        if (IsDebugLogging) { Debug.Log("EVENT: TRIGERRED PLAYER'S DEATH!!!!!"); }
        HullDmgRecieved = health; //Set outgoing var
        health = 0;
        TriggerDeath();
    }

    #endregion

    #region EVENT HANDLING

    //Should only contain calls to animations, sounds, sfx and the like on death
    protected override void TriggerDeath()
    {
        //Stub
        EventData.RaiseOnPlayerDeath();

        // TODO: Check if this is a good idea:
        // Never destroy the Player object, it will cause errors around the game
        // Instead, Destroy only its model (?)
        Destroy(this.ModelObject);
    }

    //What happens to the game and the player on death
    protected override void WhenPlayerDies()
    {
        //NOTE: after death triggers, TakeDamage() wont respond to incoming damage anymore
        StopAllCoroutines();
        isInvulnerable = true;

        //TODO: Add here events that happens when player dies ---------------



        //-----------------------------------------------------------------------
    }

    public override void TriggerInvulnerability(float seconds, bool ignoreCollisions = false)
    {
        // If input is less, return
        if (seconds < timeLeftInvulnerable)
        {
            return;
        }

        // Else, new invulnerability gives more time
        if (invulnRoutine != null)
        {
            // Stop current invulnerabily routine if still running
            StopCoroutine(invulnRoutine);
            Physics.IgnoreLayerCollision(PhysicsConfig.Get.PlayerLayer, PhysicsConfig.Get.EnemyLayer, false);
            Physics.IgnoreLayerCollision(PhysicsConfig.Get.PlayerLayer, PhysicsConfig.Get.HazardLayer, false);
        }

        invulnRoutine = StartCoroutine(iFramesRoutine(seconds, ignoreCollisions));
    }

    // Overriden as to allow for disabling collisions with other entities while on iFrames
    protected override IEnumerator iFramesRoutine(float seconds, bool ignoreCollisions)
    {
        isInvulnerable = true;
        timeLeftInvulnerable = seconds;

        //Disable Entity collisions if bool "ignoreCollisions" was true
        if (ignoreCollisions)
        {
            Physics.IgnoreLayerCollision(PhysicsConfig.Get.PlayerLayer, PhysicsConfig.Get.EnemyLayer, true);
            Physics.IgnoreLayerCollision(PhysicsConfig.Get.PlayerLayer, PhysicsConfig.Get.HazardLayer, true);
        }

        // Runs the iframes timer
        while (timeLeftInvulnerable > 0f)
        {
            timeLeftInvulnerable -= Time.deltaTime;
            // Wait a frame
            yield return null;
        }

        //Re-enable collisions upon end
        if (ignoreCollisions)
        {
            Physics.IgnoreLayerCollision(PhysicsConfig.Get.PlayerLayer, PhysicsConfig.Get.EnemyLayer, false);
            Physics.IgnoreLayerCollision(PhysicsConfig.Get.PlayerLayer, PhysicsConfig.Get.HazardLayer, false);
        }

        // Player can be hurt again
        timeLeftInvulnerable = 0f;
        isInvulnerable = false;
        invulnRoutine = null;
    }

    private void HandleShieldRegen()
    {
        //If shield was regening before, restart it
        if (ShieldRoutine != null)
        {
            StopCoroutine(ShieldRoutine);
        }
        ShieldRoutine = StartCoroutine(ShieldRegenBehaviour()); //NOTE: Does not stack with iFrames
    }

    /// <summary> Coroutine that handles the regeneration of the shield </summary>
    private IEnumerator ShieldRegenBehaviour()
    {
        //First, wait for shield regen delay
        yield return new WaitForSeconds(shieldRegenDelayTime);

        if (IsDebugLogging) { Debug.Log("STARTED REGEN SHIELD BEHAVIOUR"); }

        //Start Regen of the shield
        while (shieldFloat < MAX_SHIELD)
        {
            shieldFloat += shieldPerSecond * Time.deltaTime;
            //Assign the float value to player's shiled, typecasted
            shield = (int)shieldFloat;
            //Wait until 1 frame has passed
            yield return null;
        }
        //We finished regening the shield, assign it MAX_SHIELD in case we went over before
        shield = MAX_SHIELD;
        shieldFloat = (float)shield;
        ShieldRoutine = null;

        if (IsDebugLogging) { Debug.Log("REGEN SHIELD BEHAVIOUR FINISHED"); }
    }

    // Starts the Checking routine, stops ShieldRestoredRoutine from having more
    // Than one instance running
    private void HandleShieldResotredCheck()
    {
        //If it was already checking before, dont restart it
        if (isShieldRestoredRoutine != null)
        {
            return;
        }

        //Else, start checking
        isShieldRestoredRoutine = StartCoroutine(IsShieldRestoredRoutine());
    }

    private IEnumerator IsShieldRestoredRoutine()
    {
        //Runs until shield regenerated 1 point
        while (shield < 1)
        {
            //Wait until 1 frame has passed
            yield return null;
        }
        isShieldBroken = false;
        isShieldRestoredRoutine = null;
    }

    #endregion

    #region GETTERS AND SETTERS

    //Getters
    public int GetHealth() { return health; }
    public int GetShield() { return shield; }
    public int GetMaxShield() { return MAX_SHIELD; }
    public int GetMaxHealth() { return MAX_HEALTH; }
    //This getter method may prove useful for building the UI
    public float GetShieldFloat() { return shieldFloat; }
    public Weapon GetCurrWeapon() { return currWeapon; }

    #endregion
}
