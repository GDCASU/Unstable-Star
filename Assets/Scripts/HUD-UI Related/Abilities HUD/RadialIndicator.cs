using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radial : MonoBehaviour
{
    
    [SerializeField] private Image RadialIndicatorUI = null;

    private void Start()
    {
        EventData.OnAbilityCooldown += RadialCooldown;
    }
    private void OnDestroy()
    {
        EventData.OnAbilityCooldown -= RadialCooldown;
    }

    public void RadialCooldown(float maxCooldown, float currentCooldown)
    {

        RadialIndicatorUI.fillAmount = currentCooldown / maxCooldown;
        
    }
}
