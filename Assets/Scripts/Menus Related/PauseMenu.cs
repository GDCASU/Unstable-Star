using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public static bool pausedGame;
    public GameObject pauseObject;
    public UnityEngine.UI.Button[] pauseList;

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
            PlayerInput.instance.ToggleControls(false); // Disable player input
        }
        else // unpauses
        {
            pauseObject.SetActive(false);
            Time.timeScale = 1.0f;
            PlayerInput.instance.ToggleControls(true); // Disable player input
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
        ScenesManager.instance.LoadScene(Scenes.MainMenu);
    }

    public void LoadMainMenu()
    {
        ScenesManager.instance.LoadScene(Scenes.MainMenu);
    }
}
