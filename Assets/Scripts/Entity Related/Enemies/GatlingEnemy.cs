using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

/// <summary> A gatling Enemy. Inherits from the "CombatEntity" class </summary> P.S. most of this is copied from BasicEnemy.cs
public class GatlingEnemy : CombatEntity
{
    [Header("Enemy Parameters")]
    [SerializeField] private GameObject WeaponAnchor;
    public bool testShoot = true;

    //Local variables
    private ShootScript shootComponent;
    private Weapon currWeapon;

    private void Start()
    {
        //Remember to look into the CombatEntity class to see what variables
        //Should be kept track of or re-set here if needed

        //Initialize Component
        shootComponent = GetComponent<ShootScript>();
        shootComponent.InitializeData(WeaponAnchor);

        //Set variables
        health = 10;
        shield = 5;
        currWeapon = new Gatling(15f, 1, "Enemy Gatling Gun", 0.1f);
    }

    private void Movement(){
        
    }


    //Testing. (┬┬﹏┬┬)
    private void Update()
    {
        if (testShoot)
        {
            shootComponent.ShootWeapon(currWeapon);
            //testShoot = false;
        }
    }

     //Execute instructions for when player dies
    protected override void WhenPlayerDies()
    {
        //Stub
    }

    // Behaviours on death
    protected override void TriggerDeath()
    {
        //TODO: DEFINE WHAT HAPPENS WHEN ENEMY DIES
        //TODO: It should also increase the kill counter here



        // -----------------------

        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}
