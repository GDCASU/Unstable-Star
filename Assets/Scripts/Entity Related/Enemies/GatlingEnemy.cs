using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GatlingEnemy : Enemy
{
    [SerializeField] private GatlingCrosshair crosshair;
    [SerializeField] private Transform weaponAnchor;
    [SerializeField] private Vector3 worldUp = Vector3.up;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        Move();
        MoveGun();
        if (canShoot)
        {
            shootComponent.ShootWeapon(currWeapon);
            StartCoroutine(ShootDelayCo());
        }
    }

    /// <summary>
    /// Moves the Basic Enemy left and right across the screen
    /// </summary>
    protected override void Move()
    {
        base.Move();

        
    }

    private void MoveGun()
    {
        Vector3 direction = Vector3.Normalize(crosshair.transform.position - weaponAnchor.position);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

        weaponAnchor.rotation = rotation;        
    }
}
