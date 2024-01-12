using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - new Vector3(0f, 0.5f * Time.deltaTime, 0f);
        Debug.Log(transform.position.y);
        if (transform.position.y < 0)
        {
            transform.position = transform.position + new Vector3(0f, 27.19f, 0f);
        }
    }
}
