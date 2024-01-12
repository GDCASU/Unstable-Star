using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] float aroundXSpeed = 0f;
    [SerializeField] float aroundYSpeed = 0f;
    [SerializeField] float aroundZSpeed = 0f;
    [SerializeField] float delay = 0f;

    // Update is called once per frame
    void Update()
    {
        if (delay > 0f)
        {
            delay -= Time.deltaTime;
            return;
        }

        transform.Rotate(aroundXSpeed * Time.deltaTime, aroundYSpeed * Time.deltaTime, aroundZSpeed * Time.deltaTime);
    }
}
