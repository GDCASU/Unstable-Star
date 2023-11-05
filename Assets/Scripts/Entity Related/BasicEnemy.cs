using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

//On Visual studio, press F12 over "CombatEntity" to open that script

//A basic Enemy. Inherits from the "ParentEnemy" class
public class BasicEnemy : ParentEnemy
{
    //Enemy Values
    [Header("Enemy Stats Readings")]
    public int health;

    //Local Variables

    //TODO: Ask if design wants enemies to become harder with time
    private float damageMultiplier;
    private float healthMultiplier;

    private void Awake()
    {
        //Set stats
        health = 5;

        //Add WhenPlayerDies so it listens to the event OnPlayerDeath
        EventData.OnPlayerDeath += WhenPlayerDies;
    }

    public override void TakeDamage(int damage)
    {
        int healthCheck = health - damage;

        if (healthCheck <= 0)
        {
            HitpointsRenderer.Instance.PrintDamage(this.transform.position, health, false);

            //Enemy died
            OnDeath();
            return;
        }

        //Else, the enemy still is alive after hit
        HitpointsRenderer.Instance.PrintDamage(this.transform.position, health - healthCheck, false);
        health -= damage;
    }

    //Execute instructions for when player dies
    public override void WhenPlayerDies()
    {
        //Stub
    }

    // Behaviours on death
    public override void OnDeath()
    {
        //TODO: DEFINE WHAT HAPPENS WHEN ENEMY DIES
        //TODO: It should also increase the kill counter here



        // -----------------------

        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

}
