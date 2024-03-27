using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script that sets the interaction matrix among layers in the game </summary>
public class PhysicsConfig : MonoBehaviour
{
    //Bool to disable Physics Sets for debugging
    bool disablePhysicsConfig = false;

    //Singleton
    public static PhysicsConfig Get;

    //Lists
    private List<int> EnemyRelatedLayers = new List<int>();
    private List<int> PlayerRelatedLayers = new List<int>();
    private List<int> HazardRelatedLayers = new List<int>();
    private List<int> AllLayers = new List<int>();

    //Layer Integers
    public int DefaultLayer { get; private set; }
    public int DefaultProjectileLayer { get; private set; }
    public int ProjectilesPlayer { get; private set; }
    public int ProjectilesEnemies { get; private set; }
    public int PlayerLayer { get; private set; }
    public int EnemyLayer { get; private set; }
    public int HazardLayer { get; private set; }

    private void Awake()
    {
        // Handle Singleton
        if (Get != null) { Destroy(gameObject); }
        Get = this;

        //Sets the numbers using the names of the layers, so its not hardcoded
        DefaultLayer = LayerMask.NameToLayer("Default");
        DefaultProjectileLayer = LayerMask.NameToLayer("Default Projectile Layer");
        ProjectilesPlayer = LayerMask.NameToLayer("Projectiles Player");
        ProjectilesEnemies = LayerMask.NameToLayer("Projectiles Enemies");
        PlayerLayer = LayerMask.NameToLayer("Player");
        EnemyLayer = LayerMask.NameToLayer("Enemy");
        HazardLayer = LayerMask.NameToLayer("Hazard");

        //Dont change anything on the physics matrix if disabled
        if (disablePhysicsConfig) { return; }

        //Populate List of all layers (Skipping Deletion Zone and some others that are unused right now)
        AllLayers.Add(DefaultLayer);
        AllLayers.Add(DefaultProjectileLayer);
        AllLayers.Add(ProjectilesPlayer);
        AllLayers.Add(ProjectilesEnemies);
        AllLayers.Add(PlayerLayer);
        AllLayers.Add(EnemyLayer);
        AllLayers.Add(HazardLayer);

        //Populate List of all layers that belong to enemies
        EnemyRelatedLayers.Add(ProjectilesEnemies);
        EnemyRelatedLayers.Add(EnemyLayer);

        //Populate List of all layers that belong to the player
        PlayerRelatedLayers.Add(ProjectilesPlayer);
        PlayerRelatedLayers.Add(PlayerLayer);

        //Populate list of all layers belonging to the Hazards
        HazardRelatedLayers.Add(HazardLayer);


        /***************  END LISTS SETTINGS  *******************/


//Default -----------------------------------------------

        // Make default projectile layer ignore all other layers
        IgnoreAllListedLayers(targetLayer: DefaultProjectileLayer);

// Player -----------------------------------------------

        //Ignores collisions between player projectiles and the player itself
        IgnoreCollisionsAmongPlayers();

// Hazards ---------------------------------------------

        //Ignore collisions between Hazards, will go through each other
        IgnoreCollisionsAmongHazards();

// Enemies ---------------------------------------------

        //Ignore collisions between enemies, will go through each other
        IgnoreCollisionsAmongEnemies();
    }

    #region LOGIC HELPER FUNCTIONS

    private void IgnoreCollisionsAmongEnemies()
    {
        for (int i = 0; i < EnemyRelatedLayers.Count; i++)
        {
            for (int j = i; j < EnemyRelatedLayers.Count; j++)
            {
                Physics.IgnoreLayerCollision(EnemyRelatedLayers[i], EnemyRelatedLayers[j]);
            }
        }
    }

    private void IgnoreCollisionsAmongPlayers()
    {
        for (int i = 0; i < PlayerRelatedLayers.Count; i++)
        {
            for (int j = i; j < PlayerRelatedLayers.Count; j++)
            {
                Physics.IgnoreLayerCollision(PlayerRelatedLayers[i], PlayerRelatedLayers[j]);
            }
        }
    }

    private void IgnoreCollisionsAmongHazards()
    {
        for (int i = 0; i < HazardRelatedLayers.Count; i++)
        {
            for (int j = i; j < HazardRelatedLayers.Count; j++)
            {
                Physics.IgnoreLayerCollision(HazardRelatedLayers[i], HazardRelatedLayers[j]);
            }
        }
    }

    private void IgnoreAllListedLayers(int targetLayer)
    {
        for (int i = 0; i < AllLayers.Count; i++)
        {
            Physics.IgnoreLayerCollision(targetLayer, AllLayers[i]);
        }
    }

    #endregion
}