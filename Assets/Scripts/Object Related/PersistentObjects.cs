using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that will protect all objects that are meant to be present on all scenes
/// </summary>
public class PersistentObjects : MonoBehaviour
{
    // Ian: There's nothing to access here, but DontDestroyOnLoad only works on root
    // Objects, so if we need to nest them, they need to be in an object were dont destroy is called
    public static PersistentObjects instance;

    private void Awake()
    {
        // Handle singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
