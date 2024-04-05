using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the prefab data to a birdshot weapon </summary>
[CreateAssetMenu(fileName = "ScriptableBirdshot", menuName = "ScriptableObjects/Weapons/Birdshot")]
public class ScriptableBirdshot : ScriptableWeapon
{
    // Stats related to this weapon
    [Header("Data")]
    public string weaponName;
    public GameObject bulletPrefab;
    public Sprite weaponIcon;
    public FMODUnity.EventReference sound;
    [TextAreaAttribute]
    public string description;

    [Header("Stats")]
    public int damage;
    public float bulletSpeed;
    public float shotsCooldown;

    public override Weapon GetWeaponObject()
    {
        return new Birdshot(bulletPrefab, weaponIcon, sound, bulletSpeed, damage, weaponName, shotsCooldown, description);
    }
}
