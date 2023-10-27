using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] float playerViewWidth = 50f; //Serialize
    bool spawnAsteroid = true;
    [SerializeField] GameObject asteroid;
    float maxXPos;
    float asteroidWidth;
    // Start is called before the first frame update
    void Start()
    {
        asteroidWidth = asteroid.GetComponent<MeshRenderer>().bounds.size.x;
        maxXPos = transform.position.x + (playerViewWidth / 2) - (asteroidWidth / 2); //subtract asteriods width
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawner()
    {
        while (spawnAsteroid)
        {
            initiateAsteroid();
            yield return new WaitForSeconds(3f + Random.Range(0f, 3f));
            
        }
        yield return null;
    }

    public void initiateAsteroid() {
        float yPos = transform.position.y;
        float xPos = transform.position.x + (Random.Range(-1f, 1f) * maxXPos);
        Vector3 position = new Vector3(xPos, yPos, 0);
        GameObject newAsteroid = Instantiate(asteroid, position, Quaternion.identity);
    }
    
}
