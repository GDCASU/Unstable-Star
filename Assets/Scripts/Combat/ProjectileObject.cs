using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> Script/Class attached to anything that is a projectile </summary>
public class ProjectileObject : MonoBehaviour
{
    [Header("Current Data")]
    public string creator = "";
    public float currentSpeed = 0f;
    public int damage;
    public float currentDamage;

    //Local Variables

    private void Update()
    {
        this.transform.Translate(currentSpeed * Time.deltaTime * Vector3.up);
    }

    /// <summary> Should be called by the creator and set the projectile data </summary>
    public void SetData(string creator, int LayerInt, float speed, int damage)
    {
        this.damage = damage;
        this.currentSpeed = speed;
        this.gameObject.layer = LayerInt;

        /* Taken from the list of tags in the project
         * List of Entities:
         * "Player"
         * "Enemy"
         * "Asteroid"
         */
        this.creator = creator;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //TODO: Ask design if bullets should also be destroyed if colliding with enemies

        //For anything else, find out if object we collided against can be damaged
        if (collision.gameObject.TryGetComponent<CombatEntity>(out CombatEntity entity))
        {
            // if the entity is ignoring collisions, then continue foward
            if (entity.isIgnoringCollisions) return;
            
            // Else, deal damage
            entity.TakeDamage(this.damage, out int dmgRecieved, out Color colorSet);
            HitpointsRenderer.Instance.PrintDamage(this.transform.position, dmgRecieved, colorSet);
        }

        //Destroy bullet after a collision
        Destroy(this.gameObject);
    }

    // Disable the projectile's collision detection once out of the playing space
    private void OnTriggerEnter(Collider other)
    {
        // Technically this check is not necessary, but im doing it just in case some other object
        // In the future decides to also use trigger colliders
        if (other.gameObject.CompareTag("Screen Bounds"))
        {
            this.gameObject.layer = PhysicsConfig.Get.DefaultLayer;
        }
    }

}
