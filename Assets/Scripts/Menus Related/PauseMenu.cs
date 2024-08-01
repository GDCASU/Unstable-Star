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
    [Header("References")]
    [SerializeField] GameObject graphicsContainer;  // Contains the graphics that show up when the game is paused.
    [SerializeField] UnityEngine.UI.Button[] menuButtons;

    [Header("Values")]
    [SerializeField] bool openOnStart = false;
    int curButtonIndex = 0;

    [Header("Debugging")]
    [SerializeField] bool printDebugs = false;

    [HideInInspector] public static bool pausedGame = false;  // Signal if the game is paused or not.

    #region Unity Events

    private void Awake()
    {
        if (printDebugs) Debug.Log("PauseMenu::Awake");

        // Bind relevant events
        PlayerInput.OnPauseGame += OpenPauseMenu;
        PlayerInput.OnCancel += ClosePauseMenu;

        PlayerInput.OnMenuNavigate += NavigateMenu;

        if (openOnStart) OpenPauseMenu();
        else ClosePauseMenu();
    }

    private void OnDestroy()
    {
        // Clean up attached events
        PlayerInput.OnPauseGame -= OpenPauseMenu;
        PlayerInput.OnCancel -= ClosePauseMenu;

        PlayerInput.OnMenuNavigate -= NavigateMenu;
    }

    #endregion

    #region Menu Navigation

    public void OpenPauseMenu()
    {
        if (printDebugs) Debug.Log("PauseMenu::OpenPauseMenu");

        pausedGame = true;
        graphicsContainer.SetActive(true);
        Time.timeScale = 0f;
        PlayerInput.instance.ActivateUiControls();

        // Ensure menu is properly loaded
        menuButtons[curButtonIndex].Select();
    }

    public void ClosePauseMenu()
    {
        if (printDebugs) Debug.Log("PauseMenu::ClosePauseMenu");

        pausedGame = false;
        graphicsContainer.SetActive(false);
        Time.timeScale = 1.0f;
        PlayerInput.instance.ActivateShipControls();
    }

    /// <summary>
    /// Moves the menu selection up or down.
    /// </summary>
    /// <param name="dir">Which way to move the menu selection.</param>
    void NavigateMenu(Vector2 dir)
    {
        if (printDebugs) Debug.Log("PauseMenu::NavigateMenu");

        // Up
        if (dir.x + dir.y > 0) curButtonIndex = (curButtonIndex + 1) % menuButtons.Length;
        else if(dir.x + dir.y < 0)
        {
            curButtonIndex--;
            if(curButtonIndex < 0) curButtonIndex = menuButtons.Length - 1;
        }

        menuButtons[curButtonIndex].Select();
    }

    #endregion

    #region Button Handlers

    public void LoadMainMenu()
    {
        ClosePauseMenu();
        ScenesManager.instance.LoadScene(Scenes.MainMenu);
    }

    #endregion
}
