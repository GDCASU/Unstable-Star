using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> Scriptable Object that contains the prefab data to a Laser weapon </summary>
[CreateAssetMenu(fileName = "ScriptableLaserGun", menuName = "ScriptableObjects/Weapons/Laser Gun")]
public class ScriptableLaserGun : ScriptableWeapon
{
    // Stats related to this weapon
    [Header("Data")]
    public string weaponName;
    public GameObject laserPrefab;
    public GameObject chargingSpherePrefab;
    public Sprite weaponIcon;
    public FMODUnity.EventReference sound;
    [TextAreaAttribute]
    public string description;

    [Header("Settings")]
    public int minDamage;
    public int maxDamage;
    [Tooltip("If enemy, This time indicates how long the enemy will wait until firing the laser gun again")]
    public float laserCooldown;
    public float maxChargeUpTime;

    [Header("Enemy Specific")]
    public bool isEnemy;

    public override Weapon GetWeaponObject()
    {
        return new LaserGun(weaponName, laserPrefab, chargingSpherePrefab, weaponIcon, sound, minDamage, maxDamage, laserCooldown, maxChargeUpTime, isEnemy, description);
    }
}
