using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script/Class that manages all the stats conected to the player </summary>
public class Player : CombatEntity
{
    //Singleton
    public static Player instance;

    //Player Related
    [SerializeField] private ScriptablePlayer playerStatsData;
    [SerializeField] private bool isShieldBroken;
    [SerializeField] private FMODUnity.EventReference deathSFX;
    [SerializeField] private FMODUnity.EventReference shieldHitSFX;
    [SerializeField] private FMODUnity.EventReference healthHitSFX;
    private int MAX_HEALTH; // Not serialized, since they are meant to be editted through the scriptable object
    private int MAX_SHIELD;

    //Settings
    [SerializeField] private bool IsDebugLogging;

    //Testing
    [SerializeField] private bool DamageTest;
    [SerializeField] private bool HealTest;
    [SerializeField] private bool GainShieldTest;
    [SerializeField] private bool DeathTest;
    [SerializeField] private bool InvulnerabilityTest;
    [SerializeField] private float shieldFloat;

    //Local variables
    private ShootComponent shootComponent;
    private AbilityComponent abilityComponent;
    private Coroutine ShieldRoutine;
    private Coroutine isShieldRestoredRoutine;

    // IAN HACK: The Status bars of the hud keep picking up a player object that gets destroyed or 
    // something, so this event fixes that
    public static System.Action hasLoadedStats;
    private void RaiseHasLoadedStats() => hasLoadedStats?.Invoke();

    protected override void Awake()
    {
        base.Awake();

        // Handle Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        //Get Components
        shootComponent = GetComponent<ShootComponent>();
        abilityComponent = GetComponent<AbilityComponent>();

        // Change Stats if cheat(s) are active
        if (GameSettings.instance.isHealth100)
        {
            // Health 100 cheat is active
            MAX_HEALTH = 100;
            health = MAX_HEALTH;
        }
        else
        {
            MAX_HEALTH = playerStatsData.maxHealth;
            health = MAX_HEALTH;
        }

        MAX_SHIELD = playerStatsData.maxShield;
        shield = MAX_SHIELD;
        shieldFloat = shield;
        collisionDamage = playerStatsData.collisionDamage;

        //Set Variables
        ShieldRoutine = null;
        isShieldRestoredRoutine = null;
        isShieldBroken = false;

        // Subscribe to events
        EventData.OnPlayerDeath += WhenPlayerDies;
    }

    // Unsubscribe from events on destroy
    protected override void OnDestroy()
    {
        EventData.OnPlayerDeath -= WhenPlayerDies;
        instance = null;
    }

    //Update's only purpose is debugging, everything else runs on
    //Coroutines, independent of update
    private void Update()
    {
        //Testing
        if (DamageTest)
        {
            TakeDamage(2, out int dmgRecieved, out Color colorSet);
            HitpointsRenderer.Instance.PrintDamage(this.transform.position, dmgRecieved, colorSet);
            DamageTest = false;
        }
        if (HealTest)
        {
            TryAddHealth(1);
            HealTest = false;
        }
        if (GainShieldTest)
        {
            TryAddShield(1);
            GainShieldTest = false;
        }
        if (InvulnerabilityTest)
        {
            TriggerInvulnerability(10f);
            InvulnerabilityTest = false;
        }
        if (DeathTest)
        {
            // FIXME: Re-work this
            TriggerDeath();
            DeathTest = false;
        }

        // HACK: Fuh dis shit, spamming this event with update to make it load the HUD
        RaiseHasLoadedStats();
    }

    #region WEAPON SYSTEMS

    /// <summary> Shoots the current weapon the player has selected </summary>
    public void ShootWeapon()
    {
        // Dont shoot if disabled
        if (isShootingLocked) return;
        
        // Shoot weapon
        shootComponent.ShootWeapon(WeaponArsenal.instance.GetCurrentWeapon());
    }

    /// <summary> Switches to the next weapon in the arsenal </summary>
    public void SwitchToNextWeapon()
    {
        WeaponArsenal.instance.SwitchToNextWeapon();
    }

