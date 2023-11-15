using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Interface to implement if the object can be damaged by bullets </summary>
public interface IDamageable
{
    void TakeDamage(int damageIn, out int dmgRecieved, out Color colorSet);
}

public interface IConsumable
{
    //TODO: See if design wants power ups and the like
}
