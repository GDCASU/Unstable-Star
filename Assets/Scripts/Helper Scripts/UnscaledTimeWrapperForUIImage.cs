using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnscaledTimeWrapperForUIImage : MonoBehaviour
{
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Image>().material;
    }

    private void Update()
    {
        mat.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
