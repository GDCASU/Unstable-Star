using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script Used by the boundary boxes, destroys anything it touches
//Also sets physics ignores here
public class BoundaryBoxesScript : MonoBehaviour
{
    private void Start()
    {
        Physics.IgnoreLayerCollision(6, 6); //Ignores collisions between projectiles
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
