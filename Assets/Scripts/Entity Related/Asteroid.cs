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

    public override void TakeDamage(int damageIn, out int dmgRecieved, out bool wasShield)
    {
        int healthCheck = health - damageIn;
        wasShield = false; //This would change if enemy has a shield

        if (healthCheck <= 0)
        {
            dmgRecieved = health; //out var set
            //Enemy died
            OnDeath();
            return;
        }

        //Else, the enemy still is alive after hit
        dmgRecieved = health - healthCheck;
        health -= damageIn;
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
