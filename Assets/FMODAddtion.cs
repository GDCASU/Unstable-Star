using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class FMODAddtion : MonoBehaviour
{
    [SerializeField] private StudioListener listener;
    [SerializeField] private TimelineAsset timelineAsset;
    [SerializeField] private PlayableDirector playableDirector;
    // Start is called before the first frame update
    void Start()
    {
        GameObject newSoundObject = SoundManager.instance.gameObject;
        listener.attenuationObject = newSoundObject;
        
        TrackAsset trackAsset = timelineAsset.GetRootTrack(1).GetChildTracks().Last();
        playableDirector.SetGenericBinding(trackAsset, newSoundObject);


    }
}
