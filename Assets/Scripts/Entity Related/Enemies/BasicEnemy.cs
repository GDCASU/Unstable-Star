using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicEnemy : Enemy
{
    private bool moveLeft = false;

    /// <summary>
    /// Inheriting states from Enemy; move and shoot state
    /// </summary>
    protected override void Update()
    {
        base.Update();

        Move();
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


}
