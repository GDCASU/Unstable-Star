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

    [Header("Settings")]
    [SerializeField] private Color cooldownColor;
    [SerializeField] private Color chargeColor;
    [SerializeField] private Color fullChargeColor;

    // Canvases-Icons
    private Canvas primaryCanvas;
    private Canvas secondaryCanvas;
    private Image primaryIcon;
    private Image secondaryIcon;

    // Meters
    private Image primaryMeter;
    private Image secondaryMeter;
    private RectTransform primaryMeterRect;
    private RectTransform secondaryMeterRect;
    private float maxMeterHeight;
    private float lastMeterHeightPrimary;
    private float lastMeterHeightSecondary;

    // Weapon References
    private Weapon primaryWep;
    private Weapon secondaryWep;

    private void Awake()
    {
        // Get Weapon Icon components
        primaryIcon = primaryIconObj.GetComponent<Image>();
        secondaryIcon = secondaryIconObj.GetComponent<Image>();

        // Get Meter Components
        primaryMeter = primaryMeterObj.GetComponent<Image>();
        secondaryMeter = secondaryMeterObj.GetComponent<Image>();
        primaryMeterRect = primaryMeterObj.GetComponent<RectTransform>();
        secondaryMeterRect = secondaryMeterObj.GetComponent<RectTransform>();

        // Get values for meter modifying
        maxMeterHeight = primaryMeterRect.offsetMax.y * -1;

        // Subscribe to input events
        PlayerInput.OnSwitchToNextWeapon += swapPanes;
        EventData.OnPlayerDeath += UnsubscribeFromEvents;

        // Set variables
        lastMeterHeightPrimary = maxMeterHeight;
        lastMeterHeightSecondary = maxMeterHeight;
}

    // Unsubscribe to input events on destroy
    private void OnDestroy()
    {
        UnsubscribeFromEvents();
        EventData.OnPlayerDeath -= UnsubscribeFromEvents;
    }

    // In a separate function so it can also be stopped if the player dies
    private void UnsubscribeFromEvents()
    {
        PlayerInput.OnSwitchToNextWeapon -= swapPanes;
        primaryWep.ModifyMeterCharge -= UpdateChargeMeterPrimary;
        primaryWep.ModifyMeterCooldown -= UpdateCooldownMeterPrimary;
        secondaryWep.ModifyMeterCharge -= UpdateChargeMeterSecondary;
        secondaryWep.ModifyMeterCooldown -= UpdateCooldownMeterSecondary;
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
        primaryIcon.sprite = primaryWep.weaponIcon;
        // Cycle to next
        WeaponArsenal.instance.SwitchToNextWeapon();
        // Get next icon
        secondaryWep = WeaponArsenal.instance.GetCurrentWeapon();
        secondaryIcon.sprite = secondaryWep.weaponIcon;
        // Return to primary
        WeaponArsenal.instance.SetCurrentWeaponToFirst();

        // Attach the weapons to their UI events
        primaryWep.ModifyMeterCharge += UpdateChargeMeterPrimary;
        primaryWep.ModifyMeterCooldown += UpdateCooldownMeterPrimary;
        secondaryWep.ModifyMeterCharge += UpdateChargeMeterSecondary;
        secondaryWep.ModifyMeterCooldown += UpdateCooldownMeterSecondary;
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

    // Called by ShootComponent to Update the meters on the HUD
    private void UpdateCooldownMeterPrimary(float maxVal, float currVal)
    {
        // Set the color to Cooldown
        primaryMeter.color = cooldownColor;
        // Compute height of the meter
        float rateOfChange = (maxMeterHeight - lastMeterHeightPrimary) / (0 - currVal);
        float computedHeight = rateOfChange * (currVal - maxVal);
        Debug.Log("MAX METER VAL = " + maxMeterHeight + "\nLAST METER VAL = " + lastMeterHeightPrimary);
        // Set the height of the meter
        primaryMeterRect.offsetMax = new Vector2 (primaryMeterRect.offsetMax.x, computedHeight * -1);
        //if (computedHeight <= 0) lastMeterHeightPrimary = 0; 
    }

    private void UpdateChargeMeterPrimary(float maxVal, float currVal)
    {
        // Set the color to the charging color
        primaryMeter.color = chargeColor;
        // Compute height of the meter
        float rateOfChange = (0f - maxMeterHeight) / maxVal;
        float computedHeight = rateOfChange * (maxVal - currVal) + maxMeterHeight;
        // Set the height of the meter
        primaryMeterRect.offsetMax = new Vector2(primaryMeterRect.offsetMax.x, computedHeight * -1);
        // Update latest computed height for UI
        lastMeterHeightPrimary = maxMeterHeight - computedHeight;
        // If the charge is at full, Update it to the full charge color
        if (currVal == 0f) { primaryMeter.color = fullChargeColor; }
    }

    private void UpdateCooldownMeterSecondary(float maxVal, float currVal)
    {
        // Set the color to Cooldown
        secondaryMeter.color = cooldownColor;
        // Compute height of the meter
        float rateOfChange = lastMeterHeightSecondary / maxVal;
        float computedHeight = rateOfChange * (maxVal - currVal);
        // Set the height of the meter
        secondaryMeterRect.offsetMax = new Vector2(secondaryMeterRect.offsetMax.x, computedHeight * -1);
        if (computedHeight <= 0) lastMeterHeightSecondary = 0;
    }

    private void UpdateChargeMeterSecondary(float maxVal, float currVal)
    {
        // Set the color to the charging color
        secondaryMeter.color = chargeColor;
        // Compute height of the meter
        float rateOfChange = (0f - maxMeterHeight) / maxVal;
        float computedHeight = rateOfChange * (maxVal - currVal) + maxMeterHeight;
        // Set the height of the meter
        secondaryMeterRect.offsetMax = new Vector2(secondaryMeterRect.offsetMax.x, computedHeight * -1);
        // Update latest computed height for UI
        lastMeterHeightSecondary = computedHeight;
        // If the charge is at full, Update it to the full charge color
        if (currVal == 0f) { secondaryMeter.color = fullChargeColor; }
    }

    // Function that updates the animator controller based on Player Input
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
