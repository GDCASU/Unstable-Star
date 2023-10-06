using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Laser Prefabs")]
    [SerializeField] private GameObject defaultLaser;
    [SerializeField] private List<GameObject> Lasers;
    
    //[SerializeField] private GameObject currentLaser;

    //Assign Laser that is being currently being shot
    void Awake()
    {
        
    }

    
    void FixedUpdate()
    {
        
    }
}
