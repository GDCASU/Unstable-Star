using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Interface to implement if the object can be damaged by bullets </summary>
public interface IDamageable
{
    void TakeDamage(int damage);
}

/// <summary> Interface to implement if the object reacts to the death of the player </summary>
public interface IListensToPlayerDeath
{
    void WhenPlayerDies();
}

public interface IConsumable
{
    //TODO: See if design wants power ups and the like
}
