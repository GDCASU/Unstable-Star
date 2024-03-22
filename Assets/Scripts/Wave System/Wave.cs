using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyList;

    private float timerTime = 5f;

    [SerializeField] private bool debug = false;

    private void Start()
    {
        EventData.OnEnemyDeath += RemoveEnemy;
    }

    private bool CheckListEmpty()
    {
        if (enemyList.Count <= 0)
        {
            return true;
        }
        return false;
    }

    private void RemoveEnemy(GameObject enemy)
    {
        if (enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
            if (debug) { Debug.Log("Enemy Killed: " + enemy); }
        }

        if(CheckListEmpty())
        {
            EventData.RaiseOnWaveComplete();
            if (debug) { Debug.Log("Wave Complete: " + gameObject); }
            EventData.OnEnemyDeath -= RemoveEnemy;
        }
    }


    public void ResetAndPopulateEnemyList()
    {
        enemyList.Clear();

        foreach (Transform child in transform)
        {
            GameObject childGameObject = child.gameObject;
            if (childGameObject != null)
            {
                enemyList.Add(childGameObject);
            }
        }

        if (debug)
        {
            Debug.Log("Enemy list reset and populated with children.");
        }
    }
}
