using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SwitchAbilityWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform WeaponSelect;
    public Transform AbilitySelect;
    public float lerpspeed;
    public bool focusedOnAbilities;
    public bool moving;
    [SerializeField] private GameObject ButtontoAbilities;
    [SerializeField] private GameObject ButtontoWeapons;
    private float lerpTime;

    void Update()
    {
        if (moving)
        {
            if (focusedOnAbilities)
            {
                LerpBetweenPoints(WeaponSelect, AbilitySelect, ButtontoWeapons);
            }
            else
            {
                LerpBetweenPoints(AbilitySelect, WeaponSelect,ButtontoAbilities);
            }
        }

    }

    public void moveToAbility()
    {
        focusedOnAbilities = true;
        moving = true;
    }
    public void moveToWeapon()
    {
        focusedOnAbilities = false;
        moving = true;
    }
    void LerpBetweenPoints(Transform startPoint, Transform endPoint, GameObject button)
    {
        lerpTime += Time.deltaTime * lerpspeed;

        lerpTime = Mathf.Clamp01(lerpTime);

        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, lerpTime);
        if (transform.position == endPoint.position)
        {
            moving = false;
            button.SetActive(true);
        }

        if (lerpTime >= 1f)
        {
            lerpTime = 0f;
        }
    }
}
