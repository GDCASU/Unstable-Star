using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    public FMODUnity.EventReference PlayerStateEvent;
    public bool PlayOnAwake;

    public void PlayOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(PlayerStateEvent, this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (PlayOnAwake)
        {
            PlayOneShot();
        }
    }

}
