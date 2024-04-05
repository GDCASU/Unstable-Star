using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbilityElement : MonoBehaviour
{
    public Sprite fullSprite;
    public Sprite fillingSprite;
    public Sprite emptySprite;

    public bool empty;
    public float chargeTime;

    private RectTransform maskRectTransform;
    private float maxHeight;
    private float value;
    private Image imageComponent;

    // Start is called before the first frame update
    void Start()
    {
        empty = false;
        maskRectTransform = transform.Find("mask").GetComponent<RectTransform>();
        maxHeight = transform.Find("uncharged").GetComponent<RectTransform>().sizeDelta.y;
        imageComponent = transform.Find("mask/charged").GetComponent<Image>();
        transform.Find("uncharged").GetComponent<Image>().sprite = emptySprite;
        value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (empty)
        {
            value = 0f;
            empty = false;
        }

        setFillValue(value);
        value += Time.deltaTime / chargeTime;
    }

    void setFillValue(float value)
    {
        maskRectTransform.sizeDelta = new Vector2(maskRectTransform.sizeDelta.x, Mathf.Min(maxHeight, maxHeight*value));
        if (value < 1)
            imageComponent.sprite = fillingSprite;
        else
            imageComponent.sprite = fullSprite;

    }

    void setEmpty()
    {
        maskRectTransform.sizeDelta = new Vector2(maskRectTransform.sizeDelta.x, 0);
    }
}
