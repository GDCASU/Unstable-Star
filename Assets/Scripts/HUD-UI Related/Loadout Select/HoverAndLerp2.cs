using UnityEngine;
using System.Collections;
public class HoverAndLerp2 : MonoBehaviour
{
    public float hoverHeight = 1f;
    public float lerpSpeed = 5f;

    private bool isHovered = false;
    private Vector3 originalPosition;
    [SerializeField] private Material blackout;
    public Material transDisk;
    private Material initMAT;
    public bool selected = false;
    public bool unlocked = true;
    private AbilitySelectUI WSUI;
    private Material initDiskMat;
    [SerializeField] private ScriptableAbility weapon = null;
    void Start()
    {
        WSUI = GetComponentInParent<AbilitySelectUI>();
        originalPosition = transform.position;
        initMAT = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        if (!unlocked)
        {
            initDiskMat = gameObject.GetComponent<Material>();
            gameObject.GetComponent<Renderer>().material = transDisk;
        }
    }

    void Update()
    {
        if (IsMouseOver())
        {
            if (!isHovered)
            {
                isHovered = true;
                StartCoroutine(LerpObject(transform.position, new Vector3(transform.position.x, hoverHeight, transform.position.z)));
            }
            if (isHovered)
            {
                if (unlocked)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {

                        WSUI.CheckAbilityEquipLoad(this);

                    }
                }
            }
        }
        else
        {
            if (isHovered)
            {
                isHovered = false;
                StartCoroutine(LerpObject(transform.position, originalPosition));

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
    public bool IsMouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject;
    }

    IEnumerator LerpObject(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }
    }

    public ScriptableAbility GetScriptableWeapon()
    {
        return weapon;
    }
}
