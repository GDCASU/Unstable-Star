using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    // Levels Unlocked
    public bool isAct2Unlocked;
    public bool isAct3Unlocked;
    
    // Levels Complete
    public bool isAct1Complete;
    public bool isAct2Complete;
    public bool isAct3Complete;

    // Weapons
    public bool isGatlingUnlocked;
    public bool isLaserUnlocked;

    // Abilities
    public bool isProxiBombUnlocked;
    public bool isPhaseShiftUnlocked;

    // Misc
    public bool areCheatsUnlocked;

    /// <summary>
    /// Constructor that will be called when creating a new game
    /// </summary>
    public GameData()
    {
        // Bools are false by default, so no need to set them here
    }
}

[System.Serializable]
public class ConfigData
{
    // Sound
    public int masterVolumeValue;
    public int sfxVolumeValue;
    public int musicVolumeValue;

    // Cursor
    public bool hideCursor;
    public bool lockCursor;
    public bool confineCursor;

    // Framerate
    public bool capFrameRate;
    public int targetFrameRate;

    // Gameplay
    public int sensitivity;
    public bool invertYAxis;

    // Graphics
    public int brightness;
    public int quality;
    public bool fullscreen;

    /// <summary>
    /// Constructor that will be called when creating a new game
    /// </summary>
    public ConfigData()
    {
        masterVolumeValue = 50;
        sfxVolumeValue = 50;
        musicVolumeValue = 50;
        targetFrameRate = 60;
        brightness = 50;
        quality = 10;
        sensitivity = 3;
        fullscreen = true;
        // Should framerate be capped??
        //capFrameRate = true;
    }
}
