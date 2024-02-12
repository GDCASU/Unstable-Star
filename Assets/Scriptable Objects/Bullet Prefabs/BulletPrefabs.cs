using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the prefab data to the bullets in game </summary>
[CreateAssetMenu(fileName = "BulletPrefabData", menuName = "ScriptableObjects/BulletPrefabData")]
public class BulletPrefabs : ScriptableObject
{
    [Header("All Possible Bullet Prefabs")]
    public GameObject RedBullet;
    public GameObject GreenBullet;
    public GameObject YellowBullet;

    //Helper function for returning the prefab associated with the weapon
    public GameObject GetPrefab(Weapon inputWeapon)
    {
        switch (inputWeapon.color)
        {
            case BulletColors.Red:
                return RedBullet;
            case BulletColors.Green:
                return GreenBullet;
            case BulletColors.Yellow:
                return YellowBullet;
            default:
                return null;
        }
    }
}
