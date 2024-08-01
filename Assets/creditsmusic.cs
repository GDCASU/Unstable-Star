using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySound(SoundLibrary.CreditsTrack);
    }

    public void StopMusic() 
    {
        SoundManager.instance.PauseAllSounds();
    }

}
