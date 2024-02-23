using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tooltipText : MonoBehaviour // changes text
{
    public string newText;
    private string targetText;
    private TMP_Text tmpText;
    Coroutine crt;

    // Start is called before the first frame update
    void Start()
    {
        tmpText = gameObject.GetComponent<TMP_Text>();
        targetText = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (targetText != newText)
        {
            tmpText.text = "";
            targetText = newText;
            if (crt != null)
                StopCoroutine(crt);
            crt = StartCoroutine(setTooltipText(targetText));
        }
        //setTooltipText(targetText);
        //Debug.Log(tmpText.text + " != " + targetText + " : " + tmpText.text != targetText);

        
    }

    IEnumerator setTooltipText(string str)
    {
        while (tmpText.text != targetText)
        {
            tmpText.text = str.Substring(0, tmpText.text.Length + 1);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
