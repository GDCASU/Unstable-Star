using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Abstract class for combat entities </summary>
public abstract class CombatEntity : MonoBehaviour, IDamageable
{
    // Static Values
    protected static readonly float collisionCooldownTime = 1f;

    // Serialized Variables
    [SerializeField] protected int health = 0;
    [SerializeField] protected int shield = 0;
    [SerializeField] protected int collisionDamage = 0;
    [SerializeField] protected float timeLeftInvulnerable = 0f;
    [SerializeField] protected float dmgInvulnTime = 0f; // in seconds

    // Public Variables
    public bool isShootingLocked;
    public bool isAbilityLocked;
    public bool isInvulnerable = false;
    public bool isIgnoringCollisions = false;

    // Protected Variables
    protected Coroutine invulnRoutine = null;

    // Local Variables
    private Dictionary<int, CombatEntity> collisionHistory = new();

    protected virtual void Awake()
    {
        // Add WhenPlayerDies action to the event OnPlayerDeath, with this,
        // All entities listen to the event OnPlayerDeath by default
        EventData.OnPlayerDeath += WhenPlayerDies;
    }

    // Unsubscribe from events on destroy
    protected virtual void OnDestroy()
    {
        EventData.OnPlayerDeath -= WhenPlayerDies;
    }

    /// <summary> Function called when player dies </summary>
    protected abstract void WhenPlayerDies();

    // This function should help implementing sounds and effects easier
    /// <summary> To be called when the entity dies. Use to trigger sounds, SFX and more </summary>
    protected abstract void TriggerDeath();

    /// <summary> Blocks the player from shooting its weapon </summary>
    public void LockShooting()
    {
        isShootingLocked = true;
    }

    /// <summary> enables the player to shoot its weapon </summary>
    public void UnlockShooting()
    {
        isShootingLocked = false;
    }

    /// <summary> Blocks the player from using their abilities </summary>
    public void LockAbilities()
    {
        isAbilityLocked = true;
    }

    /// <summary> enables the player to use their abilities </summary>
    public void UnlockAbilities()
    {
        isAbilityLocked = false;
    }

    /// <summary> The TakeDamage function used by the combat system </summary>
    public virtual void TakeDamage(int damageIn, out int dmgRecieved, out Color colorSet)
    {
        // Must contain a value before any return
        dmgRecieved = 0;
        colorSet = Color.white; //Represents shield

        //Dont take damage if invulnerable
        if (isInvulnerable) { return; }

        // Shield check ---------------------------------
        int shieldCheck = shield - damageIn;

        // check if we had enough shield to tank the hit
        if (shieldCheck > 0)
        {
            dmgRecieved = damageIn;
            shield = shieldCheck;
            return;
        }

        // If we didnt and still have shield, reduce damage taken
        if (shield > 0)
        {
            dmgRecieved = shield;
            damageIn -= shield;
            shield = 0;
        }

        // Health check ---------------------------------
        colorSet = Color.magenta;

        int healthCheck = health - damageIn;

        if (healthCheck <= 0)
        {
            dmgRecieved += health; // out var set
            // Enemy died
            TriggerDeath();
            return;
        }

        // Else, the enemy still is alive after hit
        dmgRecieved += health - healthCheck;
        health -= damageIn;
    }

    /// <summary> Handles Collision Damage among entities </summary>
    public virtual void TakeCollisionDamage(CombatEntity other, int damage)
    {
        // Account for this entity being invulnerable or any of the two entities ignoring collisions. We dont check the other entity
        // to allow immortal entities to not take damage while still being able to damage other entities
        if (this.isInvulnerable) return;
        if (this.isIgnoringCollisions || other.isIgnoringCollisions) return;

        int _id = other.GetInstanceID();

        // Check if we have collided against this specific object before
        if (collisionHistory.ContainsKey(_id))
        {
            //We have collided before ending cooldown time, ignore damage
            return;
        }

        //else, Take damage
        this.TakeDamage(damage, out int dmgRecieved, out Color colorSet);
        HitpointsRenderer.Instance.PrintDamage(this.transform.position, dmgRecieved, colorSet);
        collisionHistory.Add(_id, other);

        //Start tracking the collision
        StartCoroutine(TrackCollisionCooldown(_id));

        // Note: This cooldown only provides invulnerability against new collisions, 
        // not incoming damage. Its meant to stop Collision damage spamming unless
        // The entity implements iFrames like the player, which trigger upon recieving any damage
    }

    //Will remember a collision for some time determined by collisionCooldownTime
    private IEnumerator TrackCollisionCooldown(int _id)
    {
        yield return new WaitForSeconds(collisionCooldownTime);
        collisionHistory.Remove(_id);
    }

    // Since all combat entities will have colliders, we handle collisions here
    // also overridable
    protected virtual void OnCollisionEnter(Collision collision)
    {
        // We try to get the CombatEntity Component since we want to ignore non-entities
        if (collision.gameObject.TryGetComponent<CombatEntity>(out CombatEntity other))
        {
            other.TakeCollisionDamage(this, collisionDamage);
        }
    }

    /// <summary>
    /// Triggers Invulnerability Time. <para />
    /// Will override current invulnerability if the input time is bigger than the time left invulnerable <para />
    /// NOTE: The bool ignoreCollisions is optional if the entity needs to fully ignore collisions
    /// </summary>
    public virtual void TriggerInvulnerability(float seconds, bool ignoreCollisions = false)
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
        }

        invulnRoutine = StartCoroutine(iFramesRoutine(seconds, ignoreCollisions));
    }

    // Invulnerability Routine, isDamage is unused on purpose to fix an edge case solved by
    // Overriding this method on the player class
    protected virtual IEnumerator iFramesRoutine(float seconds, bool ignoreCollisions)
    {
        isInvulnerable = true;
        timeLeftInvulnerable = seconds;

        // Runs the iframes timer
        while (timeLeftInvulnerable > 0f)
        {
            timeLeftInvulnerable -= Time.deltaTime;
            // Wait a frame
            yield return null;
        }

        // Player can be hurt again
        timeLeftInvulnerable = 0f;
        isInvulnerable = false;
        invulnRoutine = null;
    }

}