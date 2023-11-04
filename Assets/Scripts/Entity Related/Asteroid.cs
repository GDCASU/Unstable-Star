using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> An Asteroid. Inherits from the "CombatEntity" class </summary>
public class Asteroid : CombatEntity
{
    //Enemy Values
    [Header("Asteroid Stats Readings")]
    [SerializeField] private int health;

    private void Awake()
    {
        //Set stats
        health = 5;

        //Add WhenPlayerDies so it listens to the event OnPlayerDeath
        EventData.OnPlayerDeath += WhenPlayerDies;
    }

    //Execute instructions for when player dies
    public override void WhenPlayerDies()
    {
        //Stub, maybe asteroids just go their merry way offscreen?
    }

    public override void TakeDamage(int damage)
    {
        int healthCheck = health - damage;
        
        //Check if the asteroid has been destroyed
        if (healthCheck <= 0)
        {
            HitpointsRenderer.Instance.PrintDamage(this.transform.position, health, false);
            health = 0;
            
            //TODO: Program everything that happens when the entity dies
            Destroy(this.gameObject);
            return;
        }
        
        //Asteroid is not destroyed yet
        HitpointsRenderer.Instance.PrintDamage(this.transform.position, health - healthCheck, false);
        health = healthCheck;
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    //Damage the other entity we collided with
    public override void TakeCollisionDamage(Collider other)
    {
        //NOTE: I had to separate the collider for entities and the collider for
        //projectiles, so if someone uses rigidbodies on enemies or player, they wont get
        //pushed around by the asteroid hitting them

        if (onCooldown) { return; }

        //If collided againt the player, take into account their invulnerability
        if (other.TryGetComponent<Player>(out var playerStats))
        {
            if (playerStats.isInvulnerable) { return; }
        }

        //else, attempt to damage it
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            //Collision damage amount is defined in CombatEntity.cs
            damageable.TakeDamage(CollisionDamage.dmg);
        }

        //Starts Cooldown Routine
        StartCoroutine(CollisionCooldown());
    }
}
