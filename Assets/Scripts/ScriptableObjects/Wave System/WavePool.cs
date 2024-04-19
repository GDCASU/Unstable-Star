using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavePool", menuName = "Wave Pool")]
public class WavePool : ScriptableObject
{
    private const string WAVE_PARENT_NAME = "Waves";

    [Header("Wave Settings")]
    [SerializeField] private List<GameObject> waves = new List<GameObject>();
    [SerializeField] float enemyBoundsCenter = .75f;
    [SerializeField] float enemyBoundsSize = .15f;
    [SerializeField] bool overrideWaveBounds = true;

    public GameObject RandomWaveSelect()      // Select a Random Wave if it doesn't exist then use recursion to select again (highly unlikely)
    {
        if (waves.Count <= 0)       // If no waves currently selected then return null
            return null;

        if (overrideWaveBounds)
        {
            foreach(GameObject wave in waves)
            {
                wave.GetComponent<Wave>().SetBoundFields(enemyBoundsCenter, enemyBoundsSize);
            }
        }

        int roll = Random.Range(0, waves.Count);
        if (waves[roll])
            return waves[roll];
        else
            return RandomWaveSelect();
    }
}