using UnityEngine;
using System.Collections;
public class HoverAndLerp : MonoBehaviour
{
    public float hoverHeight = 1f;
    public float lerpSpeed = 5f;

    private bool isHovered = false;
    private Vector3 originalPosition;
    [SerializeField] private Material blackout;
    private Material initMAT;
    public bool selected = false;
    public bool unlocked = true;
    private WeaponSelectUI WSUI;
    void Start()
    {
        WSUI = GetComponentInParent<WeaponSelectUI>();
        originalPosition = transform.position;
        initMAT = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
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
            if(isHovered)
            {
                if(unlocked)
                {
                    if(Input.GetKeyDown(KeyCode.Mouse0))
                    {

                        WSUI.CheckWeaponEquipLoad(this);

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
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<Renderer>().material = initMAT;
            WSUI.Equipped--;
        }
        selected = !selected;

    }
    bool IsMouseOver()
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
}
