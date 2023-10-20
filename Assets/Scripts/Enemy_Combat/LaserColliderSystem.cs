using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/* A collider system for the lasers. Damages objects that
 * are not its allies.
 */

public class LaserColliderSystem : MonoBehaviour
{
    ProjectileObject projectileStats;

    // Start is called before the first frame update
    void Start()
    {
        projectileStats = GetComponent<ProjectileObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (projectileStats.creator != collision.gameObject.tag && collision.gameObject.tag != "Projectile")
        {
            Destroy(this.gameObject,.2f);
            if (collision.gameObject.tag == "Player")
            {
                //gameObject.PlayerHealthSystemIDK.health -= projectile.damage;
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                //gameObject.EnemyHealthSystemIDK.health -= projectile.damage;
            }
            else if (collision.gameObject.tag == "Asteroid")
            {
                //gameObject.AsteroidHealthSystemIDK.health -= projectile.damage;
            }
            
        }
    }
}
