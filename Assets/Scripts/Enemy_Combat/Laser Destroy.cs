using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroy : MonoBehaviour
{

    GameObject laser;
    float y;

    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        DestroyLaser();
    }

    void DestroyLaser()
    {
        if (laser.transform.position.y < -40)
        {
            Destroy(laser);
        }
    }
    void OnDestroy()
    {
        gameObject.SetActive(false);
    }
}
