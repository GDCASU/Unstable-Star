using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    private const string WAVE_PARENT_NAME = "Waves";

    public static WaveManager instance;

    public Wave currentWave;

    [Header("Wave Pools")]
    [SerializeField] private List<WavePool> wavePools = new List<WavePool>();

    [Header("Events")]
    [SerializeField] public UnityEvent<int> onWaveStart;

    private GameObject waveParent;
    private WavePool currentWavePool = null;
    private int waveCounter = 1;

    private void Awake()        // Handle Singleton
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        EventData.OnWaveComplete += SpawnWave;

        // CHECK IF WAVE SELECTED IS NULL TO PREVENT NULL REF EXC
        waveParent = GameObject.Find(WAVE_PARENT_NAME);

        currentWavePool = wavePools[0];     // Start currentWavePool at first wave in list
        SpawnWave();                        // Spawn the first wave
    }

    public void SpawnWave()
    {
        // Event for UI
        onWaveStart?.Invoke(waveCounter);
        // Spawn wave using RandomWaveSelect()
        GameObject wave = Instantiate(currentWavePool.RandomWaveSelect(), waveParent.transform);
        currentWave = wave.GetComponent<Wave>();

        if (!wave)
        {
            // handle errors with no waves existing in a wavepool
            Debug.Log("Error: Could not spawn wave");
        }
    }

    public void UpdateWaveCounter()         // Update waveCounter and check if we need to raise the difficulty
    {
        waveCounter++;
    }
}
