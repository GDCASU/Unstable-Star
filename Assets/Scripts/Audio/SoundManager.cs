using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

// VCA Enums, necessary if we want outsider scripts modifying the VCA volumes
public enum SoundControllers
{
    Master,
    Music,
    SFX
}

public class SoundManager : MonoBehaviour
{
    // Singleton
    public static SoundManager instance;

    // FMOD VCAs
    private FMOD.Studio.VCA masterVCA;
    private FMOD.Studio.VCA musicVCA;
    private FMOD.Studio.VCA sfxVCA;

    // VCA BUSES Dictionary
    private Dictionary<SoundControllers, FMOD.Studio.VCA> VCADictionary = new();

    // Sound Library ScriptableObject
    [Header("Sound Library Data")]
    [SerializeField] private SoundLibrary soundLibrary;

    // Debugging
    [Header("Debugging")]
    [SerializeField] private bool disableSliders;
    [SerializeField] private bool fadeStopCombatSounds;


    // Sound Inspector Slider
    [Header("Sound Sliders")]
    [Range(0f, 1f)] [SerializeField] private float masterSlider;
    [Range(0f, 1f)] [SerializeField] private float musicSlider;
    [Range(0f, 1f)] [SerializeField] private float sfxSlider;

    // Local variables
    private float previousMasterVolume;
    private float previousMusicVolume;
    private float previousSFXVolume;

