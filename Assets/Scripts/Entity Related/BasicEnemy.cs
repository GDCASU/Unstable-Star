using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

//On Visual studio, press F12 over "CombatEntity" to open that script

//A basic Enemy. Inherits from the "CombatEntity" class
public class BasicEnemy : CombatEntity
{
    //Enemy Values
    [Header("Enemy Stats Readings")]
    public int health;

    //Local Variables

    //TODO: Ask if design wants enemies to become harder with time
    private float damageMultiplier;
    private float healthMultiplier;

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
        //Stub
    }

    public override void TakeDamage(int damage)
    {
        int healthCheck = health - damage;

        if (healthCheck <= 0)
        {
            HitpointsRenderer.Instance.PrintDamage(this.transform.position, health, false);

            //TODO: DEFINE WHAT HAPPENS WHEN ENEMY DIES
            //TODO: It should also increase the kill counter here

            Destroy(this.gameObject);
            return;
        }

        //Else, the enemy still is alive after hit
        HitpointsRenderer.Instance.PrintDamage(this.transform.position, health - healthCheck, false);
        health -= damage;
    }

    public override void TakeCollisionDamage(Collider other)
    {
        //NOTE: I had to separate the collider for entities and the collider for
        //projectiles, so the player and enemy rigidbody wont get pushed around by collisions

        if (onCooldown) { return; }

        //Take into account if the player is invulnerable
        if (other.TryGetComponent<Player>(out var playerStats))
        {
            if (playerStats.isInvulnerable) { return; }
        }

        //Damage other entity, the other entity should deal damage back
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(CollisionDamage.dmg);
        }

        //Starts Collision Cooldown routine
        StartCoroutine(CollisionCooldown());
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

}
