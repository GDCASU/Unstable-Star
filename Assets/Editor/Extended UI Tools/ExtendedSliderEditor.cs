using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

/// <summary>
/// Editor class for the Extended Slider
/// </summary>
[CustomEditor(typeof(ExtendedSlider))]
public class ExtendedSliderEditor : SliderEditor
{
    #region Serialized Properties + Foldouts

    // Properties
    SerializedProperty onHighlight;
    SerializedProperty onUnhighlight;
    SerializedProperty onValueChangeHold;
    SerializedProperty onValueChangeRelease;

    // Foldouts
    private bool highlightFoldout;
    private bool onValueChangeExtraFoldout;

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
        onHighlight = serializedObject.FindProperty("onHighlight");
        onUnhighlight = serializedObject.FindProperty("onUnhighlight");
        onValueChangeHold = serializedObject.FindProperty("onValueChangeHold");
        onValueChangeRelease = serializedObject.FindProperty("onValueChangeRelease");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //On Inspector Update
        serializedObject.Update();

        // Add new fields
        EditorGUILayout.Space();

        // Highlight
        highlightFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(highlightFoldout, "Highlight Events");
        if (highlightFoldout)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(onHighlight);
            EditorGUILayout.PropertyField(onUnhighlight);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        // On value change extra
        EditorGUILayout.LabelField("Note: These only work with mouse for now", EditorStyles.wordWrappedLabel);
        onValueChangeExtraFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(onValueChangeExtraFoldout, "On Value Change Extra Events");
        if (onValueChangeExtraFoldout)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("On Value Change Hold only fires once until the player releases the slider, then the Release Event will fire", EditorStyles.wordWrappedLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(onValueChangeHold);
            EditorGUILayout.PropertyField(onValueChangeRelease);
            
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }
}
