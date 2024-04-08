using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;        // Singleton reference

    [Header("Gameplay")]
    [SerializeField] private bool hideCursor;

    [Header("Frame Rate")]
    [SerializeField] private bool capFrameRate;
    [SerializeField] private int targetFrameRate = 60;

    private void Awake()            // Handle Singleton
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (capFrameRate)
            SetFrameRate(targetFrameRate);

        if (hideCursor)
            HideCursor();
        else
            ShowCursorand();

    }

    /// <summary>
    /// Sets the target frame rate that the game will run at.
    /// </summary>
    /// <param name="frameRate"> Target frame rate </param>
    private void SetFrameRate(int frameRate)
    {
        Application.targetFrameRate = frameRate;
    }

    private void HideCursor()
    {
        Cursor.visible = false;
    }

    private void ShowCursorand()
    {
        Cursor.visible = true;
    }
}
