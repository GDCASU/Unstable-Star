using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    //This should make it so that the enemy starts at x, y, then moves until x2, y2 and then back to x1, y1, then the motion loops forever (it is implied that y1 > y2)
    public void GatilingEnemyMovement(int start_x, int start_y, int x1, int y1, int x2, int y2, GameObject Enemy, float velocity){
        Enemy.transform.position = new Vector3(start_x, start_y, 0);
        Vector3 velocityVector = new Vector3(0, velocity, 0);
        StartCoroutine(MovementGatling(y1, y2, Enemy, velocityVector));
    }

    private IEnumerator MovementGatling(int y1, int y2, GameObject Enemy, Vector3 velocity){
        if(Enemy.transform.position.y <= y2 || Enemy.transform.position.y >= y1){
            velocity = -velocity;
        }
        Enemy.transform.Translate(velocity * Time.deltaTime);
        yield return null;
    }
}