    /// <summary> Switches to the previous weapon in the arsenal </summary>
    public void SwitchToPreviousWeapon()
    {
        WeaponArsenal.instance.SwitchToPreviousWeapon();
    }

    #endregion

    #region ABILITY SYSTEMS

    /// <summary> Function that will attempt to trigger the player's selected ability </summary>
    public void UseAbility()
    {
        // Dont use ability if locked
        if (isAbilityLocked) return;

        // Dont use ability if out of charges
        if (AbilityInventory.instance.GetCurrentAbility().charges <= 0) return;

        // Try using ability
        abilityComponent.TriggerAbility(AbilityInventory.instance.GetCurrentAbility());
    }

    /// <summary> Switches to the next ability in the inventory </summary>
    public void SwitchToNextAbility()
    {
        AbilityInventory.instance.SwitchToNextAbility();
    }

    /// <summary> Switches to the previous ability in the inventory </summary>
    public void SwitchToPreviousAbility()
    {
        AbilityInventory.instance.SwitchToPreviousAbility();
    }

    #endregion

    #region STATS MODIFIERS

    /// <summary> Try adding health to the player, returns false if its not possible </summary>
    public bool TryAddHealth(int amount)
    {
        //Dont do anything in case we already had max health
        if (health >= MAX_HEALTH)
        {
            return false;
        }

        int healthBefore = health;

        //Else, gain health
        health += amount;
        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }

