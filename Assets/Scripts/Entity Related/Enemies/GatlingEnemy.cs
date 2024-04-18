using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GatlingEnemy : Enemy
{
    [SerializeField] private GatlingCrosshair crosshair;
    [SerializeField] private GameObject Gun;

    private ShootComponent GatlingShootComp;

    protected override void Start()
    {
        base.Start();
        Debug.Log("here");
        GatlingShootComp = GetComponent<ShootComponent>();
    }

    protected override void Update()
    {
        base.Update();
        MoveGun();
    }

    private void MoveGun()
    {
        // Create a normalized vector between the gatling gun and the crosshair
        Vector2 direction = Vector3.Normalize(crosshair.transform.position - transform.position);
        Debug.Log(direction);
        // Rotate the gun
        Gun.transform.LookAt(direction);
    }
}
