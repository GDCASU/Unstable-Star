using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> An Asteroid. Inherits from the "CombatEntity" class </summary>
public class Asteroid : CombatEntity
{
    //Local Variables
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private bool disableMovement;

    private void Start()
    {
        //Remember to look into the CombatEntity class to see what variables
        //Should be kept track of or re-set here if needed

        //Set stats if not set on the inspector
        if (health <= 0)
        {
            health = 20;
        }
        if (collisionDamage <= 0)
        {
            collisionDamage = 3;
        }
    }

    private void Update()
    {
        if (!disableMovement)
        {
            MoveDown();
        }
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

    private void MoveDown()
    {
        transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);
    }

}
