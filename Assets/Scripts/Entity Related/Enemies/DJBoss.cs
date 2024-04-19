using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DJBoss : Boss
{
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _shieldModel;
    [SerializeField] private GameObject[] _enemyPool;

    private const int MAX_HEALTH = 75;
    private const float MIN_SHIELD_X = -35f;
    private const float MAX_SHIELD_X = 35f;
    private const float MIN_SHIELD_Y = -10f;
    private const float MAX_SHIELD_Y = 15f;
    private const float VULNERABLE_TIME = 7f;
    private const int MAX_ENEMIES = 5;
    private const float ENEMY_SPAWN_Y = 50f;

    private List<GameObject> _shieldList = new List<GameObject>();
    private List<GameObject> _enemyList = new List<GameObject>();

    private int _vulnerablePhaseCount = 0;
    private Coroutine _spawnEnemiesCoro;

    private void Start()
    {
        _shieldModel.SetActive(false);
    }

    public override void BeginFight()
    {
        health = MAX_HEALTH;
		EventData.OnEnemyDeath += OnEnemyDeath;
        _shieldModel.SetActive(true);

        StartCoroutine(ShieldPhases());
    }

    private IEnumerator ShieldPhases()
    {
        // TODO: remove
        //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.I));

        while(health > 0)
        {
            Log("Invulnerable phase");
            _shieldModel.SetActive(true);
            isInvulnerable = true;

            // Number shields generators starts at 2 then increases by 1 for each vulnerable phase reached, up to a max of 5
            for(int i = 0; i < Math.Min(_vulnerablePhaseCount + 2, 5); i++)
            {
                Vector3 shieldPos = Vector3.zero;
                // Generate a random position for the shield until it is not within close proximity of another shield or 100 attempts
                for(int attempts = 0; attempts < 100; attempts++)
                {
                    shieldPos = new Vector3(Random.Range(MIN_SHIELD_X, MAX_SHIELD_X), Random.Range(MIN_SHIELD_Y, MAX_SHIELD_Y));
                    if(_shieldList.Find((GameObject o) => Vector3.Distance(o.transform.position, shieldPos) < 15f) == null)
                    {
                        break;
                    }
                }
                CreateShield(shieldPos);
                yield return new WaitForSeconds(0.25f);
            }
            // Spawn enemies after spawning the shields
            _spawnEnemiesCoro = StartCoroutine(SpawnEnemies());

            yield return new WaitUntil(() => _shieldList.Count == 0);

            Log("Vulnerable phase");
            _shieldModel.SetActive(false);
            _vulnerablePhaseCount++;
            isInvulnerable = false;
            if(_spawnEnemiesCoro != null) StopCoroutine(_spawnEnemiesCoro);

            yield return new WaitForSeconds(VULNERABLE_TIME);
        }
    }

    private void CreateShield(Vector3 position)
	{
        Log("Spawning shield at " + position);
        GameObject shield = Instantiate(_shieldPrefab, position, Quaternion.identity);
        shield.GetComponent<DJShield>().OnShieldDestroyed += OnShieldDestroyed;
        _shieldList.Add(shield);
        if (!_shieldModel.activeInHierarchy) _shieldModel.SetActive(true);
    }

	private void OnShieldDestroyed(GameObject obj)
	{
        if(_shieldList.Contains(obj)) _shieldList.Remove(obj);
    }

    private IEnumerator SpawnEnemies()
    {
        while(isInvulnerable)
        {
            // Spawn a random enemy from the pool
            Log("Spawning enemies");
            while(_enemyList.Count < MAX_ENEMIES)
			{
                GameObject enemy = Instantiate(_enemyPool[Random.Range(0, _enemyPool.Length)],
                new Vector3(Random.Range(MIN_SHIELD_X, MAX_SHIELD_X), ENEMY_SPAWN_Y), Quaternion.identity);
                _enemyList.Add(enemy);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitUntil(() => _enemyList.Count < MAX_ENEMIES);
            // Delay before it can spawn a new wave of enemies so there's actually a point to killing the enemies
            yield return new WaitForSeconds(25f);
        }
    }

    // TODO - search the enemy list for the number of enemies that are of a certain type
    private int GetEnemyTypeCount()
	{
        return 0;
	}

    private void OnEnemyDeath(GameObject obj)
    {
        if(_enemyList.Contains(obj)) _enemyList.Remove(obj);
    }

    protected override void WhenPlayerDies()
    {
        this.enabled = false;
    }

    protected override void TriggerDeath()
    {
        Log("I have been defeated");

        EventData.OnEnemyDeath -= OnEnemyDeath;
        EventData.RaiseOnEnemyDeath(gameObject);
        StopAllCoroutines();

        // Destroy all enemies and all shields
        foreach(GameObject shield in _shieldList)
		{
            Destroy(shield);
		}
        foreach(GameObject enemy in _enemyList)
		{
            Destroy(enemy);
		}
        Destroy(gameObject);
    }

    private void Log(object o) => Debug.Log("[DJ Boss] " + o);
}