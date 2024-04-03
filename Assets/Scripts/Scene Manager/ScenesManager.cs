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

    private static int currentScene = 0;
    private Dictionary<string, Scenes> scenesDict;

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
        scenesDict = new Dictionary<string, Scenes>();

        scenesDict.Add("MainMenu", Scenes.MainMenu);
        scenesDict.Add("Cutscene_1", Scenes.CutScene_1);
        scenesDict.Add("Cutscene_2", Scenes.CutScene_2);
        scenesDict.Add("Cutscene_3", Scenes.CutScene_3);
        scenesDict.Add("Cutscene_4", Scenes.CutScene_4);
        scenesDict.Add("Cutscene_5", Scenes.CutScene_5);
        scenesDict.Add("Cutscene_6", Scenes.CutScene_6);
        scenesDict.Add("Level_1", Scenes.Level_1);
        scenesDict.Add("Level_2", Scenes.Level_2);
        scenesDict.Add("Level_3", Scenes.Level_3);
        scenesDict.Add("GameOver", Scenes.GameOver);
    }

    // Gets the name of the scene and loads it 
    public void LoadScene(Scenes scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene);
    }

    public void LoadScene(string scene)
    {
        if (scenesDict.TryGetValue(scene, out Scenes value))
        {
            currentScene = (int)value;
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)value);
        }
        else
            Debug.LogError("Scene does not exist");
    }

    public void LoadNextScene()
    {
        currentScene++;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }

    IEnumerator WaitForlvl1(string scene_name)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        Debug.Log("Waited!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene_name);
    }
}
