using UnityEngine;
using System.Collections;
public class HoverAndLerp2 : MonoBehaviour
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
    private AbilitySelectUI WSUI;
    private Material initDiskMat;
    [SerializeField] private ScriptableAbility weapon = null;

    private Coroutine lerpCoroutine;

    void Start()
    {
        WSUI = GetComponentInParent<AbilitySelectUI>();
        originalPosition = transform.position;
        initMAT = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        string disksAbilityName = weapon.name;
        switch(disksAbilityName)
        {
            case "Player Phase Shift":
                unlocked = SerializedDataManager.instance.gameData.isPhaseShiftUnlocked;
                break;
            case "Player Proximity Bomb":
                unlocked = SerializedDataManager.instance.gameData.isProxiBombUnlocked;
                break;
            default:
                print("Error with name of ability");
                break;
        }
        if (!unlocked)
        {
            initDiskMat = gameObject.GetComponent<Material>();
            gameObject.GetComponent<Renderer>().material = transDisk;
        }
    }

    void Update()
    {
        if (isHovered && unlocked)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                WSUI.CheckAbilityEquipLoad(this);

            }
        }
    }


    public void EquipPass()
    {
        if (!selected)
        {
            transform.GetChild(0).gameObject.GetComponent<Renderer>().material = blackout;
            WSUI.Equipped++;
            WSUI.setArseAbility(weapon.GetAbilityObject());
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<Renderer>().material = initMAT;
            WSUI.Equipped--;
            WSUI.removeArseAbility(weapon.GetAbilityObject());
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
            transform.position = Vector3.Lerp(start, end, elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        lerpCoroutine = null;
    }

    public ScriptableAbility GetScriptableWeapon()
    {
        return weapon;
    }
}
