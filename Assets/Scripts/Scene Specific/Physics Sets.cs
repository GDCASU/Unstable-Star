using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script that sets the interaction matrix among layers in the game </summary>
public class PhysicsSets : MonoBehaviour
{
    private void Start()
    {
        //Sets the numbers using the names of the layers, so its not hardcoded
        int Player = LayerMask.NameToLayer("Player");
        int Enemies = LayerMask.NameToLayer("Enemies");
        int ProjectilesPlayer = LayerMask.NameToLayer("Projectiles Player");
        int ProjectilesEnemies = LayerMask.NameToLayer("Projectiles Enemies");
        int AsteroidsColliderProjectiles = LayerMask.NameToLayer("Asteroid Collider Projectiles");
        int AsteroidsColliderEntities = LayerMask.NameToLayer("Asteroid Collider Entities");

        //Ignores collisions between enemy projectiles and themselves
        Physics.IgnoreLayerCollision(ProjectilesEnemies, ProjectilesEnemies);
        Physics.IgnoreLayerCollision(ProjectilesEnemies, Enemies);

        //Ignores collisions between player projectiles and the player itself
        Physics.IgnoreLayerCollision(ProjectilesPlayer, ProjectilesPlayer);
        Physics.IgnoreLayerCollision(ProjectilesPlayer, Player);

        //NOTE: Ignores collisions between enemies, will go through each other
        Physics.IgnoreLayerCollision(Enemies, Enemies);

        // Asteroids need a bit of special treatment --------------------------------

        //Ignore collisions in the non-trigger function between anything else that isnt the player projectiles
        Physics.IgnoreLayerCollision(AsteroidsColliderProjectiles, Player);
        Physics.IgnoreLayerCollision(AsteroidsColliderProjectiles, Enemies);
        Physics.IgnoreLayerCollision(AsteroidsColliderProjectiles, ProjectilesEnemies);

        //Ignore collisions on the isTrigger function with anything that is a projectile
        Physics.IgnoreLayerCollision(AsteroidsColliderEntities, ProjectilesEnemies);
        Physics.IgnoreLayerCollision(AsteroidsColliderEntities, ProjectilesPlayer);

        //Ignore collisions between asteroids, will go through each other
        Physics.IgnoreLayerCollision(AsteroidsColliderProjectiles, AsteroidsColliderProjectiles);
        Physics.IgnoreLayerCollision(AsteroidsColliderEntities, AsteroidsColliderEntities);

        // --------------------------------------------------------------------------
    }
}
