using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that manages all the stats conected to the player
public class Player : CombatEntity
{
    //Singleton for any other scripts that interact with the player, like UI (?)
    public static Player Instance;

    //Readonly vars, has to be assigned here
    //TODO: Ask design if Max health and shield can be upgraded one day
    private readonly int MAX_HEALTH = 10;
    private readonly int MAX_SHIELD = 5;
    //how much % of the shield is regenerated every second, format = 15%, 30%, etc.
    private readonly float SHIELD_PERCENTAGE_REGEN_BASE = 20f;
    private readonly float INVULN_TIME = 1f; // in seconds
    private readonly float SHIELD_REGEN_DELAY_TIME = 3f; // in seconds, STACKS WITH INVULN_TIME!!

    [Header("Player Stats Readings")]
    [SerializeField] private int HealthRead;
    [SerializeField] private int ShieldRead;
    [SerializeField] private float ShieldFloatRead;
    [SerializeField] private bool IsShieldBrokenRead;
    [SerializeField] private bool IsInvulnerableRead;
    [SerializeField] private bool IsRegeningRead;

    [Header("Debugging")]
    [SerializeField] private bool IsDebugLogging;
    [SerializeField] private bool DamageTest;
    [SerializeField] private bool HealTest;

    //Local variables
    private Coroutine ShieldRoutine;
    public int Health { get; private set; }
    public int Shield { get; private set; }
    private float shieldPercentageVal;
    private bool isInvulnerable;
    private bool isShieldBroken;
    private bool isShieldRegening;
    private float shieldFloat;

    private void Start()
    {
        //Sets the singleton
        Instance = this;

        //Add WhenPlayerDies action to the event OnPlayerDeath
        EventData.OnPlayerDeath += WhenPlayerDies;

        //Set Variables
        ShieldRoutine = null;
        Health = MAX_HEALTH;
        Shield = MAX_SHIELD;
        shieldFloat = Shield;
        isInvulnerable = false;
        isShieldBroken = false;
        isShieldRegening = false;
        changeShieldRegenPercentage(SHIELD_PERCENTAGE_REGEN_BASE);
    }

    //Update's only purpose is debugging, everything coded here runs on
    //Take damage, independent of update
    private void Update()
    {
        //Testing
        HealthRead = Health;
        ShieldRead = Shield;
        IsRegeningRead = isShieldRegening;
        ShieldFloatRead = shieldFloat;
        IsShieldBrokenRead = isShieldBroken;
        IsInvulnerableRead = isInvulnerable;
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
        Health += amount;
        if (Health > MAX_HEALTH)
        {
            Health = MAX_HEALTH;
        }
        //Invoke the event signaling a change of health
        if (IsDebugLogging) { Debug.Log("ATTEMPTED TO HEAL THE PLAYER"); };
        EventData.RaiseOnHealthAdded();
    }

    //TODO: See if design plans to have buff items or something of the like
    public void changeShieldRegenPercentage(float newPercentage)
    {
        shieldPercentageVal = (MAX_SHIELD * newPercentage) / 100;
        if (IsDebugLogging) { Debug.Log("CHANGED HEALING PERCENTAGE TO " + shieldPercentageVal); };
    }

    //Damage recieved, declared as per contract with IDamageable interface
    //TODO: Ask design if projectiles should be deleted if colliding against
    //an invulnerable player, or let them phase through (?) 
    public override void TakeDamage(int damage)
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
        int shieldDmgCheck = Shield - damage;
        int healthDmgCheck = Health + shieldDmgCheck;
        bool triggeredReturn;

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
            if (IsDebugLogging) { Debug.Log("DAMAGE RECIEVED TO SHIELD: " + (Shield - shieldDmgCheck)); };
            Shield = shieldDmgCheck;
            shieldFloat = (float)Shield;
            EventData.RaiseOnShieldDamaged();
        }
        //If value is negative or zero, it means we broke it.
        //Wont run if it was already broken before regenerating 1 shield point
        else if (!isShieldBroken)
        {
            if (IsDebugLogging) { Debug.Log("SHIELD WAS BROKEN"); };
            Shield = 0;
            shieldFloat = 0;
            isShieldBroken = true;
            EventData.RaiseOnShieldBroken();
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
            if (IsDebugLogging) { Debug.Log("DAMAGE RECIEVED TO HULL: " + (Health - healthDmgCheck)); };
            Health = healthDmgCheck;
            EventData.RaiseOnHealthLost();
        }
        //else, the player died
        else
        {
            if (IsDebugLogging) { Debug.Log("EVENT: TRIGERRED PLAYER'S DEATH!!!!!"); };
            Health = 0;
            EventData.RaiseOnPlayerDeath();
        }
    }

    public override void WhenPlayerDies()
    {
        //NOTE: after death triggers, TakeDamage() wont respond to incoming damage anymore
        StopAllCoroutines();
        isInvulnerable = true;

        //TODO: Add here events that happens when player dies ---------------



        //-----------------------------------------------------------------------
    }

    private IEnumerator CheckForShieldRestore()
    {
        //Runs until shield regenerated 1 point
        while (Shield < 1)
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
        if (IsDebugLogging) { Debug.Log("STARTED IFRAMES TIMER"); }; //Debugging

        isInvulnerable = true;
        //Runs the iframes timer
        yield return new WaitForSeconds(INVULN_TIME);
        //Player can be hurt again
        isInvulnerable = false;

        //Start the regen of shield again. Take into consideration that the iFrames
        //NOTE: Duration stacks with the regen delay here
        ShieldRoutine = StartCoroutine(ShieldRegenBehaviour());

        if (IsDebugLogging) { Debug.Log("IFRAMES TIMER FINISHED"); };
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

        if (IsDebugLogging) { Debug.Log("STARTED REGEN SHIELD BEHAVIOUR"); };

        //Start Regen of the shield
        while (shieldFloat < MAX_SHIELD)
        {
            shieldFloat += shieldPercentageVal * Time.deltaTime;
            //Assign the float value to player's shiled, typecasted
            Shield = (int)shieldFloat;
            //Wait until 1 frame has passed
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //We finished regening the shield, assign it MAX_SHIELD in case we went over before
        Shield = MAX_SHIELD;
        shieldFloat = (float)Shield;
        isShieldRegening = false;

        if (IsDebugLogging) { Debug.Log("REGEN SHIELD BEHAVIOUR FINISHED"); };
    }
}
