using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryController : MonoBehaviour
{
    public AnimationClip animClip;
    Vector3 initposition = new Vector3(-35, -27, 0);
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = initposition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
