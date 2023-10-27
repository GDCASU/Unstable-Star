using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used by ShootSpawningBehaviour to access all possible laser prefabs
public class LaserPrefabList : MonoBehaviour
{
    //ALL POSSIBLE LASERS MUST BE ADDED HERE
    [Header("All Possible Laser Prefabs")]
    public GameObject RedLaser;         //String = "Red" 
    public GameObject YellowLaser;      //String = "Yellow"
    public GameObject PinkLaser;      //String = "Pink"
}
