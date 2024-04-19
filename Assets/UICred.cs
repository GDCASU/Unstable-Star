using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UI;

public class UICred : MonoBehaviour
{
    // [SerializeField] private TMP_Text mytxt;
    [SerializeField] List<CredText> creditsTextList = new List<CredText>();
    [SerializeField] TMP_Text txt_Target;
    [SerializeField] Button myButton;
   // [SerializeField] private GameObject mypanel;
    [SerializeField] float speed = 2f;
    Vector3 moving = new Vector3(0,1,0);
    private void Start()
    {
        txt_Target.text = "";
        foreach (var creditsText in creditsTextList)
        {
            txt_Target.text += creditsText.title + "\n\n" +
                creditsText.text.Replace(',', '\n') + "\n\n\n";
        }
        if(myButton!=null)
        {
            Debug.Log("Event listener Added");
            myButton.onClick.AddListener(HandleButtonClick);
        }
    }
    void Update()
    {
        txt_Target.rectTransform.Translate(moving*speed*Time.deltaTime);
        if (txt_Target.transform.localPosition.y > txt_Target.rectTransform.rect.height)
        {
            ScenesManager.instance.LoadScene(Scenes.MainMenu);
        }
    }
    public void HandleButtonClick()
    {
        Debug.Log("wordsss");
        ScenesManager.instance.LoadScene(Scenes.MainMenu);
    }
    
}
