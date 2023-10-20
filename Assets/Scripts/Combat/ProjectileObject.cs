using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
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
        Physics.IgnoreLayerCollision(6, 6); //Ignores collisions between projectiles
        //TODO: ask design if there's projectiles that collide with other projectiles
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

    private void OnCollisionEnter(Collision collision)
    {
        string foreignerTag = collision.gameObject.tag;

        //Return if the object is of the same type
        if (this.CompareTag(foreignerTag))
        {
            return;
            //TODO: Ask design if enemies can kill each other
        }
        
        
        switch (foreignerTag)
        {
            case "Player":
                //Register Hit on player
                PlayerRegisterHit(collision.gameObject);
                break;
            case "Enemy":
                //Register Hit on Enemy
                EnemyRegisterHit(collision.gameObject);
                break;
            case "Asteroid":
                //Register Hit on Asteroit, TODO: Ask design if asteroids can be destroyed
                AsteroidRegisterHit(collision.gameObject);
                break;
            default:
                //FIXME: Ignore Collision???
                break;
        }
    }

    private void PlayerRegisterHit(GameObject foreignerObject)
    {
        //TODO:
    }

    private void EnemyRegisterHit(GameObject foreignerObject)
    {
        //TODO:
        Vector3 previousVelocity = rgbd.velocity;
        Destroy(foreignerObject);
        rgbd.velocity = previousVelocity;
    }

    private void AsteroidRegisterHit(GameObject foreignerObject)
    {
        //TODO:
    }


    // -----------------------------------------------------------------



    //TODO: ADD TIMER DELETION HERE ------------------------------------




    // -----------------------------------------------------------------
}
