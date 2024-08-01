using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script Used by the Deletion Zones, destroys anything it touches </summary>
public class DeletionZonesScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
}
