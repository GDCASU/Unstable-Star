using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public TMPro.TextMeshPro newLevel;
    public string levelToLoad;
    [SerializeField] private GameObject noSaveFile = null;

    [Header("Select Modifiers")]
    public Button[] menuList;
    int arrayNumber;

    private void Start()
    {
        arrayNumber = menuList.Length;
    }

    private void Update()
    {
        // scrolls through buttons
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.DownArrow)) { arrayNumber++; }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { arrayNumber--; }

        // selects and highlights buttons
        menuList[arrayNumber % menuList.Length].OnSelect(null);
        menuList[arrayNumber % menuList.Length].Select();
    }

    public void NewGame() // loads a new game
    {
        SceneManager.LoadScene(newLevel.text);
        Debug.Log("New Game");
    }

    public void LoadGame() // loads an old save file
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }

        else { noSaveFile.SetActive(true); } // displays error text
    }

    public void ExitGame() // exits application
    {
        Application.Quit();
    }

}
