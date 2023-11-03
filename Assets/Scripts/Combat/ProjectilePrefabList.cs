using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Used by Shoot.cs to access all possible bullet prefabs </summary>
public class ProjectilePrefabList : MonoBehaviour
{
    //All possible bullet prefabs must be added here
    [Header("All Possible Laser Prefabs")]
    public GameObject RedBullet;
    public GameObject YellowBullet;
    public GameObject PinkBullet;

    //TODO: Maybe consider making a weapon class and store them here too
}
