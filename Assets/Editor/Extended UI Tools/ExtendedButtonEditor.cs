using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Editor class for the Extended Button
/// </summary>
[CustomEditor(typeof(ExtendedButton))]
public class ExtendedButtonEditor : ButtonEditor
{
    #region Serialized Properties

    SerializedProperty onHighlight;
    SerializedProperty onUnhighlight;

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
        onHighlight = serializedObject.FindProperty("onHighlight");
        onUnhighlight = serializedObject.FindProperty("onUnhighlight"); 
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //On Inspector Update
        serializedObject.Update();

        // Add new fields
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Highlight Events", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(onHighlight);
        EditorGUILayout.PropertyField(onUnhighlight);

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }
}
