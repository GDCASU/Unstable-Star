using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    //Local Variables
    string creator;
    float speed;
    float rotation;

    //Headers for inspector
    [Header("Debugging")]
    [SerializeField] private bool detectForeigner = true;

    private void Awake()
    {
        
        if (this.transform.parent.parent.eulerAngles.x <= 180f)
        {
            rotation = this.transform.parent.parent.eulerAngles.x;
        }
        else
        {
            rotation = this.transform.parent.parent.eulerAngles.x - 360f;
        }


    }


    private void FixedUpdate()
    {
        
    }

    //CREATOR MUST MATCH ITS OWN TAG IN INSPECTOR
    public void SetCreator(string creator)
    {
        this.creator = creator;
    }

}
