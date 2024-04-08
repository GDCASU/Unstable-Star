using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class levelText : MonoBehaviour
{
    public int maxLevel;

    private TMP_Text tmpText;

    // Start is called before the first frame update
    void Start()
    {
        tmpText = gameObject.GetComponent<TMP_Text>();
        UpdateText(ScenesManager.currentLevel);
    }

    public void UpdateText(int level)
    {
        if (level <= maxLevel && level > 0)
            tmpText.text = "Act " + level;
        else
            Debug.LogError("Error: Level does not exist.");
    }
}
