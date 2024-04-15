using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.AssetImporters;
using UnityEngine;

public class UICred : MonoBehaviour
{
    // [SerializeField] private TMP_Text mytxt;
    [SerializeField]CredText soText;
    [SerializeField] TMP_Text mytext;
   // [SerializeField] private GameObject mypanel;
    float speed = 2f;
    Vector3 moving = new Vector3(0,1,0);
    private void Start()
    {
        mytext.text = soText.text;
    }
    void Update()
    {
        this.gameObject.transform.Translate(moving*speed*Time.deltaTime);
        if(this.gameObject.transform.position.y>0)
        {
            ScenesManager.instance.LoadScene(Scenes.MainMenu);
        }
    }
}
