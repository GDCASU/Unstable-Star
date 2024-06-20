using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameSettings : MonoBehaviour, IDataPersistance
{
    public static GameSettings instance;        // Singleton reference

    [Header("Cursor Settings")]
    [SerializeField] private bool hideCursor;
    [SerializeField] private bool lockCursor;
    [SerializeField] private bool confineCursor;

    [Header("Frame Rate")]
    [SerializeField] private bool capFrameRate;
    [SerializeField] private int targetFrameRate = 60;

    [Header("Cheats")]
    public bool areCheatsUnlocked;
    public bool isPistolLethal;
    public bool isHealth100;

    private void Awake()            
    {
        // Handle Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Subscribe to saving events
        SerializedDataManager.StartSavingEvent += SaveData;
    }

    private void Start()
    {
        // Load Data
        LoadData();
    }

    public void SaveData()
    {
        // Save data to file
        SerializedDataManager.instance.configData.hideCursor = hideCursor;
        SerializedDataManager.instance.configData.lockCursor = lockCursor;
        SerializedDataManager.instance.configData.confineCursor = confineCursor;
        SerializedDataManager.instance.configData.capFrameRate = capFrameRate;
        SerializedDataManager.instance.configData.targetFrameRate = targetFrameRate;
        SerializedDataManager.instance.gameData.areCheatsUnlocked = areCheatsUnlocked;

        // Unsubscribe from events
        SerializedDataManager.StartSavingEvent -= SaveData;
    }

    public void LoadData()
    {
        // Load data from configs
        hideCursor = SerializedDataManager.instance.configData.hideCursor;
        lockCursor = SerializedDataManager.instance.configData.lockCursor;
        confineCursor = SerializedDataManager.instance.configData.confineCursor;
        capFrameRate = SerializedDataManager.instance.configData.capFrameRate;
        targetFrameRate = SerializedDataManager.instance.configData.targetFrameRate;
        areCheatsUnlocked = SerializedDataManager.instance.gameData.areCheatsUnlocked;

        // Set variables
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
