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

        if (GUILayout.Button("Reset & Populate Enemy List"))
        {
            wave.ResetAndPopulateEnemyList();
        }
    }
}