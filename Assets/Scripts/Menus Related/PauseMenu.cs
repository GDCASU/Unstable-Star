using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public static bool pausedGame;
    public GameObject pauseObject;
    public UnityEngine.UI.Button[] pauseList;
    public Scene_Switch sChange;

    int arrayNumber;

    private void Start()
    {
        arrayNumber = pauseList.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // toggles pause menu
        {
            pausedGame = !pausedGame;
            ChangeMenu();
        }

        if (pausedGame)
        {
            // scrolls through buttons
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.DownArrow)) { arrayNumber++; }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { arrayNumber--; }

            // selects and highlights buttons
            pauseList[arrayNumber% pauseList.Length].OnSelect(null);
            pauseList[arrayNumber% pauseList.Length].Select();
        }
    }

    void ChangeMenu()
    {
        if (pausedGame) // pauses
        {
            pauseObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else // unpauses
        {
            pauseObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    public void SetPause()
    {
        pausedGame = false;
        ChangeMenu();
    }

    public void LoadMenu()
    {
        pausedGame = false;
        ChangeMenu();
        Scene_Switch.instance.scene_changer("Menu");
    }

}
