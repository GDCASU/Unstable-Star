using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScreens : MonoBehaviour
{
    [SerializeField] private WeaponSelectUI WSUI;
    private GameObject gunselected;
    private Weapon weaponselected;
    public Canvas Screen1;
    public Canvas Screen2;

    [Header("Canvas Fields")]
    [SerializeField] private TextMeshProUGUI fieldName;
    [SerializeField] private TextMeshProUGUI fieldDesc;
    [SerializeField] private TextMeshProUGUI fieldDam;
    [SerializeField] private RawImage fieldImg;

    [Header("Current Data")]
    [SerializeField] string wepName;
    [SerializeField] string wepDesc;
    [SerializeField] string wepDam;
    [SerializeField] Sprite wepImg;


    public RawImage nothing;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gunselected = WSUI.scanforhover();
        if(gunselected != null) {
            weaponselected = gunselected.GetComponent<HoverAndLerp>().GetScriptableWeapon().GetWeaponObject();
            SetFields(weaponselected);
        }
        if (gunselected == null)
        {
            gunselected = null;
            weaponselected = null;
            clearFields();
        }


    }
    private void SetFields(Weapon wep)
    {
        wepName = ("Name: " + wep.sName);
        wepDesc = ("\"" + wep.description + "\"");
        wepDam = ("Damage: " + getDam(wep));
        wepImg = wep.weaponIcon;
        if (wep.weaponIcon == null) print("NULL ICON");
        writeFields();

    }

    private string getDam(Weapon wep)
    {
        if(wep.damage != 0)
        {
            return wep.damage.ToString();
        }
        else
        {
            return (wep.minDamage.ToString() + " - " + wep.maxDamage.ToString());
        }
    }
    private void writeFields()
    {
        fieldName.text = wepName;
        fieldDesc.text = wepDesc;
        fieldDam.text = wepDam;
            }
    private void clearFields()
    {
        fieldName.text = string.Empty;
        fieldDesc.text = string.Empty;
        fieldDam.text = string.Empty;
    }
}
