using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GatlingEnemy : Enemy
{
    [SerializeField] private GatlingCrosshair crosshair;
    [SerializeField] private GameObject Gun;
    [SerializeField] private Vector3 worldUp = Vector3.up;

    private ShootComponent GatlingShootComp;

    protected override void Start()
    {
        base.Start();
        GatlingShootComp = GetComponent<ShootComponent>();
    }

    protected override void Update()
    {
        base.Update();
        MoveGun();
    }

    private void MoveGun()
    {
        // Rotate the gun
        Gun.transform.LookAt(crosshair.transform.position, worldUp);
    }
}
