using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(rotationSpeed, 0, 0) * Time.deltaTime);
    }
}
