using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that manages all the stats conected to the player
public class PlayerStats : MonoBehaviour, IDamageable
{
    //Singleton for any other scripts that interact with the player, like UI (?)
    public static PlayerStats Instance;

    //Readonly vars, has to be assigned here
    //TODO: Ask design if Max health and shield can be upgraded one day
    private readonly int MAX_HEALTH = 20;
    private readonly int MAX_SHIELD = 100;
    //how much % of the shield is regenerated every second, format = 15%, 30%, etc.
    private readonly float SHIELD_PERCENTAGE_REGEN_BASE = 5f;
    private readonly float INVULN_TIME = 1f; // in seconds
    private readonly float SHIELD_REGEN_DELAY_TIME = 6f; // in seconds, STACKS WITH INVULN_TIME!!

    [Header("Player Stats Readings")]
    public int HealthRead;
    public int ShieldRead;
    public float shieldFloatRead;
    public bool isShieldBrokenRead;
    public bool isInvulnerableRead;
    public bool isRegeningRead;

    [Header("Debugging")]
    public bool isDebugLogging;
    public bool DamageTest;
    public bool HealTest;

    //Delegates and events
    //Event that everything on the game should be aware of: player's death
    public delegate void OnPlayerDeathDelegate();
    public static event OnPlayerDeathDelegate OnPlayerDeath;

    //Events used to call an update to the UI, play animations, sounds, etc.
    public delegate void OnHealthLostDelegate();
    public static event OnHealthLostDelegate OnHealthLost; //Health Lost

    public delegate void OnHealthAddedDelegate();
    public static event OnHealthAddedDelegate OnHealthAdded; //Health Added

    public delegate void OnShieldDamagedDelegate();
    public static event OnShieldDamagedDelegate OnShieldDamaged; //Shield Damaged

    public delegate void OnShieldBrokenDelegate();
    public static event OnShieldBrokenDelegate OnShieldBroken; //Shield Broken

    //Local variables
    private Coroutine ShieldRoutine;
    public int health { get; private set; }
    public int shield { get; private set; }
    private float shieldPercentageVal;
    private bool isInvulnerable;
    private bool isShieldBroken;
    private bool isShieldRegening;
    private float shieldFloat;

    private void Start()
    {
        //Sets the singleton
        Instance = this;

        //Set Variables
        ShieldRoutine = null;
        health = MAX_HEALTH;
        shield = 5; //HACK: Testing shield regen and hull dmg, later should be set to MAX_SHIELD
        shieldFloat = shield;
        isInvulnerable = false;
        isShieldBroken = false;
        isShieldRegening = false;
        changeShieldRegenPercentage(SHIELD_PERCENTAGE_REGEN_BASE);

        //Debugging
        HealthRead = health;
        ShieldRead = shield;
        isRegeningRead = isShieldRegening;
    }

    //Update's only purpose is debugging, everything coded here runs on
    //Take damage, independent of update
    private void Update()
    {
        //Testing
        HealthRead = health;
        ShieldRead = shield;
        isRegeningRead = isShieldRegening;
        shieldFloatRead = shieldFloat;
        isShieldBrokenRead = isShieldBroken;
        isInvulnerableRead = isInvulnerable;
        if (DamageTest)
        {
            TakeDamage(1);
            DamageTest = false;
        }
        if (HealTest)
        {
            AddHealth(1);
            HealTest = false;
        }
    }

