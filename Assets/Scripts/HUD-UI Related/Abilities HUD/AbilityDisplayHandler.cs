using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager of the Ability HUD on gameplay, for now only works with only one ability equipped
/// </summary>
public class AbilityDisplayHandler : MonoBehaviour
{
    [Header("Ability HUD Settings")]
    [SerializeField] private Image activeAbilityObj;
    [SerializeField] private Image inactiveAbilityObj;

    void Start()
    {
        // Get the equipped ability
        Ability currAbility = AbilityInventory.instance.GetCurrentAbility();

        // Set the images
        activeAbilityObj.sprite = currAbility.abilityIconActive;
        inactiveAbilityObj.sprite = currAbility.abilityIconInactive;

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
        activeAbilityObj.fillAmount = (maxCooldown - currentCooldown) / maxCooldown;
    }
}
