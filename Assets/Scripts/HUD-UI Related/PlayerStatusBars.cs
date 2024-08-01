using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusBars : MonoBehaviour
{
    [Header("Health Bar Objects")]
    [SerializeField] private GameObject firstHealthBar;
    [SerializeField] private GameObject middleHealthBar;
    [SerializeField] private GameObject coloredHealthBarParent;
    [SerializeField] private GameObject blackedHealthBarParent;
    [SerializeField] private GameObject healthBarBlackedOut;
    [SerializeField] private GameObject healthBarColored;

    [Header("Shield Bar Objects")]
    [SerializeField] private GameObject firstShieldBar;
    [SerializeField] private GameObject middleShieldBar;
    [SerializeField] private GameObject coloredShieldBarParent;
    [SerializeField] private GameObject blackedShieldBarParent;
    [SerializeField] private GameObject shieldBarBlackedOut;
    [SerializeField] private GameObject shieldBarColored;

    [Header("Settings")]
    [SerializeField] private float statBarsOffset; //As in, the space between the bars in the HUD

    // Local Vars
    private List<GameObject> healthBarsColoredList = new List<GameObject>();
    private List<GameObject> shieldBarsColoredList = new List<GameObject>();

    private bool finishedSetup;
    private int healthBefore;
    private int shieldBefore;

    private void Start()
    {
        // Subscribe to Events
        EventData.OnHealthGained += OnHealthGained;
        EventData.OnHealthLost += OnHealthLost;
        EventData.OnShieldGained += OnShieldGained;
        EventData.OnShieldDamaged += OnShieldDamaged;
        EventData.OnShieldBroken += OnShieldDamaged;
        Player.hasLoadedStats += SetupStatBars;

    }

    // Unsubscribe from events on destroy
    private void OnDestroy()
    {
        EventData.OnHealthGained -= OnHealthGained;
        EventData.OnHealthLost -= OnHealthLost;
        EventData.OnShieldGained -= OnShieldGained;
        EventData.OnShieldDamaged -= OnShieldDamaged;
        EventData.OnShieldBroken -= OnShieldDamaged;
        Player.hasLoadedStats -= SetupStatBars;
    }

    // Setup the UI Stat Bars
    // TODO: Maybe if Max Health or Max Shield can be upgraded down the line
    // This code could be modified to account for that
    private void SetupStatBars()
    {
        // This bool stops it from bombing the game with objects
        if (finishedSetup) return;

        // Readability Vars
        int playerMaxHealth = Player.instance.GetMaxHealth();
        int playerMaxShield = Player.instance.GetMaxShield();

        // If the values are 0 or less, dont run
        if (playerMaxHealth < 1 || playerMaxShield < 1) return;

        // Scale the UI segments in respect to the player's Max Health
        if (playerMaxHealth > 1)
        {
            // Add the first two bars
            healthBarsColoredList.Add(firstHealthBar);
            healthBarsColoredList.Add(middleHealthBar);

            // Setup the health bars on the canvas
            Vector3 canvasPos = middleHealthBar.transform.localPosition;
            Vector3 barOffset = new Vector3(statBarsOffset, 0, 0);
            for (int i = 2; i < playerMaxHealth; i++)
            {
                // Compute the position
                canvasPos += barOffset;

                //Setup blacked bar
                GameObject segmentDark = Instantiate(healthBarBlackedOut, blackedHealthBarParent.transform);
                segmentDark.transform.localPosition = canvasPos;
                segmentDark.SetActive(true);

                // Setup colored bar
                GameObject segmentColor = Instantiate(healthBarColored, coloredHealthBarParent.transform);
                segmentColor.transform.localPosition = canvasPos;
                segmentColor.SetActive(true);
                healthBarsColoredList.Add(segmentColor);
            }
        }
        else
        {
            Debug.Log("<color=red> ERROR, PLAYER'S HEALTH IS LESS THAN 2 </color>");
        }

        // Scale the UI segments in respect to the player's Max Shield
        if (playerMaxShield > 1)
        {
            // Add the first two bars
            shieldBarsColoredList.Add(firstShieldBar);
            shieldBarsColoredList.Add(middleShieldBar);

            // Setup the health bars on the canvas
            Vector3 canvasPos = middleShieldBar.transform.localPosition;
            Vector3 barOffset = new Vector3(statBarsOffset, 0, 0);
            for (int i = 2; i < playerMaxShield; i++)
            {
                // Compute the position
                canvasPos += barOffset;

                //Setup blacked bar
                GameObject segmentDark = Instantiate(shieldBarBlackedOut, blackedShieldBarParent.transform);
                segmentDark.transform.localPosition = canvasPos;
                segmentDark.SetActive(true);

                // Setup colored bar
                GameObject segmentColor = Instantiate(shieldBarColored, coloredShieldBarParent.transform);
                segmentColor.transform.localPosition = canvasPos;
                segmentColor.SetActive(true);
                shieldBarsColoredList.Add(segmentColor);
            }
        }
        else
        {
            Debug.Log("<color=red> ERROR, PLAYER'S SHIELD IS LESS THAN 2 </color>");
        }

        // Set variables used for going through the HUD array
        healthBefore = Player.instance.GetMaxHealth();
        shieldBefore = Player.instance.GetMaxShield();

        // Finished
        finishedSetup = true;
    }

    #region HEALTH CHECKS

    private void OnHealthGained(int currHealth)
    {
        // healthPerBar should not be 0

        for (int i = healthBefore; i < currHealth; i++)
        {
            healthBarsColoredList[i].SetActive(true);
        }

        // Update before variable
        healthBefore = currHealth;
    }

    private void OnHealthLost(int currHealth)
    {
        for (int i = healthBefore - 1; i > currHealth - 1; i--)
        {
            healthBarsColoredList[i].SetActive(false);
        }

        // Update before variable
        healthBefore = currHealth;
    }

    #endregion

    #region SHIELD CHECKS

    private void OnShieldGained(int currShield)
    {
        // Will go through the segment list from the last shield segment index to the current one
        for (int i = shieldBefore; i < currShield; i++)
        {
            shieldBarsColoredList[i].SetActive(true);
        }
        
        // Update before variable
        shieldBefore = currShield;
    }

    private void OnShieldDamaged(int currShield)
    {
        // Will go through the segment list from the last shield segment index to the current one
        for (int i = shieldBefore - 1; i > currShield - 1; i--)
        {
            shieldBarsColoredList[i].SetActive(false);
        }

        // Update before variable
        shieldBefore = currShield;
    }

    #endregion
}
