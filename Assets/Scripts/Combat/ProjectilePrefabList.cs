using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used by Shoot.cs to access all possible laser prefabs
public class ProjectilePrefabList : MonoBehaviour
{
    //All possible bullet prefabs must be added here
    [Header("All Possible Laser Prefabs")]
    public GameObject RedBullet;
    public GameObject YellowBullet;
    public GameObject PinkBullet;
}
