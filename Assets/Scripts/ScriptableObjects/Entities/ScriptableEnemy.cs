using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Scriptable Object that contains the prefab data to the Basic Enemy entity 
/// </summary>
[CreateAssetMenu(fileName = "ScriptableEnemyData", menuName = "ScriptableObjects/Enemy")]
public class ScriptableEnemy : ScriptableObject
{
    // Stats of scripted enemy
    [Header("Enemy Stats")]
    public int health;
    public int shield;
    public int collisionDamage;
    public float dmgInvulnTimeSecs; // in seconds
    public ScriptableWeapon weapon;
    public EnemyType enemyType;
}
