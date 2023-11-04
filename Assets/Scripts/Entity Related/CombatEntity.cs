using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Abstract class from which all entities involved in combat should derive </summary>
public abstract class CombatEntity : MonoBehaviour, IDamageable, IListensToPlayerDeath
{
    /// <summary> Function called when player dies </summary>
    public abstract void WhenPlayerDies();

    /// <summary> The TakeDamage function used by the combat system </summary>
    public abstract void TakeDamage(int damage);

    /// <summary> Handle Collision Damage, will be called by CombatParentRef </summary>
    public abstract void TakeCollisionDamage(Collider other);

    /// <summary> Used by player to collide with other entities </summary>
    public void OnTriggerEnter(Collider other)
    {
        TakeCollisionDamage(other);
    }

    //Cooldown of Impact Collisions
    protected bool onCooldown = false;
    protected IEnumerator CollisionCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(CooldownTime.time);
        onCooldown = false;
    }
}

//Collision Damage among entities
public static class CollisionDamage { public static readonly int dmg = 2; }
public static class CooldownTime { public static readonly float time = 1f; }