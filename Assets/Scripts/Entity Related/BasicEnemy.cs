using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

//On Visual studio, press F12 over "CombatEntity" to open that script

//A basic Enemy. Inherits from the "CombatEntity" class
public class BasicEnemy : CombatEntity
{
    //Enemy Values
    [Header("Enemy Stats Readings")]
    [SerializeField] private int health;
    [SerializeField] private int damage;

    //Local Variables

    //TODO: Ask if design wants enemies to become harder with time
    private float damageMultiplier;
    private float healthMultiplier;

    private void Awake()
    {
        //Set stats
        health = 5; 
        damage = 1; //This value should be used by enemy AI

        //Add WhenPlayerDies so it listens to the event OnPlayerDeath
        EventData.OnPlayerDeath += WhenPlayerDies;
    }

    //Execute instructions for when player dies
    public override void WhenPlayerDies()
    {
        //Stub
    }

    public override void TakeDamage(int damage)
    {
        int healthCheck = health - damage;
        
        
        if (healthCheck <= 0)
        {
            HitpointsRenderer.Instance.PrintDamage(this.transform.position, health, false);

            //TODO: DEFINE WHAT HAPPENS WHEN ENEMY DIES
            //TODO: It should also increase the kill counter here

            Destroy(this.gameObject);
            return;
        }

        //Else, the enemy still is alive after hit
        HitpointsRenderer.Instance.PrintDamage(this.transform.position, health - healthCheck, false);
        health -= damage;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

}
