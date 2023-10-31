using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that sets the interaction matrix among layers in the game
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

        //Ignores collisions between enemy projectiles
        Physics.IgnoreLayerCollision(ProjectilesEnemies, ProjectilesEnemies);

        //Ignores collisions between player projectiles
        Physics.IgnoreLayerCollision(ProjectilesPlayer, ProjectilesPlayer);

        //Ignores collisions between enemies, will go through each other
        Physics.IgnoreLayerCollision(Enemies, Enemies);

        //Ignore collisions between player and its own bullets
        Physics.IgnoreLayerCollision(ProjectilesPlayer, Player);

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






        //Ignores collisions between Player projectiles and enemy projectiles
        //NOTE: By nature of projectiles being on trigger, this shouldnt be neccesary
        //But im doing it anyways in case we make a bullet that doesnt use OnTrigger
        //in the future
        Physics.IgnoreLayerCollision(ProjectilesEnemies, ProjectilesPlayer);
    }
}
