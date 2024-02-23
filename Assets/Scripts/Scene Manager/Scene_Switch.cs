using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

//Receives int input and via switch statement plays the transition animation
//as well as loading new scene (can also change to String if needed)

public class Scene_Switch : MonoBehaviour
{
    public static Scene_Switch instance = null;
    //FadeInOut fade;
    public Animator transition;
     private void Start()
    {
        //Checks if a scene object is currently in use and destroys it if true
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            // DontDestroyOnLoad(this);
        }
    }
    
    // Gets the name of the scene and loads it 
    public void scene_changer(string scene_name)
    {
        Debug.Log("Before switch Statement");
        switch (scene_name) {
            case "Menu":
                //Play transition animation
                Debug.Log("c1");
                SceneManager.LoadScene(scene_name);
                break;
            case "Level 1":
                //Play transition animation
                Debug.Log("c2");
                StartCoroutine(WaitForlvl1(scene_name));
                break;
            case "Level_2":
                //Play transition animation
                Debug.Log("c3");
                SceneManager.LoadScene(scene_name);
                break;
            case "Level_3":
                //Play transition animation
                Debug.Log("c4");
                SceneManager.LoadScene(scene_name);
                break;
            case "Game_Over":
                //Play transition animation
                Debug.Log("c5");
                SceneManager.LoadScene(scene_name);
                break;
            case "Credits":
                Debug.Log("c6");
                //Play transition animation
                SceneManager.LoadScene(scene_name);
                break;

        }

    }
    IEnumerator WaitForlvl1(string scene_name)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        Debug.Log("Waited!");
        SceneManager.LoadScene(scene_name);
    }
}
