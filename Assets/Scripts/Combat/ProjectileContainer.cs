using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Stores all scene projectiles under the object this script is placed in 
/// </summary>
public class ProjectileContainer : MonoBehaviour
{
    //Singleton
    public static ProjectileContainer Instance;

    private void Awake()
    {
        // Handle Singleton
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;
    }

}
