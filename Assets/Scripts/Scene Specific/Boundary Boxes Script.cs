using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script Used by the boundary boxes, destroys anything it touches
public class BoundaryBoxesScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
}
