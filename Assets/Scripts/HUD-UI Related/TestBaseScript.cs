using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBaseScript : MonoBehaviour
{
    public GameObject prefab;
    public Sprite pistolSprite;
    public Image testImage;
    public WeaponArsenal weaponArsenalScript;
    public SoundTag soundTag;

    // Start is called before the first frame update
    void Start()
    {
        weaponArsenalScript = GameObject.Find("Weapon Arsenal").GetComponent<WeaponArsenal>();
        Pistol myPistol = new Pistol(prefab, pistolSprite, soundTag, 3f, 2, "Pistol", 3f);
        weaponArsenalScript.AddWeaponToArsenal(myPistol);

        //weaponArsenalScript.weaponArsenal[0] = new Pistol(prefab, pistolSprite, soundTag, 3f, 2, "Pistol", 3f);
        testImage.sprite = weaponArsenalScript.weaponArsenal[0].weaponIcon;
    }


    // Update is called once per frame
    void Update()
    {
        UnityEngine.Debug.Log(weaponArsenalScript.weaponArsenal[0].sName);
        //if (Input.GetKeyDown(KeyCode.M))
        /*{ 
            Pistol myPistol = new Pistol(prefab, pistolSprite, soundTag, 3f, 2, "Pistol", 3f);
            weaponArsenalScript.AddWeaponToArsenal(myPistol);
            
            //weaponArsenalScript.weaponArsenal[0] = new Pistol(prefab, pistolSprite, soundTag, 3f, 2, "Pistol", 3f);
            testImage.sprite = weaponArsenalScript.weaponArsenal[0].weaponIcon;
        }*/
    }
}
