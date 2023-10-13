using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    //Headers for inspector
    //[Header("Debugging")]
    //[SerializeField] private bool detectForeigner = false;
    //[SerializeField] private bool printCollisionCheck = false;
    //[SerializeField] private bool printWhenDeleted = false;

    [Header("Current Data")]
    public string creator = "";
    public float currentSpeed;
    public float currentDamage;
    public bool isThisTracking;

    //Local Variables
    public float speed;
    public float damage;

    //NOTE: Check with design if there's more world objects other than enemies and asteroids




    //Should be called by the creator
    public void SetData(string creator, float speed, float damage)
    {
        this.creator = creator;
        /* Taken from the list of tags in the project
         * List of Entities:
         * "Player"
         * "Enemy"
         * "Asteroid"
         */
    }

    private void FixedUpdate()
    {
        
    }


}
