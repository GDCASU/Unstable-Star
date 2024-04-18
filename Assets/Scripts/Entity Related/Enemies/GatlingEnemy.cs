using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GatlingEnemy : Enemy
{
    [SerializeField] private GatlingCrosshair crosshair;
    [SerializeField] private Transform weaponAnchor;

    private bool moveLeft = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

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
}
