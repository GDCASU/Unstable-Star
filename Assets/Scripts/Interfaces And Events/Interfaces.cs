using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface to implement if the object can be damaged by bullets
public interface IDamageable
{
    void TakeDamage(int damage);
}

//interface to implement if the object reacts to the death of the player
public interface IListensToPlayerDeath
{
    void WhenPlayerDies();
}

public interface IConsumable
{
    //TODO: See if design wants power ups and the like
}
