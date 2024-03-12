using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the prefab data to a Laser weapon </summary>
[CreateAssetMenu(fileName = "ScriptableLaserGun", menuName = "ScriptableObjects/Weapons/Laser Gun")]
public class ScriptableLaserGun : ScriptableWeapon
{
    // Stats related to this weapon
    [Header("Data")]
    public string weaponName;
    public GameObject bulletPrefab;
    public Sprite weaponIcon;
    public SoundTag sound;

    [Header("Stats")]
    public int damage;
    public float bulletSpeed;
    public float shotsCooldown;

    public override Weapon GetWeaponObject()
    {
        return new LaserGun(bulletPrefab, weaponIcon, sound, bulletSpeed, damage, weaponName, shotsCooldown);
    }
}
