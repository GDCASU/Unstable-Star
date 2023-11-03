using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXController : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private ParticleSystem hitParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if get hit
        // hitParticle.Play();
        //TESTING STUFF
        if(Input.GetKeyDown(KeyCode.K))
        {
        deathParticle.Play();
        }
        // if Health == 0;
        if(Input.GetKeyDown(KeyCode.H)) 
        {
            hitParticle.Play();
            Debug.Log("HIT");

        }
        
    }

}
