using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : CombatEntity
{
    [Header("Physics Variables")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private float enterSpeed = 20f;
    private bool moveDown = true;
    private bool moveLeft = false;
    private float percentUpScreen = 0.9f;

    // Should hold the Stat data of this enemy
    [Header("Scriptable Data Object")]
    [Tooltip("If ignoreDataHolder is true, the enemy will have the data set in the inspector")]
    [SerializeField] private bool ignoreDataHolder;
    [SerializeField] private ScriptableEnemy statsData;

    //Local Variables
    [Header("Enemy Parameters")]
    [SerializeField] private GameObject WeaponAnchor;
    [SerializeField] private float shootDelay = 1f;

    //Local variables
    [HideInInspector] public bool canShoot = false;
    private bool enterScreen = true;
    private bool exitScreen = false;
    bool inScreen = false;
    private ShootScript shootComponent;
    private Weapon currWeapon;

    protected virtual void Start()
    {
        //Remember to look into the CombatEntity class to see what variables
        //Should be kept track of or re-set here if needed

        //Initialize Component
        shootComponent = GetComponent<ShootScript>();
        shootComponent.InitializeData(WeaponAnchor);

        //Set Stats from data holder if not ignored
        if (!ignoreDataHolder)
        {
            health = statsData.health;
            shield = statsData.shield;
            collisionDamage = statsData.collisionDamage;
            dmgInvulnTime = statsData.dmgInvulnTimeSecs;
        }
        // The weapon must be loaded from data however
        currWeapon = statsData.weapon.GetWeaponObject();
    }

    //Testing
    protected virtual void Update()
    {
        if (enterScreen)
        {
            //EnterScreen();
            if (moveDown) EnterScreenSpace();
        }
        if (exitScreen)
        {
            ExitScreen();
            enterScreen = false; // makes sure exit and enter don't play at same time
        }
        if (inScreen)
        {
            Move();
            if (canShoot)
            {
                shootComponent.ShootWeapon(currWeapon);
                StartCoroutine(ShootDelayCo());
            }
        }

        if (!inScreen && Camera.main.WorldToScreenPoint(gameObject.transform.position).y <= Camera.main.pixelHeight)
        {
            // variable that tells system when ship is in camera view
            inScreen = true;
            canShoot = true;
        }
    }

    protected virtual void Move()
    {
        if (moveLeft)
        {
            //transform.Translate(Vector3.left * Time.deltaTime * speed);
            transform.position = transform.position + new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
            moveLeft = Camera.main.WorldToViewportPoint(transform.position).x > 0f;
        }
        else
        {   // Move right
            //transform.Translate(Vector3.right * Time.deltaTime * speed);
            transform.position = transform.position + new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
            moveLeft = Camera.main.WorldToViewportPoint(transform.position).x > 1f;
        }
    }

    protected virtual void EnterScreenSpace()
    {
        transform.Translate(Vector3.down * Time.deltaTime * enterSpeed);                        // Move Down.
        moveDown = Camera.main.WorldToViewportPoint(transform.position).y > percentUpScreen;    // Check if should move down again.

        //if (!moveDown) GetComponent<EnemyHealth>().ToggleInvulnerable(false);                    // toggle invulnerable off
    }

    protected virtual void ExitScreen() // method that causes ship to exit screen
    {
        
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


        EventData.RaiseOnEnemyDeath(gameObject);
        // -----------------------

        StopAllCoroutines();
        StartCoroutine(DestroyEnemyAfterCallCo());
    }

    private IEnumerator ShootDelayCo()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    private IEnumerator DestroyEnemyAfterCallCo()
    {
        yield return new WaitUntil(() => EventData.RaiseOnEnemyDeath(gameObject));
        Destroy(this.gameObject);
    }
}
