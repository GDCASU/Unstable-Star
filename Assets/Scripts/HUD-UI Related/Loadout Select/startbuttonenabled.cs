using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class startbuttonenabled : MonoBehaviour
{
    // Start is called before the first frame update

    private WeaponArsenal arsenal;
    public GameObject button;


    public TextMeshProUGUI textObject;
    public float fadeDuration = 2f;
    public float elapsedTime;
    void Start()
    {
        textObject.alpha = 0f;

        arsenal = GameObject.Find("Weapon Arsenal").GetComponent<WeaponArsenal>();
    }



    // Update is called once per frame



    private IEnumerator FadeOutText()
    {
        elapsedTime = 0f;
        float startAlpha = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeDuration);
            textObject.alpha = newAlpha;
            yield return null;
        }
        // Ensure the text is completely invisible
        textObject.alpha = 0f;
    }


    public void selectbutton()
    {
        if (!arsenal.IsWeaponArsenalEmpty())
        {
            //SCENE MANAGER CHANGE SCENE
            print("SWITCH SCENE");
        }
        else
        {
            StartCoroutine(FadeOutText());
        }
    }
}
