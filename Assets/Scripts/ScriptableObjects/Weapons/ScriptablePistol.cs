using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the prefab data to a pisto </summary>
[CreateAssetMenu(fileName = "ScriptablePistol", menuName = "ScriptableObjects/Weapons/Pistol")]
public class ScriptablePistol : ScriptableWeapon
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
        return new Pistol(bulletPrefab, weaponIcon, sound, bulletSpeed, damage, weaponName, shotsCooldown);
    }
}
