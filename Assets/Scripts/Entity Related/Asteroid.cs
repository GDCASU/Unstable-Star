using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An Asteroid. Inherits from the "CombatEntity" class
public class Asteroid : CombatEntity
{
    //Enemy Values
    [Header("Asteroid Stats Readings")]
    [SerializeField] private int health;
    [SerializeField] private int damage;

    //Local Variables

    //TODO: Ask if design wants asteroids to become harder with time
    private float healthMult;
    private float damageMult;

    private void Awake()
    {
        //Set stats
        health = 5;
        damage = 3; //Moderate damage

        //Add WhenPlayerDies so it listens to the event OnPlayerDeath
        EventData.OnPlayerDeath += WhenPlayerDies;
    }

    //Execute instructions for when player dies
    public override void WhenPlayerDies()
    {
        //Stub, maybe asteroids just go their merry way offscreen?
    }

    public override void TakeDamage(int damage)
    {
        int healthCheck = health - damage;
        
        //Check if the asteroid has been destroyed
        if (healthCheck <= 0)
        {
            HitpointsRenderer.Instance.PrintDamage(this.transform.position, health, false);
            health = 0;
            
            //TODO: Program everything that happens when the entity dies
            Destroy(this.gameObject);
            return;
        }
        
        //Asteroid is not destroyed yet
        HitpointsRenderer.Instance.PrintDamage(this.transform.position, health - healthCheck, false);
        health = healthCheck;
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    //On collision try to damage entity
    //Refer to Physics Sets to see what can asetroids interact with
    private void OnTriggerEnter(Collider other)
    {
        //NOTE: I had to separate the collider for entities and the collider for
        //projectiles, so if someone uses rigidbodies on enemies or player, they wont get
        //pushed around by the asteroid hitting them

        //Collided with something, attempt to damage it
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(this.damage);
        }
    }
}
