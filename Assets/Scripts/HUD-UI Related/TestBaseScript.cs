using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBaseScript : MonoBehaviour
{
    //public GameObject pistolprefab;
    //public Sprite pistolSprite;
    //public GameObject buckprefab;
    //public Sprite buckSprite;
    private Animator animator;
    public GameObject primary;
    public GameObject secondary;
    string primaryName;
    string secondaryName;
    public Canvas[] primarycanvases;
    public Canvas[] secondarycanvases;
    public Image testImage;
    public WeaponArsenal weaponArsenalScript;
    bool happened;

  //  public SoundTag soundTag;

    // Start is called before the first frame update
    void Start()
    {
        happened = false;
        animator = this.GetComponent<Animator>();
        animator.SetBool("toSIF", false);
        animator.SetBool("toPIF", false);
        animator.SetBool("toCSTF", false);
        animator.SetBool("toCPTF", false);
        weaponArsenalScript = GameObject.Find("Weapon Arsenal").GetComponent<WeaponArsenal>();

        /*Pistol myPistol = new Pistol(pistolprefab, pistolSprite, soundTag, 3f, 2, "Pistol", 3f);
        Buckshot myBuck = new Buckshot(buckprefab, buckSprite, soundTag, 3f, 2, "Buck", 3f);
        weaponArsenalScript.AddWeaponToArsenal(myPistol);
        weaponArsenalScript.AddWeaponToArsenal(myBuck);*/

        //weaponArsenalScript.weaponArsenal[0] = new Pistol(prefab, pistolSprite, soundTag, 3f, 2, "Pistol", 3f);
        testImage.sprite = weaponArsenalScript.weaponArsenal[0].weaponIcon;
        primaryName = weaponArsenalScript.weaponArsenal[0].sName;
        secondaryName = weaponArsenalScript.weaponArsenal[1].sName;
        for(int i = 0; i<primarycanvases.Length; i++)
        {
            primarycanvases[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < secondarycanvases.Length; i++)
        {
            secondarycanvases[i].gameObject.SetActive(false);
        }
        
        



        /* UnityEngine.Debug.Log(weaponArsenalScript.weaponArsenal[0].sName);
         UnityEngine.Debug.Log(weaponArsenalScript.weaponArsenal[1].sName);
         UnityEngine.Debug.Log(weaponArsenalScript.weaponArsenal[2].sName);
         UnityEngine.Debug.Log(weaponArsenalScript.weaponArsenal[3].sName);
         UnityEngine.Debug.Log(weaponArsenalScript.weaponArsenal[4].sName);*/

    }


    // Update is called once per frame
    void Update()
    {
        RectTransform secrtransform = secondary.GetComponent<RectTransform>();
        RectTransform primrtransform = primary.GetComponent<RectTransform>();
        //animator.SetFloat("Sec_Z", rtransform.localPosition.z);
        UnityEngine.Debug.Log("Sec z: " + animator.GetFloat("toPIF"));
        UnityEngine.Debug.Log("Local Position: " + secrtransform.localPosition.z);
        //UnityEngine.Debug.Log(rtransform.position.z);
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            happened = true;
            for (int i = 0; i < primarycanvases.Length; i++)
            {
                if (primarycanvases[i].tag == primaryName)
                {
                    //primary.transform.position = new Vector3(-35, -27, 0);
                    primarycanvases[i].gameObject.SetActive(true);
                    primarycanvases[i].GetComponentInChildren<Image>().sprite = weaponArsenalScript.weaponArsenal[0].weaponIcon;
                }
            }
            for (int i = 0; i < secondarycanvases.Length; i++)
            {
                if (secondarycanvases[i].tag == secondaryName)
                {
                    //secondary.transform.position = new Vector3(-35, -27, 10);
                    secondarycanvases[i].gameObject.SetActive(true);
                    secondarycanvases[i].GetComponentInChildren<Image>().sprite = weaponArsenalScript.weaponArsenal[1].weaponIcon;
                }
            }
        }
        float epsilon = 0.01f;
        UnityEngine.Debug.Log("happened: " + happened);
        UnityEngine.Debug.Log("newlocalposition: " + secrtransform.localPosition.z + " type: " + secrtransform.localPosition.z.GetType());
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("toCSTF", true);
        }
        if(secrtransform.localPosition.z < 1f)
        {
            animator.SetBool("toSIF", true);
            animator.SetBool("toCSTF", false);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            animator.SetBool("toCPTF", true);
            animator.SetBool("toSIF", false);
        }
        if (happened && (secrtransform.localPosition.z > 59f))
        {
            animator.SetBool("toPIF", true);
            animator.SetBool("toCPTF", false);
        }
        /*if (Input.GetKeyDown(KeyCode.N))
        {
            animator.SetFloat("sec_z", 60f);
        }*/
        //UnityEngine.Debug.Log(weaponArsenalScript.weaponArsenal[0].sName);
        //if (Input.GetKeyDown(KeyCode.M))
        /*{ 
            Pistol myPistol = new Pistol(prefab, pistolSprite, soundTag, 3f, 2, "Pistol", 3f);
            weaponArsenalScript.AddWeaponToArsenal(myPistol);

            //weaponArsenalScript.weaponArsenal[0] = new Pistol(prefab, pistolSprite, soundTag, 3f, 2, "Pistol", 3f);
            testImage.sprite = weaponArsenalScript.weaponArsenal[0].weaponIcon;
        }*/
    }
}
