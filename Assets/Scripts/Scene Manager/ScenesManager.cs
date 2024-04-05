using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    MainMenu,
    CutScene_1,
    Level_1,
    CutScene_2,
    CutScene_3,
    Level_2,
    CutScene_4,
    CutScene_5,
    Level_3,
    CutScene_6,
    GameOver
}

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance = null;     // Singleton instance

    [SerializeField] private bool debug = false;

    public static int currentScene { get; private set; }
    public static int currentLevel = 1;
    private Dictionary<Scenes, bool> unlockedScenes;

    public Animator transition;

    private void Awake()
    {
        // Singleton: Checks if a scene object is currently in use and destroys it if true
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;

        InitializeDict();
    }

    private void InitializeDict()
    {
        unlockedScenes = new Dictionary<Scenes, bool>();

        unlockedScenes.Add(Scenes.MainMenu, true);
        unlockedScenes.Add(Scenes.CutScene_1, true);
        unlockedScenes.Add(Scenes.CutScene_2, true);
        unlockedScenes.Add(Scenes.CutScene_3, true);
        unlockedScenes.Add(Scenes.CutScene_4, false);
        unlockedScenes.Add(Scenes.CutScene_5, false);
        unlockedScenes.Add(Scenes.CutScene_6, false);
        unlockedScenes.Add(Scenes.Level_1, true);
        unlockedScenes.Add(Scenes.Level_2, true);
        unlockedScenes.Add(Scenes.Level_3, false);
        unlockedScenes.Add(Scenes.GameOver, false);
    }

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

    // Gets the name of the scene and loads it 
    public void LoadScene(Scenes scene)
    {
        if (!unlockedScenes.ContainsKey(scene))
        {
            Debug.LogError("Scene does not exist");
            return;
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene);
    }

    public void LoadNextScene()
    {
        currentScene++;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
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
                ScenesManager.instance.LoadScene(Scenes.CutScene_1);
                break;
            case 2:
                ScenesManager.instance.LoadScene(Scenes.CutScene_3);
                break;
            case 3:
                ScenesManager.instance.LoadScene(Scenes.CutScene_4);
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

    IEnumerator WaitForlvl1(string scene_name)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        Debug.Log("Waited!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene_name);
    }
}