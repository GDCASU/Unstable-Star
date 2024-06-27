using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSelectCanvas : MonoBehaviour
{
    private GameObject canvas = null;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerInput.OnPauseGame += HideWeaponMenu;
        PlayerInput.OnCancel += ShowWeaponMenu;
        canvas = gameObject.transform.GetChild(0).gameObject;
    }
        
    private void HideWeaponMenu()
    {
        canvas.SetActive(false);
    }

    private void ShowWeaponMenu()
    {
        canvas.SetActive(true);
    }
}
