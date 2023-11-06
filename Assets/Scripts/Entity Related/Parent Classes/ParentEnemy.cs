using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Abstract class from which all Enemies should derive from </summary>
public abstract class ParentEnemy : CombatEntity
{
    //Handles Collisions with other entities, defined here so making new enemies is easier
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
            damageable.TakeDamage(CollisionDamage.dmg, out int dmgRecieved, out bool wasShield);
            HitpointsRenderer.Instance.PrintDamage(other.transform.position, dmgRecieved, wasShield);
        }

        //Starts Collision Cooldown routine
        StartCoroutine(CollisionCooldown());
    }

    /// <summary> Function that contains all behaviours related to entity death </summary>
    public abstract void OnDeath();
}
