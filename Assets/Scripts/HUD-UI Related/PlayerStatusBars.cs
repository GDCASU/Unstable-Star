using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusBars : MonoBehaviour
{
    [Header("Health Bar Objects")]
    [SerializeField] private Transform healthBarBlackedOut;
    [SerializeField] private Transform healthBarColored;

    [Header("Shield Bar Objects")]
    [SerializeField] private Transform shieldBarBlackedOut;
    [SerializeField] private Transform shieldBarColored;

    // Local Vars
    private readonly float statBarsOffset = 30; //As in, the space between the bars in the HUD
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

        // Set Vars
        healthBefore = Player.instance.GetMaxHealth();
        shieldBefore = Player.instance.GetMaxShield();

        SetupStatBars();
    }

    // Unsubscribe from events on destroy
    private void OnDestroy()
    {
        EventData.OnHealthGained -= OnHealthGained;
        EventData.OnHealthLost -= OnHealthLost;
        EventData.OnShieldGained -= OnShieldGained;
        EventData.OnShieldDamaged -= OnShieldDamaged;
        EventData.OnShieldBroken -= OnShieldDamaged;
    }

    // Setup the UI Stat Bars
    // TODO: Maybe if Max Health or Max Shield can be upgraded down the line
    // This code could be modified to account for that
    private void SetupStatBars()
    {
        //Readability Vars
        int playerMaxHealth = Player.instance.GetMaxHealth();
        int playerMaxShield = Player.instance.GetMaxShield();

        // Scale the UI segments in respect to the player's Max Health
        if (playerMaxHealth > 1)
        {
            foreach (Transform t in new Transform[] { healthBarBlackedOut, healthBarColored })
            {
                for (int i = 2; i < playerMaxHealth; i++)
                {
                    GameObject segment = Instantiate(t.GetChild(i - 1).gameObject, t);
                    segment.transform.localPosition = t.GetChild(i - 1).localPosition + new Vector3(statBarsOffset, 0, 0);
                }
            }
        }
        else
        {
            Destroy(healthBarBlackedOut.Find("Middle").gameObject);
            Destroy(healthBarColored.Find("Middle").gameObject);
        }

        // Scale the UI segments in respect to the player's Max Shield
        if (playerMaxShield > 1)
        {
            foreach (Transform t in new Transform[] { shieldBarBlackedOut, shieldBarColored })
            {
                for (int i = 2; i < playerMaxShield; i++)
                {
                    GameObject segment = Instantiate(t.GetChild(i - 1).gameObject, t);
                    segment.transform.localPosition = t.GetChild(i - 1).localPosition + new Vector3(statBarsOffset, 0, 0);
                }
            }
        }
        else
        {
            Destroy(shieldBarBlackedOut.Find("Middle").gameObject);
            Destroy(shieldBarColored.Find("Middle").gameObject);
        }
    }

    #region HEALTH CHECKS

    private void OnHealthGained(int currHealth)
    {
        for (int i = healthBefore; i < currHealth; i++)
        {
            healthBarColored.GetChild(i).gameObject.SetActive(true);
        }

        // Update before variable
        healthBefore = currHealth;
    }

    private void OnHealthLost(int currHealth)
    {
        for (int i = healthBefore - 1; i > currHealth - 1; i--)
        {
            healthBarColored.GetChild(i).gameObject.SetActive(false);
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
            shieldBarColored.GetChild(i).gameObject.SetActive(true);
        }
        
        // Update before variable
        shieldBefore = currShield;
    }

    private void OnShieldDamaged(int currShield)
    {
        // Will go through the segment list from the last shield segment index to the current one
        for (int i = shieldBefore - 1; i > currShield - 1; i--)
        {
            shieldBarColored.GetChild(i).gameObject.SetActive(false);
        }

        // Update before variable
        shieldBefore = currShield;
    }

    #endregion

}
