using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement;


public class Wave : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyList;

    [SerializeField] float enemyBoundsCenter = .75f;
    [SerializeField] float enemyBoundsSize = .15f;

    private float timerTime = 5f;
    private bool debug = false;

    private void Awake()
    {
        ResetAndPopulateEnemyList();   
    }

    private void Start()
    {
        EventData.OnEnemyDeath += RemoveEnemy;
        YReposition();
    }
    private void YReposition() {

        float average = 0;
        float top = float.MinValue, bottom = float.MaxValue;

        foreach (var g in enemyList)
        {
            average += g.transform.position.y;

            if (g.transform.position.y < bottom) bottom = g.transform.position.y;
            if (g.transform.position.y > top) top = g.transform.position.y;
        }

        average /= enemyList.Count;

        var yTop = Camera.main.ViewportToWorldPoint(new Vector3(0, enemyBoundsCenter + enemyBoundsSize, -Camera.main.transform.position.z)).y;
        var yBot = Camera.main.ViewportToWorldPoint(new Vector3(0, enemyBoundsCenter - enemyBoundsSize, -Camera.main.transform.position.z)).y;

        if (debug)
        {
            Debug.Log("BOTTOM: " + bottom);
            Debug.Log("TOP: " + top);
            Debug.Log("Y_BOT: " + yBot);
            Debug.Log("T_TOP: " + yTop);
        }

        // needs mapping
        foreach(var g in enemyList)
        {
            var actual = g.transform.position.y;

            float percentage;
            if(yTop - yBot < top - bottom) percentage = (actual - bottom) / (top - bottom); // Needs mapping
            else percentage = (actual - average) / Camera.main.ViewportToWorldPoint(new Vector3(0, 1f, -Camera.main.transform.position.z)).y + .5f;

            var toPos = Mathf.Lerp(-enemyBoundsSize, enemyBoundsSize, percentage);
            g.GetComponentInChildren<Enemy>().setArrivalPercentUpScreen(enemyBoundsCenter + toPos);
        }
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

    public void SetBoundFields(float enemyBoundsCenter, float enemyBoundsSize)
    {
        this.enemyBoundsCenter = enemyBoundsCenter;
        this.enemyBoundsSize = enemyBoundsSize;
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

