using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

/// <summary> A basic Enemy. Inherits from the "CombatEntity" class </summary>
public class BasicEnemy : CombatEntity
{
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
    private ShootScript shootComponent;
    private Weapon currWeapon;

    private void Start()
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
    private void Update()
    {
        if (canShoot)
        {
            shootComponent.ShootWeapon(currWeapon);
            StartCoroutine(ShootDelayCo());
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
