using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
}

//TODO: Maybe use an interface to easily see what objects react to the death of the player?
public interface IListensToPlayerDeath
{
    void WhenPlayerDies();
}

public interface IConsumable
{
    //TODO: See if design wants power ups and the like
}
