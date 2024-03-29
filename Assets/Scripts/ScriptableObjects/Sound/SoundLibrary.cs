using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

/// <summary> 
/// <para> Scriptable Object that contains the references to all sounds in game </para> 
/// <para> WARNING: YOU CANT PLAY SOUNDS FROM THIS OBJECT, IT ONLY HOLDS EVENT REFERENCES </para>
/// Create a FMOD.Studio.EventInstance from the references
/// </summary>
[CreateAssetMenu(fileName = "ScriptableSoundLibrary", menuName = "ScriptableObjects/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    // TODO: Maybe create a custom editor script for all this

    // VCAs:
    // The string must contain the name assigned to the bus on the FMOD Mixer
    // this is useful if someone decides to change how they are named without re-scripting everything
    [Header("VCA Bus Names")]
    public string masterBus;
    public string musicBus;
    public string sfxBus;

    // Bus Groups allows us to do apply changes to a group of sound effects if playing
    [Header("Group Bus Names")]
    [SerializeField] private string onLevelCombatPath;

    // Bus variables
    public FMOD.Studio.Bus onLevelCombatBus { get; private set; }


    // FMOD Sound References: assign these on the inspector
    [Header("TESTING EFFECTS")]
    [SerializeField] private FMODUnity.EventReference _tempRayGunShot;
    public FMODUnity.EventReference TempRayGunShot { get; private set; }
    [SerializeField] private FMODUnity.EventReference _tempBattleMusic;
    public static FMODUnity.EventReference TempBattleMusic { get; private set; }


    [Header("Music Tracks")]
    [SerializeField] private FMODUnity.EventReference _mainMenuTrack;
    public static FMODUnity.EventReference MainMenuTrack { get; private set; }


    [Header("Player")]
    [SerializeField] private FMODUnity.EventReference _playerShipDestroyed;
    public static FMODUnity.EventReference PlayerShipDestroyed { get; private set; }


    [Header("Player Abilities")]
    [SerializeField] private FMODUnity.EventReference _phaseShiftEnter;
    public static FMODUnity.EventReference PhaseShiftEnter { get; private set; }
    [SerializeField] private FMODUnity.EventReference _phaseShiftExit;
    public static FMODUnity.EventReference PhaseShiftExit { get; private set; }
    [SerializeField] private FMODUnity.EventReference _phaseShiftStay;
    public static FMODUnity.EventReference PhaseShiftStay { get; private set; }
    [SerializeField] private FMODUnity.EventReference _proxibombExplosion;
    public static FMODUnity.EventReference ProxibombExplosion { get; private set; }


    [Header("Hazards")]
    [SerializeField] private FMODUnity.EventReference _asteroidDestroyed;
    public static FMODUnity.EventReference AsteroidDestroyed { get; private set; }

    // Function to initialize data, must be called by the sound manager
    public void InitializeLibrary()
    {
        // Fetch Group Buses
        onLevelCombatBus = FMODUnity.RuntimeManager.GetBus("bus:/" + onLevelCombatPath);

        // TESTING EFFECTS
        TempRayGunShot = _tempRayGunShot;
        TempBattleMusic = _tempBattleMusic;

        // Music Tracks
        MainMenuTrack = _mainMenuTrack;

        // Player
        PlayerShipDestroyed = _playerShipDestroyed;

        // Player Abilities
        PhaseShiftEnter = _phaseShiftEnter;
        PhaseShiftExit = _phaseShiftExit;
        PhaseShiftStay = _phaseShiftStay;
        ProxibombExplosion = _proxibombExplosion;

        // Hazards
        AsteroidDestroyed = _asteroidDestroyed;

        // FIXME: Most of these are stored on their scriptable objects, is it necessary to place them here?

    }

}


