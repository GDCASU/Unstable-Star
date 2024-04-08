using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelButton : MonoBehaviour
{
    public int iterationDir;
    private levelText lvlText;

    // Start is called before the first frame update
    void Start()
    {
        lvlText = GameObject.Find("levelText").GetComponent<levelText>();
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!ScenesManager.instance.CheckLevel(ScenesManager.currentLevel + iterationDir))
                return;

            ScenesManager.currentLevel += iterationDir;
            lvlText.UpdateText(ScenesManager.currentLevel);
        }
    }
}
