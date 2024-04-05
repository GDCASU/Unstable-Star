using FMOD;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Manager for the weapons HUD on gameplay
/// </summary>
public class WeaponDisplayHandler : MonoBehaviour
{
    //public GameObject pistolprefab;
    //public Sprite pistolSprite;
    //public GameObject buckprefab;
    //public Sprite buckSprite;
    //private int numUpdates = 0;
    //bool happened;
    private bool PIsInFront = true;
    private Animator animator;

    [Header("Canvas Objects")]
    [SerializeField] private GameObject primaryCanvasObj;
    [SerializeField] private GameObject secondaryCanvasObj;

    [Header("Icon Objects")]
    [SerializeField] private GameObject primaryIconObj;
    [SerializeField] private GameObject secondaryIconObj;

    [Header("Meter Objects")]
    [SerializeField] private GameObject primaryMeterObj;
    [SerializeField] private GameObject secondaryMeterObj;

    // IAN ADDED VARIABLES

    private Canvas primaryCanvas;
    private Canvas secondaryCanvas;
    private Image primaryImageComp;
    private Image secondaryImageComp;

    // References
    private Weapon primaryWep;
    private Weapon secondaryWep;

    // END OF NEW VARS

    private void Awake()
    {
        // Subscribe to input events
        PlayerInput.OnSwitchToNextWeapon += swapPanes;

        // Get components
        primaryImageComp = primaryIconObj.GetComponent<Image>();
        secondaryImageComp = secondaryIconObj.GetComponent<Image>();
    }

    // Unsubscribe to input events on destroy
    private void OnDestroy()
    {
        PlayerInput.OnSwitchToNextWeapon -= swapPanes;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        animator = this.GetComponent<Animator>();
        primaryCanvas = primaryCanvasObj.GetComponent<Canvas>();
        secondaryCanvas = secondaryCanvasObj.GetComponent<Canvas>();

        // Set the images of the HUD (This assumes there can only be two weapons equipped)
        primaryWep = WeaponArsenal.instance.GetCurrentWeapon();
        primaryImageComp.sprite = primaryWep.weaponIcon;
        // Cycle to next
        WeaponArsenal.instance.SwitchToNextWeapon();
        // Get next icon
        secondaryWep = WeaponArsenal.instance.GetCurrentWeapon();
        secondaryImageComp.sprite = secondaryWep.weaponIcon;
        // Return to primary
        WeaponArsenal.instance.SetCurrentWeaponToFirst();

        /*
        primaryName = weaponArsenalScript.weaponArsenal[3].sName;
        secondaryName = weaponArsenalScript.weaponArsenal[1].sName;
        for (int i = 0; i < primarycanvases.Length; i++)
        {
            primarycanvases[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < secondarycanvases.Length; i++)
        {
            secondarycanvases[i].gameObject.SetActive(false);
        }


        for (int i = 0; i < primarycanvases.Length; i++)
        {
            if (primarycanvases[i].tag == primaryName)
            {
                //primary.transform.position = new Vector3(-35, -27, 0);
                primarycanvases[i].gameObject.SetActive(true);
                currPrim = primarycanvases[i];
                primarycanvases[i].transform.GetChild(1).GetComponentInChildren<Image>().sprite = weaponArsenalScript.weaponArsenal[i].weaponIcon;
            }
        }
        for (int i = 0; i < secondarycanvases.Length; i++)
        {
            if (secondarycanvases[i].tag == secondaryName)
            {
                //secondary.transform.position = new Vector3(-35, -27, 10);
                secondarycanvases[i].gameObject.SetActive(true);
                currSecondary = secondarycanvases[i];
                secondarycanvases[i].transform.GetChild(1).GetComponentInChildren<Image>().sprite = weaponArsenalScript.weaponArsenal[i].weaponIcon;
            }
        }
        

        currSecondary.sortingOrder = 1;
        currPrim.sortingOrder = 2;
        UnityEngine.Debug.Log("currprim: " + currPrim.name);
        UnityEngine.Debug.Log("currsecondary: " + currSecondary.name);
        //animator.SetBool("toPIF", true);

        */
    }

    // Function called by the animatior to change the sorting layers of the canvases
    public void SwapSortingOrder()
    {
        // Swap variables using a tuple
        (secondaryCanvas.sortingOrder, primaryCanvas.sortingOrder) = (primaryCanvas.sortingOrder, secondaryCanvas.sortingOrder);
    }

    // Functions called by the animator to lock weapon switching input until animation has completed
    public void LockWeaponSwitch() { PlayerInput.instance.isWeaponSwitching = true; }
    public void ReleaseWeaponSwitch() { PlayerInput.instance.isWeaponSwitching = false; }

