using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;        // Singleton reference

    [Header("Cursor Settings")]
    [SerializeField] private bool hideCursor;
    [SerializeField] private bool lockCursor;
    [SerializeField] private bool confineCursor;

    [Header("Frame Rate")]
    [SerializeField] private bool capFrameRate;
    [SerializeField] private int targetFrameRate = 60;

    private void Awake()            
    {
        // Handle Singleton
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (capFrameRate)
            SetFrameRate(targetFrameRate);

        if (hideCursor)
            HideCursor(hideCursor);

        if (lockCursor)
            LockCursor(lockCursor);

        if (confineCursor)
            ConfineCursor(confineCursor);
    }

    /// <summary>
    /// Sets the target frame rate that the game will run at.
    /// </summary>
    /// <param name="frameRate"> Target frame rate </param>
    public void SetFrameRate(int frameRate)
    {
        Application.targetFrameRate = frameRate;
    }

    public void HideCursor(bool toggle)
    {
        Cursor.visible = !toggle;
    }

    public void LockCursor(bool toggle)
    {
        if (toggle)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    public void ConfineCursor(bool toggle)
    {
        if (toggle)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
