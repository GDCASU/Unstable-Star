using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCAController : MonoBehaviour
{
    // I Added all this code so its easier to implement into UI without much effort
    // Place this script within the slider component on the canvas
    [Header("Slider Settings")]
    public float volumeSliderMax;
    private Slider volumeSlider;
    

    // Bus Volume Control
    private FMOD.Studio.VCA VCAControl;

    // Debugging Inspector Slider
    [Header("Sound Sliders")]
    public bool useInspectorSlider;
    [Range(0f, 1f)]
    public float debugVolumeSlider;

    // String names of VCA, helps avoid hardcoding and script reusing
    // This has to match its respective VCA on the FMOD Project
    [Header("VCA Bus Name")]
    public string VCA_Name;

    // Fecth the matching VCA and setting up UI Slider
    private void Start()
    {
        VCAControl = FMODUnity.RuntimeManager.GetVCA("vca:/" + VCA_Name);

        // Uncomment this out once implemented into UI
        /*
        volumeSlider = GetComponent<Slider>();
        volumeSlider.maxValue = volumeSliderMax;
        volumeSlider.minValue = volumeSliderMin;
        */
    }

    // Should be removed once implemented into UI
    private void Update()
    {
        if (useInspectorSlider)
        {
            SetVolume(debugVolumeSlider, 1);
        }
    }

    // Sets the volume of the controller
    // 1 = Default (Highest volume possible)
    // 0.5 = Half of Default
    // 0 = Silent
    public void SetVolume(float volume, float maxSliderVal)
    {
        // Normalize value if the range is bigger than [0,1]
        // WARNING: Lowest value must be 0
        float scaledVolume = volume / maxSliderVal;

        VCAControl.setVolume(volume);
    }


}
