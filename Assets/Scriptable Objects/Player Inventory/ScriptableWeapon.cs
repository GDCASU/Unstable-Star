using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the prefab data to the bullets in game </summary>
[CreateAssetMenu(fileName = "ScriptableWeaponData", menuName = "ScriptableObjects/New Scriptable Weapon")]
public class ScriptableWeapon : ScriptableObject
{
    // Stats related to this weapon
    [Header("Weapon Stats")]
    public string weaponName;
    public float bulletSpeed;
    public float shotsCooldown;
    public int damage;
    public BulletColors color;
    public BehaviourTypes behaviour;

    /// <summary>
    /// Helper function to construct a weapon object from stored data
    /// </summary>
    public Weapon GetWeaponObject()
    {
        return new GeneralWeapon(color, behaviour, bulletSpeed, damage, weaponName, shotsCooldown);
    }
}
