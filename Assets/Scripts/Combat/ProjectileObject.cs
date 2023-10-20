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
    public float damage;
    private Rigidbody rgbd;

    //Stored Computed Variables for better efficiency and readability
    private readonly float halfPi = Mathf.PI / 2;

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody>();

    }

    //TODO: Check with design if there's more world objects other than enemies and asteroids

    //Should be called by the creator
    public void SetData(string creator, float speed, Quaternion creatorRotation)
    {
        this.creator = creator;
        float zAngle;
        /* Taken from the list of tags in the project
         * List of Entities:
         * "Player"
         * "Enemy"
         * "Asteroid"
         */

        //Set Velocity, Added 90 Degrees in radians since Unity's default Z points to the right
        zAngle = Mathf.Deg2Rad * creatorRotation.eulerAngles.z;
        rgbd.velocity = new Vector3(Mathf.Cos(halfPi + zAngle) * speed, Mathf.Sin(halfPi + zAngle) * speed, 0);
    }

    //TODO: ADD COLLISION DETECTION HERE -------------------------------




    // -----------------------------------------------------------------



    //TODO: ADD TIMER DELETION HERE ------------------------------------




    // -----------------------------------------------------------------
}