        //Invoke the event signaling a change of health
        EventData.RaiseOnHealthAdded(health);
        if (IsDebugLogging)
        {
            string msg = "PLAYER HAS GAINED " + (health - healthBefore) + " HEALTH POINTS";
            Debug.Log(msg);
        }
        return true;
    }

    /// <summary> Try adding Shield Points to the player, returns false if its not possible </summary>
    public bool TryAddShield(int amount)
    {
        //Dont do anything in case we already had max shield
        if (shield >= MAX_SHIELD)
        {
            return false;
        }

        int shieldBefore = shield;

        //Else, gain shield
        shield += amount;
        if (shield > MAX_SHIELD)
        {
            shield = MAX_SHIELD;
        }

        //Invoke the event signaling a shield gain
        EventData.RaiseOnShieldGained(shield);
        if (IsDebugLogging) 
        {
            string msg = "PLAYER HAS GAINED " + (shield - shieldBefore) + " SHIELD POINTS";
            Debug.Log(msg); 
        }
        return true;
    }

    //TODO: See if design plans to have buff items or something of the like
    /// <summary> Changes the amount of shield per second regenerated </summary>
    public void SetShieldRegen(float shieldPerSec)
    {
        playerStatsData.shieldPerSecond = shieldPerSec;
        if (IsDebugLogging) { Debug.Log("CHANGED SHIELD REGEN AMOUNT TO " + playerStatsData.shieldPerSecond); }
    }

    #endregion

    #region DAMAGE HANDLING

    // Damage recieved, declared as per contract with IDamageable interface
    // TODO: Ask design if projectiles should be deleted if colliding against
    // an invulnerable player, or let them phase through (?) 
    public override void TakeDamage(int damageIn, out int dmgRecieved, out Color colorSet)
    {
        //Set these so they return properly if invulnerable
        dmgRecieved = 0;
        //Unless hull damage is recieved, its shield dmg
        colorSet = Color.cyan;
        
        //Dont do anything if invulnerable
        if (isInvulnerable) return;

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
        if (triggeredReturn) return;

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

            // Play shield hit sound
            SoundManager.instance.PlaySound(shieldHitSFX);

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
            EventData.RaiseOnShieldBroken(shield);

            // Play shield hit sound
            SoundManager.instance.PlaySound(shieldHitSFX);

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

            // Play health hit sound
            SoundManager.instance.PlaySound(healthHitSFX);
            return;
        }
        
        //else, the player died
        if (IsDebugLogging) { Debug.Log("EVENT: TRIGERRED PLAYER'S DEATH!!!!!"); }
        HullDmgRecieved = health; //Set outgoing var
        health = 0;
        TriggerDeath();
    }

    public override void TriggerInvulnerability(float seconds, bool ignoreCollisions = false)
    {
        // If input is less, return
        if (seconds < timeLeftInvulnerable) return;

        // Else, new invulnerability gives more time, Stop current invulnerabily routine if still running
        if (invulnRoutine != null) StopCoroutine(invulnRoutine);

        // Start invuln routine
        invulnRoutine = StartCoroutine(iFramesRoutine(seconds, ignoreCollisions));
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

    #endregion

    #region COROUTINES

    /// <summary> Coroutine that handles the regeneration of the shield </summary>
    private IEnumerator ShieldRegenBehaviour()
    {
        //First, wait for shield regen delay
        yield return new WaitForSeconds(playerStatsData.shieldRegenDelayTime);

        if (IsDebugLogging) { Debug.Log("STARTED REGEN SHIELD BEHAVIOUR"); }
        int shieldBefore = shield;

        //Start Regen of the shield
        while (shieldFloat < MAX_SHIELD)
        {
            shieldFloat += playerStatsData.shieldPerSecond * Time.deltaTime;
            //Assign the float value to player's shiled, typecasted
            shield = (int)shieldFloat;
            
            //Check if we have regenerated a point. If so, Raise shield point gained
            if (shieldBefore < shield)
            {
                EventData.RaiseOnShieldGained(shield);
            }

            //Update shield before
            shieldBefore = shield;
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

    // Overriden as to allow for disabling collisions with other entities while on iFrames
    protected override IEnumerator iFramesRoutine(float seconds, bool ignoreCollisions)
    {
        isInvulnerable = true;
        isIgnoringCollisions = ignoreCollisions;
        timeLeftInvulnerable = seconds;
        // Raise event for flashing effect on ship
        EventData.RaiseOnInvulnerabilityToggled(isEntering: true);

        // Runs the iframes timer
        while (timeLeftInvulnerable > 0f)
        {
            timeLeftInvulnerable -= Time.deltaTime;
            // Wait a frame
            yield return null;
        }

        // Invulnerability time over
        isIgnoringCollisions = false;
        timeLeftInvulnerable = 0f;

        // Wait two frames to remove bullets inside the player collider
        yield return null;
        yield return null;
        isInvulnerable = false;
        invulnRoutine = null;
        // Raise event to stop flashing effect on ship
        EventData.RaiseOnInvulnerabilityToggled(isEntering: false);
    }

    #endregion

    #region EVENT HANDLING

    //Should only contain calls to animations, sounds, sfx and the like on death
    protected override void TriggerDeath()
    {
        // Play death sound
        SoundManager.instance.PlaySound(deathSFX);
        
        //Stub
        EventData.RaiseOnHealthLost(health); //To remove Last Health segment from UI
        EventData.RaiseOnPlayerDeath();
        ScenesManager.instance.LoadScene(Scenes.GameOver);
    }

    //What happens to the game and the player on death
    protected override void WhenPlayerDies()
    {
        //NOTE: after death triggers, TakeDamage() wont respond to incoming damage anymore
        StopAllCoroutines();
        isInvulnerable = true;

        //TODO: Add here events that happens when player dies ---------------

        // Explosion Effect
        Instantiate(deathEffectPrefab, this.transform.position, Quaternion.identity); 

        //-----------------------------------------------------------------------

        // Destroy player object from scene
        Destroy(gameObject);
    }

    #endregion

    #region GETTERS

    //Getters
    public int GetHealth() { return health; }
    public int GetShield() { return shield; }
    public int GetMaxShield() { return MAX_SHIELD; }
    public int GetMaxHealth() { return MAX_HEALTH; }

    //This getter method may prove useful for building the UI
    public float GetShieldFloat() { return shieldFloat; }

    #endregion
}
