using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempCheatManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private ScriptablePistol playerPistol;
    [SerializeField] private ScriptablePlayer playerData;

    [Header("HUD Objects")]
    [SerializeField] private GameObject health100Option;
    [SerializeField] private GameObject lethalPistolOption;
    [SerializeField] private GameObject unlockAllOption;

    // Local Variables
    private TextMeshProUGUI health100Text;
    private TextMeshProUGUI lethalPistolText;
    private TextMeshProUGUI unlockAllText;
    private bool areCheatsRendered;
    private int defaultMaxHealth;
    private int defaultMaxShield;

    // Cheat activated bools
    private bool clickedUnlockAll;
    private bool isPistolLethal;
    private bool isHealth100;


    // Start is called before the first frame update
    void Start()
    {
        // Get text mesh pro components
        health100Text = health100Option.GetComponent<TextMeshProUGUI>();
        lethalPistolText = lethalPistolOption.GetComponent<TextMeshProUGUI>();
        unlockAllText = unlockAllOption.GetComponent<TextMeshProUGUI>();

        // Get default stats
        defaultMaxHealth = playerData.maxHealth;
        defaultMaxShield = playerData.maxShield;
    }

    /// <summary>
    /// Function that will make the cheats show
    /// </summary>
    public void RenderCheatOptions()
    {
        if (!areCheatsRendered)
        {
            health100Option.SetActive(true);
            lethalPistolOption.SetActive(true);
            unlockAllOption.SetActive(true);
            areCheatsRendered = true;
        }
    }

    /// <summary>
    /// Function that will toggle the damage of the pistol from 3 to 99
    /// </summary>
    public void ToggleLethalPistol()
    {
        if (isPistolLethal)
        {
            // Set it back to 3
            playerPistol.damage = 3;
            lethalPistolText.color = Color.white;
            isPistolLethal = false;
        }
        else
        {
            // Set it to 99
            playerPistol.damage = 99;
            lethalPistolText.color = Color.green;
            isPistolLethal = true;
        }
    }

    /// <summary>
    /// Function that will set the health of the player to 100
    /// </summary>
    public void Toggle100HealthMode()
    {
        if (isHealth100)
        {
            // Set it back to default
            playerData.maxHealth = defaultMaxHealth;
            health100Text.color = Color.white;
            isHealth100 = false;
        }
        else
        {
            // Set it to 100
            playerData.maxHealth = 100;
            health100Text.color = Color.green;
            isHealth100 = true;
        }
    }

    /// <summary>
    /// Option that will unlock all
    /// </summary>
    public void unlockAll()
    {
        if (!clickedUnlockAll)
        {
            // Unlock all

            clickedUnlockAll = true;
        }
    }
}
