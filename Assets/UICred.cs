using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UI;

public class UICred : MonoBehaviour
{
    // [SerializeField] private TMP_Text mytxt;
    [SerializeField]CredText soText;
    [SerializeField] TMP_Text mytext;
    [SerializeField] Button myButton;
   // [SerializeField] private GameObject mypanel;
    float speed = 2f;
    Vector3 moving = new Vector3(0,1,0);
    private void Start()
    {
        mytext.text = soText.text;
        Debug.Log("Mytext: " + mytext.text);
        Debug.Log("SOtext: " + soText.text);
        if(myButton!=null)
        {
            Debug.Log("Event listener Added");
            myButton.onClick.AddListener(HandleButtonClick);
        }
    }
    void Update()
    {
        this.gameObject.transform.Translate(moving*speed*Time.deltaTime);
        if(this.gameObject.transform.position.y>0)
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
