using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the prefab data to the weapons in game </summary>
[CreateAssetMenu(fileName = "ScriptableWeaponData", menuName = "ScriptableObjects/Weapon")]
public class ScriptableWeapon : ScriptableObject
{
    // Stats related to this weapon
    [Header("Weapon Stats")]
    public GameObject bulletPrefab;
    public BehaviourTypes behaviour;
    public string weaponName;
    public int damage;
    public float bulletSpeed;
    public float shotsCooldown;

    /// <summary>
    /// Helper function to construct a weapon object from stored data
    /// </summary>
    public Weapon GetWeaponObject()
    {
        return new GeneralWeapon(bulletPrefab, behaviour, bulletSpeed, damage, weaponName, shotsCooldown);
    }
}