    //Adds health to the player, should be used by health packs and the like
    public void AddHealth(int amount)
    {
        health += amount;
        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }
        //Invoke the event signaling a change of health
        if (isDebugLogging) { Debug.Log("ATTEMPTED TO HEAL THE PLAYER"); };
        OnHealthAdded?.Invoke();
    }

    //TODO: See if design plans to have buff items or something of the like
    public void changeShieldRegenPercentage(float newPercentage)
    {
        shieldPercentageVal = (MAX_SHIELD * newPercentage) / 100;
        if (isDebugLogging) { Debug.Log("CHANGED HEALING PERCENTAGE TO " + shieldPercentageVal); };
    }

    //Damage recieved, declared as per contract with IDamageable interface
    //TODO: Ask design if projectiles should be deleted if colliding against
    //an invulnerable player, or let them phase through (?) 
    public void TakeDamage(int damage)
    {
        //Dont do anything if invulnerable
        if (isInvulnerable) { return; };

        //Start the iFrames after hit, the coroutine also handles the regen of shield
        if (isShieldRegening)
        {
            StopCoroutine(ShieldRoutine);
            isShieldRegening = false;
        }
        StartCoroutine(iFramesTimer());

        //Variables for checking
        int shieldDmgCheck = shield - damage;
        int healthDmgCheck = health - Mathf.Abs(shieldDmgCheck);
        bool triggeredReturn = false;

        //Explore Damage done to shield
        triggeredReturn = ExploreShieldDmg(shieldDmgCheck);
        
        //Start checking for when the shield regenerates at least 1 point
        //It finished instantly if theres more than 1 point
        StartCoroutine(CheckForShieldRestore());

        //If no damage is to be passed to hull, return
        if (triggeredReturn) { return; };

        //Explore damage to hull
        ExploreDamageToHull(healthDmgCheck);
    }

    //Explores what happens if we try to damage the shield
    private bool ExploreShieldDmg(int shieldDmgCheck)
    {
        //Try to damage the shield, wont execute if its already broken
        if (shieldDmgCheck > 0)
        {
            if (isDebugLogging) { Debug.Log("DAMAGE RECIEVED TO SHIELD: " + (shield - shieldDmgCheck)); };
            shield = shieldDmgCheck;
            shieldFloat = (float)shield;
            OnShieldDamaged?.Invoke();
        }
        //If value is negative or zero, it means we broke it.
        //Wont run if it was already broken before regenerating 1 shield point
        else if (!isShieldBroken)
        {
            if (isDebugLogging) { Debug.Log("SHIELD WAS BROKEN"); };
            shield = 0;
            shieldFloat = 0;
            isShieldBroken = true;
            OnShieldBroken?.Invoke();
            //Return here to negate damage after shield break
            return true;
        }

        //If the shield was not broken, it means no changes to health, so return and stop here
        //TODO: Ask design if any damage should be negated with shield break, lets say I recieve
        //10 damage with only 3 shield, should the hull loose 7 points?
        //As it is, we negate all damage with shield break
        if (!isShieldBroken)
        {
            return true;
        }

        //If the shield was already broken, return false to execute change to Hull Health
        return false;
    }

    private void ExploreDamageToHull(int healthDmgCheck)
    {
        //If we passed everything above, it means damage to the hull
        if (healthDmgCheck > 0)
        {
            if (isDebugLogging) { Debug.Log("DAMAGE RECIEVED TO HULL: " + (health - healthDmgCheck)); };
            health = healthDmgCheck;
            OnHealthLost?.Invoke();
        }
        //else, the player died
        else
        {
            if (isDebugLogging) { Debug.Log("EVENT: TRIGERRED PLAYER'S DEATH!!!!!"); };
            health = 0;
            OnPlayerDeath?.Invoke();
        }
    }

    private IEnumerator CheckForShieldRestore()
    {
        //Runs until shield regenerated 1 point
        while (shield < 1) 
        {
            //Wait until 1 frame has passed
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isShieldBroken = false;
    }
    

    //Coroutine that runs for the amount of time the player is invincible
    //TODO: Call invlunerabiliy animation here?
    private IEnumerator iFramesTimer()
    {
        if (isDebugLogging) { Debug.Log("STARTED IFRAMES TIMER"); }; //Debugging

        isInvulnerable = true;
        //Runs the iframes timer
        yield return new WaitForSeconds(INVULN_TIME);
        //Player can be hurt again
        isInvulnerable = false;

        //Start the regen of shield again. Take into consideration that the iFrames
        //NOTE: Duration stacks with the regen delay here
        ShieldRoutine = StartCoroutine(ShieldRegenBehaviour());

        if (isDebugLogging) { Debug.Log("IFRAMES TIMER FINISHED"); };
    }

    //Coroutine that handles the regeneration of the shield
    private IEnumerator ShieldRegenBehaviour()
    {
        //Set the regen flag
        isShieldRegening = true;

        /* Create a float holder so regen can be consecutive
         * If we were to add the regen value to an int, we would need to typecast a float, and depending
         * On the number its at, we will loose some regen every few executions
         */

        //First, wait for shield regen delay
        yield return new WaitForSeconds(SHIELD_REGEN_DELAY_TIME);

        if (isDebugLogging) { Debug.Log("STARTED REGEN SHIELD BEHAVIOUR"); };

        //Start Regen of the shield
        while (shieldFloat < MAX_SHIELD)
        {
            shieldFloat += shieldPercentageVal * Time.deltaTime;
            //Assign the float value to player's shiled, typecasted
            shield = (int)shieldFloat;
            //Wait until 1 frame has passed
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //We finished regening the shield, assign it MAX_SHIELD in case we went over before
        shield = MAX_SHIELD;
        shieldFloat = (float)shield;
        isShieldRegening = false;

        if (isDebugLogging) { Debug.Log("REGEN SHIELD BEHAVIOUR FINISHED"); };
    }
}
