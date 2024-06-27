using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class HoverAndLerp : MonoBehaviour
{
    public float hoverHeight = 1f;
    public float lerpTime = 1f;

    public bool isHovered = false;
    private Vector3 originalPosition;
    [SerializeField] private Material blackout;
    public Material transDisk;
    private Material initMAT;
    public bool selected = false;
    public bool unlocked = true;
    private WeaponSelectUI WSUI;
    private Material initDiskMat;
    [SerializeField] private ScriptableWeapon weapon = null;



    private Coroutine lerpCoroutine;

    void Start()
    {


        WSUI = GetComponentInParent<WeaponSelectUI>();
        originalPosition = transform.position;
        initMAT = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        string disksGunName = weapon.name;
        switch(disksGunName)
        {
            case "Player Gatling Gun":
                unlocked = SerializedDataManager.instance.gameData.isGatlingUnlocked;
                break;

            case "Player Laser":
                unlocked = SerializedDataManager.instance.gameData.isLaserUnlocked;
                break ;
            case "Player Pistol":
                break ;

            default:
                Debug.Log("No Gun Name Found");
                break;
        }
        if(!unlocked)
        {
            initDiskMat = gameObject.GetComponent<Material>();
            gameObject.GetComponent<Renderer>().material = transDisk;
        }
    }

    void Update()
    {
            if(isHovered && unlocked)
            {
                    if(Input.GetKeyDown(KeyCode.Mouse0))
                    {

                        WSUI.CheckWeaponEquipLoad(this);

                    }
            }
    }

    /// <summary>
    /// Equips gun located on Floppy Disk.
    /// </summary>
    public void EquipPass()
    {
        
        if (!selected)
        {
            transform.GetChild(0).gameObject.GetComponent<Renderer>().material = blackout;
            WSUI.Equipped++;
            WSUI.setArseWeapon(weapon.GetWeaponObject());
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<Renderer>().material = initMAT;
            WSUI.Equipped--;
            WSUI.removeArseWeapon(weapon.GetWeaponObject());
        }
        selected = !selected;
    }


    private void OnMouseEnter()
    {
        isHovered = true;
        if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
        lerpCoroutine = StartCoroutine(LerpObject(transform.position, new Vector3(originalPosition.x, hoverHeight, transform.position.z)));
    }

    private void OnMouseExit()
    {
        isHovered = false;
        if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
        lerpCoroutine = StartCoroutine(LerpObject(transform.position, originalPosition));
    }

    IEnumerator LerpObject(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpTime)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime/lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        lerpCoroutine = null;
    }

    public ScriptableWeapon GetScriptableWeapon()
    {
        return weapon;
    }
}
