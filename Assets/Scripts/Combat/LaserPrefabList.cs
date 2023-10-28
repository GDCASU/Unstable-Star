using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used by ShootSpawningBehaviour to access all possible laser prefabs
public class LaserPrefabList : MonoBehaviour
{
    //ALL POSSIBLE LASERS MUST BE ADDED HERE
    [Header("All Possible Laser Prefabs")]
    public GameObject PistolBullet;         //String = "Pistol" 
    public GameObject BirdshotBullet;      //String = "Birdshot"
    public GameObject BuckshotBullet;      //String = "Buckshot"
}
