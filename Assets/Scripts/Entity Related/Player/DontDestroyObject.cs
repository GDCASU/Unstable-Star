using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Script only purpose right now is to protect the player's Stats from wiping
public class DontDestroyObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
