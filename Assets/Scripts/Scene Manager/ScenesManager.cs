using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    MainMenu,
    Weapon_Select_1,
    CutScene_1,
    Level_1,
    CutScene_2,
    Weapon_Select_2,
    CutScene_3,
    Level_2,
    CutScene_4,
    Weapon_Select_3,
    CutScene_5,
    Level_3,
    CutScene_6,
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
    public static int currentLevel = 1;
    private Dictionary<Scenes, bool> unlockedScenes;

    private void Awake()
    {
        // Singleton: Checks if a scene object is currently in use and destroys it if true
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        DontDestroyOnLoad(this);
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
            { Scenes.MainMenu, true },
            { Scenes.CutScene_1, true },
            { Scenes.CutScene_2, true },
            { Scenes.CutScene_3, true },
            { Scenes.CutScene_4, false },
            { Scenes.CutScene_5, false },
            { Scenes.CutScene_6, false },
            { Scenes.Weapon_Select_1, true },
            { Scenes.Weapon_Select_2, false },
            { Scenes.Weapon_Select_3, false },
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
        currentScene++;
        SceneManager.LoadScene(currentScene);
    }

    public void UnlockScene(Scenes scene)
    {
        if (!unlockedScenes.ContainsKey(scene))
        {
            Debug.LogError("Error: Scene does not exist");
            return;
        }

        unlockedScenes[scene] = true;
    }

    public void LoadLevel(int level)
    {
        switch (level)
        {
            case 1:
                LoadScene(Scenes.Weapon_Select_1);
                break;
            case 2:
                LoadScene(Scenes.Weapon_Select_2);
                break;
            case 3:
                LoadScene(Scenes.Weapon_Select_3);
                break;
            default:
                Debug.LogError("Error: Level does not exist");
                break;
        }
    }

    public bool CheckLevel(int level)
    {
        switch (level)
        {
            case 1:
                return CheckScene(Scenes.CutScene_1);
            case 2:
                return CheckScene(Scenes.CutScene_3);
            case 3:
                return CheckScene(Scenes.CutScene_4);
            default:
                return false;
        }
    }
}
