using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> Script/Class attached to anything that is a projectile </summary>
public class ProjectileObject : MonoBehaviour
{
    [Header("Current Data")]
    public string creator = "";
    public float currentSpeed;
    public int damage;
    public float currentDamage;
    public bool isThisTracking;

    //Local Variables
    private Rigidbody rgbd;

    //Stored Computed Variables for better efficiency and readability
    private readonly float halfPi = Mathf.PI / 2;

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody>();
    }
    //TODO: Check with design if there's more world objects other than enemies and asteroids

    /// <summary> Should be called by the creator and set the projectile data </summary>
    public void SetData(string creator, int LayerInt, float speed, int damage, Quaternion creatorRotation)
    {
        this.creator = creator;
        this.damage = damage;
        this.gameObject.layer = LayerInt;
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

    private void OnTriggerEnter(Collider other)
    {
        //TODO: Ask design if bullets should also be destroyed if colliding with enemies

        //For anything else, find out if object we collided against can be damaged
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(this.damage, out int dmgRecieved, out bool wasShield);
            HitpointsRenderer.Instance.PrintDamage(this.transform.position, dmgRecieved, wasShield);
        }

        //Destroy bullet after collision
        Destroy(this.gameObject);
    }

}
