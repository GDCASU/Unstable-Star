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
        // Suscribe to input events
        EventData.OnAbilityCooldown += RadialCooldown;

        // Get the equipped ability
        Ability currAbility = AbilityInventory.instance.GetCurrentAbility();

        // Set the images
        activeAbilityObj.sprite = currAbility.abilityIconActive;
        inactiveAbilityObj.sprite = currAbility.abilityIconInactive;
    }

    // Unsuscribe from input events on destroy
    private void OnDestroy()
    {
        EventData.OnAbilityCooldown -= RadialCooldown;
    }

    // Function that will be called from within the Ability Component when an ability is used
    private void RadialCooldown(float maxCooldown, float currentCooldown)
    {
        activeAbilityObj.fillAmount = (maxCooldown - currentCooldown) / maxCooldown;
    }
}
