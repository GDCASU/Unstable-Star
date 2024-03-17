using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBaseScript : MonoBehaviour
{
    public GameObject pistolprefab;
    public Sprite pistolSprite;
    public GameObject buckprefab;
    public Sprite buckSprite;
    public Image testImage;
    public WeaponArsenal weaponArsenalScript;
    public SoundTag soundTag;

    // Start is called before the first frame update
    void Start()
    {
        weaponArsenalScript = GameObject.Find("Weapon Arsenal").GetComponent<WeaponArsenal>();
        Pistol myPistol = new Pistol(pistolprefab, pistolSprite, soundTag, 3f, 2, "Pistol", 3f);
        Buckshot myBuck = new Buckshot(buckprefab, buckSprite, soundTag, 3f, 2, "Buck", 3f);
        weaponArsenalScript.AddWeaponToArsenal(myPistol);
        weaponArsenalScript.AddWeaponToArsenal(myBuck);

        //weaponArsenalScript.weaponArsenal[0] = new Pistol(prefab, pistolSprite, soundTag, 3f, 2, "Pistol", 3f);
        testImage.sprite = weaponArsenalScript.weaponArsenal[1].weaponIcon;
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
