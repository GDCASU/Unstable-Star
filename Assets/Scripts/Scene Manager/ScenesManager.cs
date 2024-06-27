using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    IntroLoadingScreen, // 0
    MainMenu,           // 1
    Loadout_Select,     // 2
    CutScene_1,         // 3
    Level_1,            // 4
    CutScene_2,         // LOADOUT AFTER THIS
    CutScene_3,
    Level_2,
    CutScene_4,         // LOADOUT AFTER THIS
    CutScene_5,
    CutScene_6,
    Level_3,
    CutScene_7,
    GameOver,
    Credits
}

public class ScenesManager : MonoBehaviour
{
    // Singleton instance
    public static ScenesManager instance = null;

    [SerializeField] private bool debug = false;

    public static int currentScene { get; private set; }
    private Dictionary<Scenes, bool> unlockedScenes;

    // Ian: Stores the target scene after the weapon select is finished
    public Scenes? nextSceneAfterWeaponSelect;

    private void Awake()
    {
        // Handle Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;

        InitializeDict();
    }

    private void InitializeDict()
    {
        unlockedScenes = new Dictionary<Scenes, bool>
        {
            { Scenes.IntroLoadingScreen, true},
            { Scenes.MainMenu, true },
            { Scenes.CutScene_1, true },
            { Scenes.CutScene_2, true },
            { Scenes.CutScene_3, true },
            { Scenes.CutScene_4, false },
            { Scenes.CutScene_5, false },
            { Scenes.CutScene_6, false },
            { Scenes.Loadout_Select, true },
            { Scenes.Level_1, true },
            { Scenes.Level_2, true },
            { Scenes.Level_3, false },
            { Scenes.GameOver, true },
            { Scenes.Credits, true }
        };
    }

    /// <summary>
    /// Check if the scene is unlicked if not returns false; otherwise true
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public bool CheckScene(Scenes scene)
    {
        if (!unlockedScenes.ContainsKey(scene))
        {
            Debug.LogError("Scene does not exist");
            return false;
        }

        if (unlockedScenes[scene])
        {
            if (debug) Debug.Log("Player has access to scene.");
            return true;
        }
        else
        {
            if (debug) Debug.Log("Player does not have access to this scene yet.");
            return false;
        }
    }

    /// <summary>
    /// Pass in the name of the scene and loads it 
    /// </summary>
    /// <param name="scene"></param>
    public void LoadScene(Scenes scene)
    {
        currentScene = (int)scene;
        if (!unlockedScenes.ContainsKey(scene))
        {
            Debug.LogError("Scene does not exist");
            return;
        }

        SceneManager.LoadScene((int)scene);
    }

    public void LoadNextScene()
    {
        // Ensure there's not a priority next scene
        if(nextSceneAfterWeaponSelect != null)
        {
            Scenes temp = nextSceneAfterWeaponSelect.Value;
            nextSceneAfterWeaponSelect = null;
            LoadScene(temp);
            return;
        }

        // Check for special scenes
        if((Scenes)currentScene == Scenes.CutScene_2 || (Scenes)currentScene == Scenes.CutScene_4)
        {
            nextSceneAfterWeaponSelect = (Scenes)(++currentScene);
            LoadScene(Scenes.Loadout_Select);
            return;
        }

        // Typical behavior
        currentScene++;
        SceneManager.LoadScene(currentScene);
    }

}
