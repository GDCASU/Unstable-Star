using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Abstract class from which all Hazards should derive from </summary>
public abstract class ParentHazard : CombatEntity
{
    //Handles Collisions with other entities, defined here so making new enemies is easier
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
            damageable.TakeDamage(CollisionDamage.dmg, out int dmgRecieved, out bool wasShield);
            HitpointsRenderer.Instance.PrintDamage(other.transform.position, dmgRecieved, wasShield);
        }

        //Starts Cooldown Routine
        StartCoroutine(CollisionCooldown());
    }

    /// <summary> Function that contains all behaviours related to entity death </summary>
    public abstract void OnDeath();
}
