using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Wave))]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        Wave wave = (Wave)target;

        EditorGUILayout.HelpBox("You don't need to press the button anymore(or set up the list). It will auto setup in Awake().", MessageType.Info);


        if (GUILayout.Button("Reset & Populate Enemy List"))
        {
            wave.ResetAndPopulateEnemyList();
        }
    }
}