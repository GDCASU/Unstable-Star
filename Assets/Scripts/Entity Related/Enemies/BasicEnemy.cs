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

    private float xRange = 23.5f;
    private float speed = 30.0f;

    Vector2 movementVector;

    private void Start()
    {
        //Remember to look into the CombatEntity class to see what variables
        //Should be kept track of or re-set here if needed

        //Initialize Component
        shootComponent = ShootScript.CreateInstance(WeaponAnchor);

        //Set variables
        health = 5;
        shield = 5;
        currWeapon = new Pistol(15f, 1, "Enemy Pistol", "SingleShot");
    }

    //Testing
    private void Update()
    {
        if (testShoot)
        {
            shootComponent.ShootWeapon(currWeapon);
            testShoot = false;
        }
        moveEnemy();
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

    private void moveEnemy() {
        // Vector3 moveDirection = new Vector3(0,-1,0);
        // float speed = 6.0f;
        // gameObject.transform.position = new Vector2(transform.position.x+0.0001f,transform.position.y);

        //puts player on other side of screen if touching side walls
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector2(xRange, transform.position.y);
            // movementVector = new Vector2(1,0);
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
        if (transform.position.x > xRange)
        {
            transform.position = new Vector2(-xRange, transform.position.y);
            // movementVector = new Vector2(1,0);
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }


        Debug.Log("Time.deltaTime = " + Time.deltaTime);
    }

}
