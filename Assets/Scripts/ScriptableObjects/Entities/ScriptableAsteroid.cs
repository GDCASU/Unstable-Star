using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Scriptable Object that contains the data to the Asteroid entity 
/// </summary>
[CreateAssetMenu(fileName = "ScriptableAsteroidData", menuName = "ScriptableObjects/Asteroid")]
public class ScriptableAsteroid : ScriptableObject
{
    // Stats of scripted enemy
    [Header("Asteroid Stats")]
    public bool isInvulnerable;
    public int health;
    public int shield;
    public int collisionDamage;
    public float movementSpeed;
}
