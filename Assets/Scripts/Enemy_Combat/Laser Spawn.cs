using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//Old system. Only use as reference. Delete later//
public class LaserSpawn : MonoBehaviour
{
    bool enemyDeath = false;
    float spawnRate = 3f;
    float laserSpeed;
    Vector3 enemyPosition;
    float lastSpawn = 0f;

    [SerializeField] public GameObject laser;
    [SerializeField] public GameObject enemy;


    private void Start()
    {
        enemyPosition = enemy.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyDeath)
        {
            SpawnLaser();
        }
        
    }
    public void SpawnLaser() 
    { 
        if (lastSpawn < spawnRate)
        {
            Debug.Log(lastSpawn);
            lastSpawn += Time.deltaTime;
        }
        else
        {
            laser = Instantiate(laser, enemyPosition - new Vector3(0, 4, 0), new Quaternion(0, 0, 0, 0));
            lastSpawn = 0;
        }
    }


}
