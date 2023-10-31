using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class from which all entities involved in combat should derive
public abstract class CombatEntity : MonoBehaviour, IDamageable, IListensToPlayerDeath
{
    //Function called when player dies
    public abstract void WhenPlayerDies();

    //The take damage function used by the combat system
    public abstract void TakeDamage(int damage);
}
