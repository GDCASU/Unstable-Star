using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
}

//TODO: Maybe use an interface to easily see what objects react to the death of the player?
public interface IReactToPlayerDeath
{
    void OnPlayerDeath();
}
