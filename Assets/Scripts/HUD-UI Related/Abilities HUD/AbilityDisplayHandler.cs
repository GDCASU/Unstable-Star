using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manager of the Ability HUD on gameplay, for now only works with only one ability equipped
/// </summary>
public class AbilityDisplayHandler : MonoBehaviour
{
    [Header("Ability HUD Settings")]
    [SerializeField] private Image activeAbilityObj;
    [SerializeField] private Image inactiveAbilityObj;
    [SerializeField] private TextMeshProUGUI chargeCounter;
    

    // Local Variables
    private Ability abilityRef;

    void Start()
    {
        // Get the equipped ability
        abilityRef = AbilityInventory.instance.GetCurrentAbility();
        if (abilityRef.sName == "No Ability")
        {
            gameObject.SetActive(false);
            return;
        }

        // Set the images and charges
        activeAbilityObj.sprite = abilityRef.abilityIconActive;
        inactiveAbilityObj.sprite = abilityRef.abilityIconInactive;
        chargeCounter.text = abilityRef.charges.ToString();

        // Suscribe to input events
        EventData.OnAbilityCooldown += RadialCooldown;
        EventData.OnPlayerDeath += UnsubscribeFromEvents;
    }

    // Unsuscribe from input events on destroy
    private void OnDestroy()
    {
        UnsubscribeFromEvents();
        EventData.OnPlayerDeath -= UnsubscribeFromEvents;
    }

    // In a separate function so it can also be stopped if the player dies
    private void UnsubscribeFromEvents()
    {
        EventData.OnAbilityCooldown -= RadialCooldown;
    }

    // Function that will be called from within the Ability Component when an ability is used
    private void RadialCooldown(float maxCooldown, float currentCooldown)
    {
        chargeCounter.text = abilityRef.charges.ToString();
        activeAbilityObj.fillAmount = (maxCooldown - currentCooldown) / maxCooldown;
        // If the charges are 0, just grey out the ability Icon
        if (abilityRef.charges <= 0) { activeAbilityObj.fillAmount = 0f; }
    }
}