    public void swapPanes()
    {
        // If primary is in front, clock the secondary foward
        if (PIsInFront)
        {
            animator.SetBool("toCSTF", true);
            animator.SetBool("toCPTF", false);
            PIsInFront = false;
        }
        // Else, clock the primary foward
        else
        {
            animator.SetBool("toCSTF", false);
            animator.SetBool("toCPTF", true);
            PIsInFront = true;
        }

        /*
        RectTransform secrtransform = secondary.GetComponent<RectTransform>();
        RectTransform primrtransform = primary.GetComponent<RectTransform>();
        if (PIsInFront)
        {
            currSecondary.sortingOrder = 2;
            currPrim.sortingOrder = 1;
            UnityEngine.Debug.Log("currSecondary.sortingOrder: " + currSecondary.sortingOrder);
            UnityEngine.Debug.Log("currPrim.sortingOrder: " + currPrim.sortingOrder);
            animator.SetBool("toCSTF", true);
            animator.SetBool("toCPTF", false);
            PIsInFront = false;
        }
        else if (!PIsInFront)
        {
            currSecondary.sortingOrder = 1;
            currPrim.sortingOrder = 2;
            UnityEngine.Debug.Log("currSecondary.sortingOrder: " + currSecondary.sortingOrder);
            UnityEngine.Debug.Log("currPrim.sortingOrder: " + currPrim.sortingOrder);
            animator.SetBool("toCPTF", true);
            animator.SetBool("toCSTF", false);
            PIsInFront = true;
        }
        UnityEngine.Debug.Log("currSecondary.sortingOrder: " + currSecondary.sortingOrder);
        UnityEngine.Debug.Log("currPrim.sortingOrder: " + currPrim.sortingOrder);
        */

        /* if (animator.GetBool("toPIF") == true)
         {
             animator.SetBool("toCSTF", true);
             animator.SetBool("toPIF", false);
             //if (happened)
             //{
             currSecondary.sortingOrder = 2;
             currPrim.sortingOrder = 1;
             //}
         }
         if (animator.GetBool("toCSTF") == true && secrtransform.localPosition.z < 1f)
         {
             animator.SetBool("toSIF", true);
             animator.SetBool("toCSTF", false);
             PIsInFront = false;
             //if(happened)
             //{
             currSecondary.sortingOrder = 2;
             currPrim.sortingOrder = 1;
             // }
         }
         if (animator.GetBool("toSIF") == true)
         {
             animator.SetBool("toCPTF", true);
             animator.SetBool("toSIF", false);
             // if (happened)
             //{
             currSecondary.sortingOrder = 1;
             currPrim.sortingOrder = 2;
             //}
         }
         if (animator.GetBool("toCPTF") == true && (secrtransform.localPosition.z > 59f))
         {
             animator.SetBool("toPIF", true);
             animator.SetBool("toCPTF", false);
             PIsInFront = true;
             //if(happened)
             //{
             currSecondary.sortingOrder = 1;
             currPrim.sortingOrder = 2;
             // }
         }*/


        /*if (animator.GetBool("toPIF")==true&& Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("toCPTB", true);
            animator.SetBool("toPIF", false);
            if (happened)
            {
                currSecondary.sortingOrder = 2;
                currPrim.sortingOrder = 1;
            }
        }*/
        /*if(animator.GetBool("toCPTB")==true && (secrtransform.localPosition.z < 0.1f))
        {
            //animator.SetBool("toSIFCounter", true);
            animator.SetBool("toSIF", true);
            animator.SetBool("toCPTB", false);
        }*/
        /*if (animator.GetBool("toSIF") == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("toCSTB", true);
            animator.SetBool("toSIF", false);
            if (happened)
            {
                currSecondary.sortingOrder = 1;
                currPrim.sortingOrder = 2;
            }
        }
        if(animator.GetBool("toCSTB")==true && secrtransform.localPosition.z > 55f)
        {
            animator.SetBool("toPIF", true);
            animator.SetBool("toCSTB", false);
        }*/

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


#if false
    public void testMethodForShantanu()
    {
        numUpdates++;
        RectTransform secrtransform = secondary.GetComponent<RectTransform>();
        RectTransform primrtransform = primary.GetComponent<RectTransform>();
        //animator.SetFloat("Sec_Z", rtransform.localPosition.z);
        //UnityEngine.Debug.Log("Sec z: " + animator.GetFloat("toPIF"));
        //UnityEngine.Debug.Log("Local Position: " + secrtransform.localPosition.z);
        //UnityEngine.Debug.Log(rtransform.position.z);
        if (secrtransform.localPosition.z > 59.9f)
        {
            UnityEngine.Debug.Log("IT HAPPENED");
        }
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //happened = true;
        if (numUpdates == 1)
        {
            for (int i = 0; i < primarycanvases.Length; i++)
            {
                if (primarycanvases[i].tag == primaryName)
                {
                    //primary.transform.position = new Vector3(-35, -27, 0);
                    primarycanvases[i].gameObject.SetActive(true);
                    currPrim = primarycanvases[i];
                    primarycanvases[i].transform.GetChild(1).GetComponentInChildren<Image>().sprite = weaponArsenalScript.weaponArsenal[i].weaponIcon;
                }
            }
            for (int i = 0; i < secondarycanvases.Length; i++)
            {
                if (secondarycanvases[i].tag == secondaryName)
                {
                    //secondary.transform.position = new Vector3(-35, -27, 10);
                    secondarycanvases[i].gameObject.SetActive(true);
                    currSecondary = secondarycanvases[i];
                    secondarycanvases[i].transform.GetChild(1).GetComponentInChildren<Image>().sprite = weaponArsenalScript.weaponArsenal[i].weaponIcon;
                }
            }
            currSecondary.sortingOrder = 1;
            currPrim.sortingOrder = 2;
            UnityEngine.Debug.Log("currprim: " + currPrim.name);
            UnityEngine.Debug.Log("currsecondary: " + currSecondary.name);
            animator.SetBool("toPIF", true);
        }
    }
#endif

#if false
    public void myAnimPlay()
    {
        numUpdates++;
        RectTransform secrtransform = secondary.GetComponent<RectTransform>();
        RectTransform primrtransform = primary.GetComponent<RectTransform>();
        //animator.SetFloat("Sec_Z", rtransform.localPosition.z);
        //UnityEngine.Debug.Log("Sec z: " + animator.GetFloat("toPIF"));
        //UnityEngine.Debug.Log("Local Position: " + secrtransform.localPosition.z);
        //UnityEngine.Debug.Log(rtransform.position.z);
        if (secrtransform.localPosition.z > 59.9f)
        {
            UnityEngine.Debug.Log("IT HAPPENED");
        }
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //happened = true;
        if (numUpdates == 1)
        {
            for (int i = 0; i < primarycanvases.Length; i++)
            {
                if (primarycanvases[i].tag == primaryName)
                {
                    //primary.transform.position = new Vector3(-35, -27, 0);
                    primarycanvases[i].gameObject.SetActive(true);
                    currPrim = primarycanvases[i];
                    primarycanvases[i].transform.GetChild(1).GetComponentInChildren<Image>().sprite = weaponArsenalScript.weaponArsenal[i].weaponIcon;
                }
            }
            for (int i = 0; i < secondarycanvases.Length; i++)
            {
                if (secondarycanvases[i].tag == secondaryName)
                {
                    //secondary.transform.position = new Vector3(-35, -27, 10);
                    secondarycanvases[i].gameObject.SetActive(true);
                    currSecondary = secondarycanvases[i];
                    secondarycanvases[i].transform.GetChild(1).GetComponentInChildren<Image>().sprite = weaponArsenalScript.weaponArsenal[i].weaponIcon;
                }
            }
            currSecondary.sortingOrder = 1;
            currPrim.sortingOrder = 2;
            UnityEngine.Debug.Log("currprim: " + currPrim.name);
            UnityEngine.Debug.Log("currsecondary: " + currSecondary.name);
            animator.SetBool("toPIF", true);
        }
        //}
        float epsilon = 0.01f;
        UnityEngine.Debug.Log("happened: " + happened);
        UnityEngine.Debug.Log("newlocalposition: " + secrtransform.localPosition.z + " type: " + secrtransform.localPosition.z.GetType());

        if (animator.GetBool("toPIF") == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetBool("toCSTF", true);
            animator.SetBool("toPIF", false);
            //if (happened)
            //{
            currSecondary.sortingOrder = 2;
            currPrim.sortingOrder = 1;
            //}
        }
        if (animator.GetBool("toCSTF") == true && secrtransform.localPosition.z < 1f)
        {
            animator.SetBool("toSIF", true);
            animator.SetBool("toCSTF", false);
            //if(happened)
            //{
            currSecondary.sortingOrder = 2;
            currPrim.sortingOrder = 1;
            // }
        }
        if (animator.GetBool("toSIF") == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetBool("toCPTF", true);
            animator.SetBool("toSIF", false);
            // if (happened)
            //{
            currSecondary.sortingOrder = 1;
            currPrim.sortingOrder = 2;
            //}
        }
        if (animator.GetBool("toCPTF") == true && (secrtransform.localPosition.z > 59f))
        {
            animator.SetBool("toPIF", true);
            animator.SetBool("toCPTF", false);
            //if(happened)
            //{
            currSecondary.sortingOrder = 1;
            currPrim.sortingOrder = 2;
            // }

        }
    }
#endif
}
