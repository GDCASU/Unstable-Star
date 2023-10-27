using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Laser Prefabs")]
    [SerializeField] private List<string> laserChoices;
    [SerializeField] private string defaultLaserName;
    //TODO: Figure out how to make a selection list of choices instead of an input list

    [Header("Testing vars")]
    public bool spawn = false;

    //Local variables
    private string sCurrLaser;
    private ShootSpawningBehaviour projectileBehaviour;

    private void Awake()
    {
        projectileBehaviour = GetComponent<ShootSpawningBehaviour>();
        SetLaserFire(defaultLaserName);
    }


    //HACK: Testing loop
    private void Update()
    {
        if (spawn)
        {
            ShootProjectile();
            spawn = false;
        }
    }

    //Assign Laser that is currently being shot
    //HACK: Find a better way to set this?
    public void SetLaserFire(string laserType)
    {
        //NOTE: The string must have something contained within the name of the prebab
        //So if you input "Red", it should set the Red Laser
        int i;

        //Go through list, find laserType
        for (i = 0; i < laserChoices.Count; i++)
        {
            if (laserChoices[i].Equals(laserType))
            {
                sCurrLaser = laserChoices[i];
                return;
            }
        }
        Debug.Log("ERROR! LaserType String passed in Shoot.cs wasnt found within the available lasers");
    }

    //Function to call everytime player shoots
    //It also creates the laser with its designed behaviour
    public void ShootProjectile()
    {
        projectileBehaviour.BehaviourSpawn(sCurrLaser);
    }

}
