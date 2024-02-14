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

    // FMOD VCA Buses
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

    // The string must contain the name assigned to the bus on the FMOD Mixer
    // this is useful if someone decides to change how they are named without re-scripting anything
    [Header("VCA Bus Names")]
    public string masterBus;
    public string musicBus;
    public string sfxBus;

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
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // Fecth the matching VCAs
        masterVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + masterBus);
        musicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + musicBus);
        sfxVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + sfxBus);

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

        // Initialize the data on the Sound library attached
        soundLibrary.InitializeData();
    }

    // Debugging 
    private void Update()
    {
        
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

    /// <summary> Play Sound no matter the state </summary>
    public void PlaySound(SoundTag targetSound)
    {
        // FIXME: Should still be tracked for muting?
        if ( soundLibrary.TryGetSound(targetSound, out FMODUnity.EventReference sound) )
        {
            FMOD.Studio.EventInstance playedSound = FMODUnity.RuntimeManager.CreateInstance(sound);
            playedSound.start();
        }
    }

    /// <summary> Play only if not already playing </summary>
    public void PlaySoundIfNotPlaying(SoundTag targetSound)
    {

    }

    /// <summary> Interrupt if specified sound is playing, then play again </summary>
    public void InterruptSoundAndReplay(SoundTag targetSound)
    {

    }

    /// <summary> Play on loop. Wont play if specified sound is already playing</summary>
    public void PlayOnLoop(SoundTag targetSound)
    {

    }

    /// <summary> 
    /// <para> Stop specific sound if playing </para> 
    /// <para> Will stop all sounds associated with the target </para> 
    /// </summary>
    public void StopAllSoundsMatchingTarget(SoundTag targetSound)
    {

    }

    /// <summary> Stop all sounds playing </summary>
    public void StopAllSounds()
    {

    }

}
