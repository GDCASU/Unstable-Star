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
}
