using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OldAbilityScreens: MonoBehaviour
{
    [SerializeField] private AbilitySelectUI WSUI;
    private GameObject abilselected;
    private Ability abilityselected;
    public Canvas Screen1;
    public Canvas Screen2;

    [Header("Canvas Fields")]
    [SerializeField] private TextMeshProUGUI fieldName;
    [SerializeField] private TextMeshProUGUI fieldDesc;
    [SerializeField] private Image fieldImg;

    [Header("Current Data")]
    [SerializeField] string abilityName;
    [SerializeField] string abilityDesc;
    [SerializeField] Sprite abilityImg;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        abilselected = WSUI.scanforhover();
        if (abilselected != null)
        {

            abilityselected = abilselected.GetComponent<HoverAndLerp2>().GetScriptableWeapon().GetAbilityObject();
            SetFields(abilityselected);
            Color temp = fieldImg.GetComponent<Image>().color;
            temp.a = 1;
            fieldImg.GetComponent<Image>().color = temp; 
        }
        if (abilselected == null)
        {
            abilselected = null;
            abilityselected = null;
            clearFields();
        }


    }
    private void SetFields(Ability wep)
    {
        abilityName = ("Name: " + wep.sName);
        abilityDesc = (wep.description);
        abilityImg = wep.abilityIconActive;
        if (wep.abilityIconActive == null) print("NULL ICON");
        writeFields();

    }

    private void writeFields()
    {
        fieldName.text = abilityName;
        fieldDesc.text = abilityDesc;
        fieldImg.GetComponent<Image>().sprite = abilityImg;
    }
    private void clearFields()
    {
        fieldName.text = string.Empty;
        fieldDesc.text = string.Empty;
        Color temp = Color.white;
        temp.a = 0;
        fieldImg.GetComponent<Image>().color = temp;
    }
}
