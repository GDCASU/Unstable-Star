using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that sets the interactions among layers in the game
public class PhysicsSets : MonoBehaviour
{
    private void Start()
    {
        //Ignores collisions between projectiles
        Physics.IgnoreLayerCollision(6, 6); 

        //Ignores collisions between enemies, will go through each other
        Physics.IgnoreLayerCollision(7, 7); 
    }
}
