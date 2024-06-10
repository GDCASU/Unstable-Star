using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will wait for the Game Data to be loaded
/// </summary>
public class LoadingScreenWait : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitForDataToLoad());
    }

    /// <summary>
    /// Routine that will wait for data to load
    /// </summary>
    private IEnumerator WaitForDataToLoad()
    {
        // Wait for the singleton to be set
        while (SerializedDataManager.instance == null) yield return null;
        // Wait for the data to load
        while (!SerializedDataManager.instance.hasLoaded) yield return null;
        // Make sure the scene manager singleton is set
        while (ScenesManager.instance == null) yield return null;
        // Everything good, go to main menu
        ScenesManager.instance.LoadScene(Scenes.MainMenu);
    }
}
