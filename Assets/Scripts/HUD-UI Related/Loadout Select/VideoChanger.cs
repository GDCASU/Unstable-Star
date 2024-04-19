using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoChanger : MonoBehaviour
{
    // Start is called before the first frame update

    public WeaponSelectUI WSUI;
    public GameObject VPobj;
    private GameObject gun;
    private VideoPlayer VP;
    public Material VPmat;
    public Material invis;

    void Start()
    {
        VP = VPobj.GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        gun = WSUI.scanforhover();
            if (gun != null )
        {
            string temp = gun.name;
            VPobj.GetComponent<RawImage>().enabled = true;

            VideoClip tempclip = Resources.Load<VideoClip>(temp);
            VP.clip = tempclip;

        }
        else VPobj.GetComponent<RawImage>().enabled = false;
        
    }
}
