using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script that sets the interaction matrix among layers in the game </summary>
public class PhysicsSets : MonoBehaviour
{
    private List<int> AllButPlayerProjectiles = new List<int>();
    private List<int> EnemyRelatedLayers = new List<int>();
    private List<int> PlayerRelatedLayers = new List<int>();
    private List<int> AsteroidRelatedLayers = new List<int>();
    private List<int> AllProjectileLayers = new List<int>();

    private void Start()
    {
        //Sets the numbers using the names of the layers, so its not hardcoded
        int Player = LayerMask.NameToLayer("Player");
        int ProjectilesPlayer = LayerMask.NameToLayer("Projectiles Player");
        int ProjectilesEnemies = LayerMask.NameToLayer("Projectiles Enemies");
        int AsteroidsColliderProjectiles = LayerMask.NameToLayer("Asteroid Collider Projectiles");
        int AsteroidsColliderEntities = LayerMask.NameToLayer("Asteroid Collider Entities");
        int EnemyColliderProjectiles = LayerMask.NameToLayer("Enemy Collider Projectiles");
        int EnemyColliderEntities = LayerMask.NameToLayer("Enemy Collider Entities");

        //Populate list of all layers that arent the player's projectiles
        AllButPlayerProjectiles.Add(Player);
        AllButPlayerProjectiles.Add(ProjectilesEnemies);
        AllButPlayerProjectiles.Add(AsteroidsColliderEntities);
        AllButPlayerProjectiles.Add(AsteroidsColliderProjectiles);
        AllButPlayerProjectiles.Add(EnemyColliderProjectiles);
        AllButPlayerProjectiles.Add(EnemyColliderEntities);

        //Populate List of all layers that belong to enemies
        EnemyRelatedLayers.Add(ProjectilesEnemies);
        EnemyRelatedLayers.Add(EnemyColliderProjectiles);
        EnemyRelatedLayers.Add(EnemyColliderEntities);

        //Populate List of all layers that belong to the player
        PlayerRelatedLayers.Add(Player);
        PlayerRelatedLayers.Add(ProjectilesPlayer);

        //Populate list of all layers belonging to the asteroids
        AsteroidRelatedLayers.Add(AsteroidsColliderEntities);
        AsteroidRelatedLayers.Add(EnemyColliderProjectiles);

        //Populate list of all layers belonging to projectiles
        AllProjectileLayers.Add(ProjectilesPlayer);
        AllProjectileLayers.Add(ProjectilesEnemies);


        // Player -------------------------------------------------

        //Ignores collisions between player projectiles and the player itself
        IgnoreCollisionsAmongPlayers();


        // Asteroids ---------------------------------------------


        //Ignore collisions in the Projectile Collider with anything that isnt the player projectiles
        IgnoreAllButPlayerProjectiles(AsteroidsColliderProjectiles);

        //Ignore collisions on the Entity Collider with anything that is a projectile
        IgnoreAllProjectiles(AsteroidsColliderEntities);

        //Ignore collisions between asteroids, will go through each other
        IgnoreCollisionsAmongAsteroids();


        // Enemies -----------------------------------------------


        //Ignore collisions in the Projectile Collider between anything else that isnt the player projectiles
        IgnoreAllButPlayerProjectiles(EnemyColliderProjectiles);

        //Ignore collisions on the Entity Collider with anything that is a projectile
        IgnoreAllProjectiles(EnemyColliderEntities);

        //Ignore collisions between enemies, will go through each other
        IgnoreCollisionsAmongEnemies();

    }

    //Even ignores contact with the same collider
    private void IgnoreAllButPlayerProjectiles(int targetLayer)
    {
        for (int i = 0; i < AllButPlayerProjectiles.Count; i++)
        {
            Physics.IgnoreLayerCollision(AllButPlayerProjectiles[i], targetLayer);
        }
    }

    private void IgnoreAllProjectiles(int targetLayer)
    {
        for (int i = 0; i < AllProjectileLayers.Count; i++)
        {
            Physics.IgnoreLayerCollision(AllProjectileLayers[i], targetLayer);
        }
    }

    private void IgnoreCollisionsAmongEnemies()
    {
        for (int i = 0; i < EnemyRelatedLayers.Count; i++)
        {
            for (int j = i + 1; j < EnemyRelatedLayers.Count; j++)
            {
                Physics.IgnoreLayerCollision(EnemyRelatedLayers[i], EnemyRelatedLayers[j]);
            }
        }
    }

    private void IgnoreCollisionsAmongPlayers()
    {
        for (int i = 0; i < PlayerRelatedLayers.Count; i++)
        {
            for (int j = i + 1; j < PlayerRelatedLayers.Count; j++)
            {
                Physics.IgnoreLayerCollision(PlayerRelatedLayers[i], PlayerRelatedLayers[j]);
            }
        }
    }

    private void IgnoreCollisionsAmongAsteroids()
    {
        for (int i = 0; i < AsteroidRelatedLayers.Count; i++)
        {
            for (int j = i + 1; j < AsteroidRelatedLayers.Count; j++)
            {
                Physics.IgnoreLayerCollision(AsteroidRelatedLayers[i], AsteroidRelatedLayers[j]);
            } 
        }
    }
}
