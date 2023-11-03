using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script Used by the boundary boxes, destroys anything it touches </summary>
public class BoundaryBoxesScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
}
