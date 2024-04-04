using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityToggle : MonoBehaviour
{
    [SerializeField] AbilityInventory abilityChosen;

    // Start is called before the first frame update
    void Start()
    {
        Ability initializedAbility = abilityChosen.GetCurrentAbility();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
