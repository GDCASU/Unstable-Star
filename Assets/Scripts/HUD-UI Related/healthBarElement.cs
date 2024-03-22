using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBarElement : MonoBehaviour
{
    public Sprite fullSprite;
    public Sprite emptySprite;

    public void setEmpty()
    {
        gameObject.GetComponent<Image>().sprite = emptySprite;
    }

    public void setFull()
    {
        gameObject.GetComponent<Image>().sprite = fullSprite;
    }
}
