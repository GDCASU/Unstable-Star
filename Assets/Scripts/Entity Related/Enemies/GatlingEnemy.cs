using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GatlingEnemy : Enemy
{
    [SerializeField] private GatlingCrosshair crosshair;
    [SerializeField] private Transform weaponAnchor;
    [SerializeField] private GameObject gatlingParent;

    private bool moveLeft = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(WaitToEnterScreen());
    }

    protected override void Update()
    {
        base.Update();

        // Dont do anything until position is reached
        if (moveDown)
        {
            Move();
            return;
        }

        if (canShoot)
        {
            shootComponent.ShootWeapon(currWeapon);
            StartCoroutine(ShootDelayCo());
        }

        if (crosshair.followPlayer)
        {
            Move();
            MoveGun();
        }
    }

    /// <summary>
    /// Keeps the gatling crosshair on the enemy until we reached the stopping point in the screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitToEnterScreen()
    {
        // Wait for ship to reach position
        while (moveDown)
        {
            // Keep crosshair close
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, crosshair.transform.position.z);
            crosshair.transform.position = newPos;
            yield return null;
        }

        // Reached position
        crosshair.canFollowPlayer = true;
        crosshair.followPlayer = true;
    }

    /// <summary>
    /// Moves the Basic Enemy left and right across the screen
    /// </summary>
    protected override void Move()
    {
        base.Move();

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

    private void MoveGun()
    {
        Vector3 direction = Vector3.Normalize(crosshair.transform.position - weaponAnchor.position);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

        weaponAnchor.rotation = rotation;        
    }

    protected override void TriggerDeath()
    {
        EventData.RaiseOnEnemyDeath(gatlingParent);

        StopAllCoroutines();

        // Explosion Effect
        Instantiate(deathEffectPrefab, this.transform.position, Quaternion.identity);

        // Destroy Enemy Object
        Destroy(gatlingParent);
    }
}
