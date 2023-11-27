using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> An Asteroid. Inherits from the "CombatEntity" class </summary>
public class Asteroid : CombatEntity
{
    //Local Variables

    private void Start()
    {
        //Remember to look into the CombatEntity class to see what variables
        //Should be kept track of or re-set here if needed

        //Set stats
        health = 20;
        collisionDamage = 3;
    }

    //Execute instructions for when player dies
    protected override void WhenPlayerDies()
    {
        //Stub, maybe asteroids just go their merry way offscreen?
    }

    protected override void TriggerDeath()
    {
        //TODO: DEFINE WHAT HAPPENS WHEN THE ASTEROID IS DESTROYED



        // -----------------------

        StopAllCoroutines();
        Destroy(this.gameObject);
    }

}
