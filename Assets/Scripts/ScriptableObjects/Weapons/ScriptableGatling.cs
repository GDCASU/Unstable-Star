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
    public FMODUnity.EventReference sound;
    [TextAreaAttribute]
    public string description;

    [Header("Stats")]
    public float heatUpTime;
    public int damage;
    public float bulletSpeed;
    public float timeBetweenShots;

    [Header("Enemy Specific")]
    [Tooltip("This time indicates how long the enemy will keep shooting the gatling gun before stopping")]
    public float shootStayTime;

    public override Weapon GetWeaponObject()
    {
        return new GatlingGun(bulletPrefab, weaponIcon, sound, bulletSpeed, heatUpTime, damage, weaponName, timeBetweenShots, shootStayTime, description);
    }
}
