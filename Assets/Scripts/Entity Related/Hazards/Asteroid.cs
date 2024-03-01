using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> An Asteroid. Inherits from the "CombatEntity" class </summary>
public class Asteroid : CombatEntity
{
    // Should hold the Stat data of this asteroid
    [Header("Scriptable Data Object")]
    [Tooltip("If ignoreDataHolder is true, the enemy will have the data set in the inspector")]
    [SerializeField] private bool ignoreDataHolder;
    [SerializeField] private ScriptableAsteroid statsData;

    // Stats
    [Header("Asteroid Stats")]
    [SerializeField] private float movementSpeed;

    // Debugging
    [Header("Debugging")]
    [SerializeField] private bool disableMovement;

    //Local Variables
    

    private void Start()
    {
        //Set Stats from data holder if not ignored
        if (!ignoreDataHolder)
        {
            health = statsData.health;
            shield = statsData.shield;
            collisionDamage = statsData.collisionDamage;
            movementSpeed = statsData.movementSpeed;
            isInvulnerable = statsData.isInvulnerable;
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
        transform.Translate(movementSpeed * Time.deltaTime * Vector3.down);
    }

}