    private void Awake()
    {
        // Handle Singleton
        if (instance != null) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Initialize SoundLibrary Singleton
        soundLibrary.InitializeLibrary();

        // Fecth the matching VCAs
        masterVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + soundLibrary.masterBus);
        musicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + soundLibrary.musicBus);
        sfxVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + soundLibrary.sfxBus);

        // Populate Dictionary
        VCADictionary.Add(SoundControllers.Master, masterVCA);
        VCADictionary.Add(SoundControllers.Music, musicVCA);
        VCADictionary.Add(SoundControllers.SFX, sfxVCA);

        // Setup the previous values for checking
        masterVCA.getVolume(out float masterVolumeVal);
        musicVCA.getVolume(out float musicVolumeVal);
        sfxVCA.getVolume(out float sfxVolumeVal);
        previousMasterVolume = masterVolumeVal;
        previousMusicVolume = musicVolumeVal;
        previousSFXVolume = sfxVolumeVal;
    }

    // Debugging
    private void Update()
    {
        if (fadeStopCombatSounds)
        {
            FadeStopCombatSounds();
            fadeStopCombatSounds = false;
        }
    }

    // Loop for inspector sliders, can be removed once UI can manage this
    private void LateUpdate()
    {
        // Get the current volumes of their VCA's
        masterVCA.getVolume(out float currMasterVolume);
        musicVCA.getVolume(out float currMusicVolume);
        sfxVCA.getVolume(out float currSFXVolume);

        // if disabled, lock the values of the sliders and update them
        if (disableSliders)
        {
            masterSlider = currMasterVolume;
            musicSlider = currMusicVolume;
            sfxSlider = currSFXVolume;
            previousMasterVolume = currMasterVolume;
            previousMusicVolume = currMusicVolume;
            previousSFXVolume = currSFXVolume;
            return;
        }

        // TWO VITAL CHECKS

        // Check if the volume value has been changed from outside the script
        // Update the sliders to the new value
        if (previousMasterVolume != currMasterVolume)
        {
            masterSlider = currMasterVolume;
        }
        if (previousMusicVolume != currMusicVolume)
        {
            musicSlider = currMusicVolume;
        }
        if (previousSFXVolume != currSFXVolume)
        {
            sfxSlider = currSFXVolume;
        }

        // Check if the volume sliders in this script have been changed in the inspector
        // If so, update the values of the volumes
        if (masterSlider != currMasterVolume)
        {
            SetVolume(SoundControllers.Master, masterSlider, 1);
        }
        if (musicSlider != currMusicVolume)
        {
            SetVolume(SoundControllers.Music, musicSlider, 1);
        }
        if (sfxSlider != currSFXVolume)
        {
            SetVolume(SoundControllers.SFX, sfxSlider, 1);
        }

        // Set the previous values to the current ones
        previousMasterVolume = currMasterVolume;
        previousMusicVolume = currMusicVolume;
        previousSFXVolume = currSFXVolume;
    }

    /// <summary>
    /// <para> Sets the volume of the controller. Where maxSliderVal is the maximum value of your slider. </para> 
    /// <para> if the range is [0,1]. Then [1 = Full volume] [0.5 = Half Volume] [0 = Silent] </para> 
    /// </summary>
    public void SetVolume(SoundControllers targetVCA, float volume, float maxSliderVal)
    {
        // Fetch the matching VCA 
        bool valueFound = VCADictionary.TryGetValue(targetVCA, out FMOD.Studio.VCA obtainedVCA);

        // Check if the key did get a value
        if (!valueFound) 
        {
            string msg = "<color=red>ERROR! Target VCA Specified does not exist within dictionary!</color>\n";
            msg += "Error thrown by calling SoundManager.instance.SetVolume";
            Debug.Log(msg);
            return;
        }

        // Normalize value if the range is bigger than [0,1]
        float scaledVolume = volume / maxSliderVal;
        
        // WARNING: Range must be within [0,1], throw a warning message in case the range is outside
        if (scaledVolume < 0 || scaledVolume > 1)
        {
            string msg1 = "<color=red>WARNING! A SCRIPT TRIED TO SET THE VOLUME TO A VALUE OUTSIDE OF RANGE [0,1]!</color>\n";
            msg1 += "Please check your input values used for calling SoundManager.instance.SetVolume\n";
            msg1 += "After normalizing, You tried to set it to value: <color=yellow>" + scaledVolume + "</color>";
            Debug.Log(msg1);
            return;
        }

        // Else, set the volume of the specified bus
        obtainedVCA.setVolume(scaledVolume);
    }

    /// <summary> 
    /// <para> Play Sound no matter the state </para> 
    /// Use SoundLibrary.get.[Sound] in the argument to play a specific sound
    /// </summary>
    public void PlaySound(FMODUnity.EventReference soundEvent) => FMODUnity.RuntimeManager.PlayOneShot(soundEvent);

    /// <summary>
    /// Pauses ALL sounds in the game, use carefully and sparingly
    /// </summary>
    public void PauseAllSounds() => FMODUnity.RuntimeManager.PauseAllEvents(true);

    /// <summary>
    /// Resume's all events that are paused
    /// </summary>
    public void ResumeAllSounds() => FMODUnity.RuntimeManager.PauseAllEvents(false);


    /// <summary>
    /// Stops all the combat sounds currently firing in the game using a fade out effect
    /// </summary>
    public void FadeStopCombatSounds() => soundLibrary.onLevelCombatBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);


    // TODO: Implement bank loading/de-loading for better memory?
    public void LoadBank() { throw new System.NotImplementedException(); }
    public void UnloadBank() { throw new System.NotImplementedException(); }


    // NOT IMPLEMENTED YET ******************************

    /// <summary> Play only if not already playing </summary>
    public void PlaySoundIfNotPlaying()
    {
        throw new System.NotImplementedException();
    }

    /// <summary> Interrupt if specified sound is playing, then play again </summary>
    public void InterruptSoundAndReplay()
    {
        throw new System.NotImplementedException();
    }

    /// <summary> Play on loop. Wont play if specified sound is already playing</summary>
    public void PlayOnLoop()
    {
        throw new System.NotImplementedException();
    }

    /// <summary> 
    /// <para> Stop specific sound if playing </para> 
    /// <para> Will stop all sounds associated with the target </para> 
    /// </summary>
    public void StopAllSoundsMatchingTarget()
    {
        throw new System.NotImplementedException();
    }

    /// <summary> Stop all sounds playing </summary>
    public void StopAllSounds()
    {
        throw new System.NotImplementedException();
    }

}
