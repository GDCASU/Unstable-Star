using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOptions : MonoBehaviour
{
    [SerializeField] Sprite[] rebekahEmotions;
    [SerializeField] Sprite[] jaughnEmotions;
    [SerializeField] Sprite[] apolloEmotions;
    [SerializeField] Sprite[] ebbEmotions;
    [SerializeField] Sprite[] blankEmotions;
    [SerializeField] Sprite[] boss1Emotions;
    [SerializeField] Sprite[] boss2Emotions;
    [SerializeField] Sprite[] boss3Emotions;

    public Options CreateOptions(string optionType)
    {
        switch (optionType)
        {
            case "JAUGHN":
                Options jaughn = new Options
                {
                    name = "JAUGHN",
                    color = new Color32(80, 80, 80, 255),
                    emotions = jaughnEmotions,
                    isDialogue = true,
                };
                return jaughn;
            case "REBEKAH":
                Options rebekah = new Options
                {
                    name = "REBEKAH",
                    color = new Color32(255, 0, 0, 255),
                    emotions = rebekahEmotions,
                    isDialogue = true,
                };
                return rebekah;
            case "APOLLO":
                Options apollo = new Options
                {
                    name = "APOLLO",
                    color = new Color32(0, 75, 255, 255),
                    emotions = apolloEmotions,
                    isDialogue = true,
                };
                return apollo;
            case "EBB":
                Options ebb = new Options
                {
                    name = "EBB",
                    color = new Color32(180, 110, 0, 255),
                    emotions = ebbEmotions,
                    isDialogue = true,
                };
                return ebb;
            case "DESC: ":
                Options description = new Options
                {
                    name = "",
                    color = Color.black,
                    emotions = blankEmotions,
                    isDialogue = false,
                };
                return description;
            case "NOISE":
                Options noise = new Options
                {
                    name = "NOISE",
                    color = Color.clear,
                    emotions = blankEmotions,
                    isDialogue = false,
                };
                return noise;

            default:
                Options baseOptions = new Options
                {
                    name = optionType,
                    color = Color.black,
                    emotions = jaughnEmotions,
                    isDialogue = true,
                };
                return baseOptions;

        }
    }

    public Sprite GetEmotion(Options option, string emotion)
    {
        switch (emotion)
        {
            case "NORMAL":
                return option.emotions[0];
            case "HAPPY":
                return option.emotions[1];
            case "SAD":
                return option.emotions[2];
            case "ANGRY":
                return option.emotions[3];
            default:
                return option.emotions[0];
        }
    }
}

public struct Options
{
    public string name;
    public Color32 color;
    public Sprite[] emotions;
    public bool isDialogue;
};

