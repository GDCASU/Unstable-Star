using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script that sets the interaction matrix among layers in the game </summary>
public class PhysicsSets : MonoBehaviour
{
    private List<int> AllButPlayerProjectiles = new List<int>();
    private List<int> EnemyRelatedLayers = new List<int>();
    private List<int> PlayerRelatedLayers = new List<int>();
    private List<int> HazardRelatedLayers = new List<int>();
    private List<int> AllProjectileLayers = new List<int>();
    private List<int> AllLayers = new List<int>();

    private void Start()
    {
        //Sets the numbers using the names of the layers, so its not hardcoded
        int Player = LayerMask.NameToLayer("Player");
        int DefaultProjectileLayer = LayerMask.NameToLayer("Default Projectile Layer");
        int ProjectilesPlayer = LayerMask.NameToLayer("Projectiles Player");
        int ProjectilesEnemies = LayerMask.NameToLayer("Projectiles Enemies");
        int HazardColliderProjectiles = LayerMask.NameToLayer("Hazard Collider Projectiles");
        int HazardColliderEntities = LayerMask.NameToLayer("Hazard Collider Entities");
        int EnemyColliderProjectiles = LayerMask.NameToLayer("Enemy Collider Projectiles");
        int EnemyColliderEntities = LayerMask.NameToLayer("Enemy Collider Entities");

        //Populate List of all layers (Skipping default and some that are unused right now)
        AllLayers.Add(Player);
        AllLayers.Add(DefaultProjectileLayer);
        AllLayers.Add(ProjectilesPlayer);
        AllLayers.Add(ProjectilesEnemies);
        AllLayers.Add(HazardColliderEntities);
        AllLayers.Add(HazardColliderProjectiles);
        AllLayers.Add(EnemyColliderProjectiles);
        AllLayers.Add(EnemyColliderEntities);

        //Populate list of all layers that arent the player's projectiles
        AllButPlayerProjectiles.Add(Player);
        AllButPlayerProjectiles.Add(ProjectilesEnemies);
        AllButPlayerProjectiles.Add(HazardColliderEntities);
        AllButPlayerProjectiles.Add(HazardColliderProjectiles);
        AllButPlayerProjectiles.Add(EnemyColliderProjectiles);
        AllButPlayerProjectiles.Add(EnemyColliderEntities);

        //Populate List of all layers that belong to enemies
        EnemyRelatedLayers.Add(ProjectilesEnemies);
        EnemyRelatedLayers.Add(EnemyColliderProjectiles);
        EnemyRelatedLayers.Add(EnemyColliderEntities);

        //Populate List of all layers that belong to the player
        PlayerRelatedLayers.Add(Player);
        PlayerRelatedLayers.Add(ProjectilesPlayer);

        //Populate list of all layers belonging to the Hazards
        HazardRelatedLayers.Add(HazardColliderEntities);
        HazardRelatedLayers.Add(EnemyColliderProjectiles);

        //Populate list of all layers belonging to projectiles
        AllProjectileLayers.Add(ProjectilesPlayer);
        AllProjectileLayers.Add(ProjectilesEnemies);

        /****************************************************************/


        // Make default projectile layer ignore all other layers -------------
        IgnoreAllLayers(DefaultProjectileLayer);

        // Player -------------------------------------------------

        //Ignores collisions between player projectiles and the player itself
        IgnoreCollisionsAmongPlayers();

        // Hazards ---------------------------------------------

        //Ignore collisions in the Projectile Collider with anything that isnt the player projectiles
        IgnoreAllButPlayerProjectiles(HazardColliderProjectiles);

        //Ignore collisions on the Entity Collider with anything that is a projectile
        IgnoreAllProjectiles(HazardColliderEntities);

        //Ignore collisions between Hazards, will go through each other
        IgnoreCollisionsAmongHazards();

        // Enemies -----------------------------------------------

        //Ignore collisions in the Projectile Collider between anything else that isnt the player projectiles
        IgnoreAllButPlayerProjectiles(EnemyColliderProjectiles);

        //Ignore collisions on the Entity Collider with anything that is a projectile
        IgnoreAllProjectiles(EnemyColliderEntities);

        //Ignore collisions between enemies, will go through each other
        IgnoreCollisionsAmongEnemies();
    }

    //Ignore all layers
    private void IgnoreAllLayers(int targetLayer)
    {
        for (int i = 0; i < AllLayers.Count; i++)
        {
            Physics.IgnoreLayerCollision(targetLayer, AllLayers[i]);
        }
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

    //HACK: I know there must be a more efficient way to iterate over these
    //But it works for now
    private void IgnoreCollisionsAmongEnemies()
    {
        for (int i = 0; i < EnemyRelatedLayers.Count; i++)
        {
            for (int j = 0; j < EnemyRelatedLayers.Count; j++)
            {
                Physics.IgnoreLayerCollision(EnemyRelatedLayers[i], EnemyRelatedLayers[j]);
            }
        }
    }

    private void IgnoreCollisionsAmongPlayers()
    {
        for (int i = 0; i < PlayerRelatedLayers.Count; i++)
        {
            for (int j = 0; j < PlayerRelatedLayers.Count; j++)
            {
                Physics.IgnoreLayerCollision(PlayerRelatedLayers[i], PlayerRelatedLayers[j]);
            }
        }
    }

    private void IgnoreCollisionsAmongHazards()
    {
        for (int i = 0; i < HazardRelatedLayers.Count; i++)
        {
            for (int j = 0; j < HazardRelatedLayers.Count; j++)
            {
                Physics.IgnoreLayerCollision(HazardRelatedLayers[i], HazardRelatedLayers[j]);
            } 
        }
    }
}
