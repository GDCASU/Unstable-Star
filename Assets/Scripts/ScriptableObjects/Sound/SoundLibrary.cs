using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum used to get specific sounds to play
public enum SoundTag
{
    tempRayGunFire,
    tempBattleMusic
}

/// <summary> 
/// <para> Scriptable Object that contains the references to all sounds in game </para> 
/// <para> WARNING: YOU CANT PLAY SOUNDS FROM THIS OBJECT, IT ONLY HOLDS EVENT REFERENCES </para>
/// Create a FMOD.Studio.EventInstance from the references
/// </summary>
[CreateAssetMenu(fileName = "ScriptableSoundLibrary", menuName = "ScriptableObjects/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    // Dictionary with keys
    private Dictionary<SoundTag, FMODUnity.EventReference> indexSound = new();

    // A null key for use in dictionary indexing
    private readonly FMODUnity.EventReference nullReference = new();

    // All possible sounds, assign on the inspector
    [Header("Sound Effects")]
    public FMODUnity.EventReference tempRayGunShot;

    [Header("Music Tracks")]
    public FMODUnity.EventReference tempBattleMusic;

    // Must be called by the SoundManager
    public void InitializeData()
    {
        // Populate dictionary with keys, must reference all possible Sounds
        indexSound.Add(SoundTag.tempRayGunFire, tempRayGunShot);
        indexSound.Add(SoundTag.tempBattleMusic, tempBattleMusic);
    }

    // Function to index a sound, returns false if it fails to find it
    public bool TryGetSound(SoundTag targetSound, out FMODUnity.EventReference soundEvent)
    {
        // Set soundEvent to null in case its not found in dictionary
        soundEvent = nullReference;

        // Check if sound is within the dictionary
        if (indexSound.TryGetValue(targetSound, out FMODUnity.EventReference soundObtained))
        {
            // We did find it, return it
            soundEvent = soundObtained;
            return true;
            
        }
        // We did not find it
        return false;
    }
}
