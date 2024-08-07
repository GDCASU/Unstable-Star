using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySound(SoundLibrary.CreditsTrack);

        PlayerInput.instance.ActivateUiControls();
    }

    private void OnDestroy()
    {
        PlayerInput.instance.ActivateShipControls();
    }

    public void StopMusic() 
    {
        SoundManager.instance.PauseAllSounds();
        ScenesManager.instance.LoadScene(Scenes.MainMenu);
    }

}
