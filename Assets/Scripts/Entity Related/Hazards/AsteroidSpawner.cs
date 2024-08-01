using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // Singleton Reference
    public static AsteroidSpawner Instance;

    [SerializeField] float playerViewWidth = 50f; //Serialize
    public bool spawnAsteroid = true;
    [SerializeField] GameObject[] asteroids;
    [SerializeField] MeshRenderer generalAsteriodMesh;
    float maxXPos;
    float asteroidWidth;

    [SerializeField] bool testSpawnOnStart = false;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        asteroidWidth = generalAsteriodMesh.bounds.size.x;
        maxXPos = transform.position.x + (playerViewWidth / 2) - (asteroidWidth / 2); //subtract asteriods width

        if (testSpawnOnStart) // FOR DEBUGGING PURPOSES; DO NOT ENABLE IN FINAL GAME
            StartSpawn();
    }

    public void StartSpawn()
    {
        spawnAsteroid = true;
        StartCoroutine(Spawner());
    }

    public void StopSpawn()
    {
        spawnAsteroid = false;
        StopCoroutine(Spawner());
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

        int randIndex = Random.Range(0, asteroids.Length);
        GameObject newAsteroid = Instantiate(asteroids[randIndex], position, Quaternion.identity);
    }
    
}
