using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

/// <summary> A basic Enemy. Inherits from the "CombatEntity" class </summary>
public class BasicEnemy : CombatEntity
{
    //Local Variables
    [Header("Enemy Parameters")]
    [SerializeField] private GameObject WeaponAnchor;
    public bool testShoot;

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
        health = 5;
        shield = 5;
        currWeapon = new Pistol(BulletColors.Red ,15f, 1, "Enemy Pistol", 0.2f);
    }

    //Testing
    private void Update()
    {
        if (testShoot)
        {
            shootComponent.ShootWeapon(currWeapon);
            testShoot = false;
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
