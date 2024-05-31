using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startbuttonenabled : MonoBehaviour
{
    public GameObject button;

    public TextMeshProUGUI textObject;
    public float fadeDuration = 2f;
    public float elapsedTime;
    ScenesManager sm;

    void Start()
    {
       sm = ScenesManager.instance;
        textObject.alpha = 0f;
    }

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
        if (!WeaponArsenal.instance.IsWeaponArsenalEmpty())
        {
            //SCENE MANAGER CHANGE SCENE
            print("SWITCH SCENE");
            sm.LoadNextScene();
        }
        else
        {
            StartCoroutine(FadeOutText());
        }
    }
}
