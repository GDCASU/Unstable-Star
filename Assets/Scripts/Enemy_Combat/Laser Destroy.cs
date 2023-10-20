using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Old system. Only use as reference. Delete later//
public class LaserDestroy : MonoBehaviour
{

    GameObject laser;
    float yBoundTop = 10;
    float yBoundBottom = -40;
    float xBoundRight = 20;
    float xBoundLeft = -20;

    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        PositionDestroyLaser();
    }

    void PositionDestroyLaser()
    {
        if (laser.transform.position.y < yBoundBottom && laser.transform.position.y > yBoundTop &&
            laser.transform.position.x < xBoundRight && laser.transform.position.x > xBoundLeft)
        {
            DestroyImmediate(laser);
        }
    }
    void OnDestroy()
    {
        laser.SetActive(false);
    }
}
