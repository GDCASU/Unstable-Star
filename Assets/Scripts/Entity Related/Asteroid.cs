using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> An Asteroid. Inherits from the "ParentHazard" class </summary>
public class Asteroid : ParentHazard
{
    //Enemy Values
    [Header("Asteroid Stats Readings")]
    [SerializeField] private int health;

    private void Awake()
    {
        //Set stats
        health = 5;

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

            //Asteroid Destroyed
            OnDeath();
            
            return;
        }
        
        //Asteroid is not destroyed yet
        HitpointsRenderer.Instance.PrintDamage(this.transform.position, health - healthCheck, false);
        health = healthCheck;
    }

    public override void OnDeath()
    {
        //TODO: Program everything that happens when the entity dies



        // -------------------------
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }
}
