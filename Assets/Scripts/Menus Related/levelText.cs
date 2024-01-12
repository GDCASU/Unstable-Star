using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class levelText : MonoBehaviour
{
    public List<string> levelNames;
    public int maxLevel;
    public int newCurrentLevel;
    private int currentLevel;
    private TMP_Text tmpText;

    // Start is called before the first frame update
    void Start()
    {
        tmpText = gameObject.GetComponent<TMP_Text>();
        currentLevel = -1;
        newCurrentLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (newCurrentLevel != currentLevel)
        {
            if (newCurrentLevel >= 0 && newCurrentLevel <= maxLevel && newCurrentLevel < levelNames.Count)
            {
                currentLevel = newCurrentLevel;
                tmpText.text = levelNames[currentLevel];
            }
            else
                newCurrentLevel = currentLevel;
        }
    }
}
