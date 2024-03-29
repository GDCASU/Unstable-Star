using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement;



public class Wave : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyList;

    private float timerTime = 5f;

    [SerializeField] private bool debug = false;


    

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
        int i = 0;
        foreach (GameObject g in enemyList)
        {
            average += g.transform.position.y;
            i++;
        }
        average = average / i;

        //foreach (GameObject g in enemyList)
        //{
        //    Enemy e = g.GetComponent<Enemy>();
        //     e.setYOffSet(g.transform.localPosition.yaverage);
        //}

        float top = float.MinValue, bottom = float.MaxValue;

        foreach (var g in enemyList)
        {
            if (g.transform.position.y < bottom) bottom = g.transform.position.y;
            if (g.transform.position.y > top) top = g.transform.position.y;
        }

        var yTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -Camera.main.transform.position.z)).y;
        var yBot = Camera.main.ViewportToWorldPoint(new Vector3(0, .5f, -Camera.main.transform.position.z)).y;

        Debug.Log("BOTTOM: " + bottom);
        Debug.Log("TOP: " + top);
        Debug.Log("Y_BOT: " + yBot);
        Debug.Log("T_TOP: " + yTop);

        // needs clamping
        if (Camera.main.ViewportToWorldPoint(new Vector3(0, .9f, -Camera.main.transform.position.z)).y < (top - bottom))
        {
            Debug.Log("AAA");
            // actual - bottom / top - bottom
            foreach (var g in enemyList)
            {
                var actual = g.transform.position.y;


                var percentage = (actual - bottom) / (top - bottom);




                var toPos = Mathf.Lerp(-.2f, .2f, percentage);


     
                g.GetComponent<Enemy>().setArrivalPercentUpScreen(1f);
                Debug.Log(Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -Camera.main.transform.position.z)));
            }
        }
        else {
           // Debug.Log("BBB");
            foreach (var g in enemyList)
            {
                var actual = g.transform.position.y;
                //  var percentage = ( actual-bottom) / (Camera.main.ViewportToWorldPoint(new Vector3(0, .75f, -Camera.main.transform.position.z)).y - Camera.main.ViewportToWorldPoint(new Vector3(0, .6f, -Camera.main.transform.position.z)).y);
                var percentage = (actual - average) / Camera.main.ViewportToWorldPoint(new Vector3(0,1f, -Camera.main.transform.position.z)).y +.5f;
                  var toPos = Mathf.Lerp(-.15f, .15f, percentage) 
                    ;

          
                g.GetComponent<Enemy>().setArrivalPercentUpScreen(.75f + toPos);
            }
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


    //private void OnDrawGizmos()
    //{

    //        Vector3 position = transform.position;
    //        float size = 6.0f; // Set the size of your rectangle

    //        // Define the corners of the rectangle
    //        Vector3 corner1 = position + new Vector3(0, 50-size, -size);
    //        Vector3 corner2 = position + new Vector3(0, 50 + size, -size);
    //        Vector3 corner3 = position + new Vector3(0, 50 + size, size);
    //        Vector3 corner4 = position + new Vector3(-0, 50 - size, size);

    //        // Draw lines between the corners to form a rectangle
    //        Gizmos.DrawLine(corner1, corner2);
    //        Gizmos.DrawLine(corner2, corner3);
    //        Gizmos.DrawLine(corner3, corner4);
    //        Gizmos.DrawLine(corner4, corner1);
    //    }

    }

