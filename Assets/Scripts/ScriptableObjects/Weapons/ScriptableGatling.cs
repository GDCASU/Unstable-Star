using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the prefab data to a birdshot weapon </summary>
[CreateAssetMenu(fileName = "ScriptableGatling", menuName = "ScriptableObjects/Weapons/Gatling")]
public class ScriptableGatling : ScriptableWeapon
{
    // Stats related to this weapon
    [Header("Data")]
    public string weaponName;
    public GameObject bulletPrefab;
    public Sprite weaponIcon;
    public SoundTag sound;

    [Header("Stats")]
    public float warmUpTime;
    public int damage;
    public float bulletSpeed;
    public float timeBetweenShots;

    [Header("Enemy Specific")]
    public bool isEnemy;
    [Tooltip("This time indicates how long the enemy will keep shooting the gatling gun before stopping")]
    public float shootStayTime;

    public override Weapon GetWeaponObject()
    {
        // Set for an enemy
        if (isEnemy)
        {
            return new GatlingGun(bulletPrefab, weaponIcon, sound, bulletSpeed, warmUpTime, damage, weaponName, timeBetweenShots, isEnemy, shootStayTime);
        }
        // Set for a player
        return new GatlingGun(bulletPrefab, weaponIcon, sound, bulletSpeed, warmUpTime, damage, weaponName, timeBetweenShots, isEnemy, shootStayTime * 0);
    }
}
