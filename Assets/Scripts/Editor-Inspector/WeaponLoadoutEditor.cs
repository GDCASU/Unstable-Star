using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary> A custom editor layout for reading the weapon loadout of the player </summary>
[CustomEditor(typeof(WeaponArsenal))]
public class WeaponLoadoutEditor : Editor
{
    //Change this bool if you want to enable/disable the custom editor for the loadout object
    bool EnableCustomEditor = true;

    //SerializedProperties
    SerializedProperty maxWeaponCount;
    SerializedProperty doDebugLog;
    SerializedProperty weaponArsenalStrings;
    SerializedProperty currWeaponIndex;
    SerializedProperty loadDefaultArsenal;

    //Serialized Weapons
    SerializedProperty PlayerPistol;
    SerializedProperty PlayerBirdshot;
    SerializedProperty PlayerBuckshot;

    // Foldouts
    bool DebuggingGroup = true;
    bool WeaponArsenalGroup = true;
    bool AllScriptedWeapons = false;

    private void OnEnable()
    {
        maxWeaponCount = serializedObject.FindProperty("maxWeaponCount");
        doDebugLog = serializedObject.FindProperty("doDebugLog");
        weaponArsenalStrings = serializedObject.FindProperty("weaponArsenalStrings");
        currWeaponIndex = serializedObject.FindProperty("currWeaponIndex");
        loadDefaultArsenal = serializedObject.FindProperty("loadDefaultArsenal");
        PlayerPistol = serializedObject.FindProperty("PlayerPistol");
        PlayerBirdshot = serializedObject.FindProperty("PlayerBirdshot");
        PlayerBuckshot = serializedObject.FindProperty("PlayerBuckshot");
    }

    public override void OnInspectorGUI()
    {
        //On Inspector Update
        serializedObject.Update();

        if (!EnableCustomEditor)
        {
            base.OnInspectorGUI();
            return;
        }

        // Add the little shortcut box to the script
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((WeaponArsenal)target), typeof(WeaponArsenal), false);

        // Debug Settings
        DebuggingGroup = EditorGUILayout.BeginFoldoutHeaderGroup(DebuggingGroup, "Settings/Debugging");
        if (DebuggingGroup)
        {
            EditorGUILayout.PropertyField(maxWeaponCount);
            EditorGUILayout.PropertyField(doDebugLog);
            EditorGUILayout.PropertyField(loadDefaultArsenal);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Currently Stored Weapons
        WeaponArsenalGroup = EditorGUILayout.BeginFoldoutHeaderGroup(WeaponArsenalGroup, "Stored Weapons");
        if (WeaponArsenalGroup)
        {
            // If the list is empty, add a label to notify it, else show index and a list
            if (weaponArsenalStrings.arraySize <= 0)
            {
                EditorGUILayout.LabelField("No weapons stored yet");
            }
            else
            {
                EditorGUILayout.LabelField("Current Index: [" + currWeaponIndex.intValue + "]");
                EditorGUI.indentLevel++;
                // Display the string of each weapon within the list
                for (int i = 0; i < weaponArsenalStrings.arraySize; i++)
                {
                    SerializedProperty weaponProperty = weaponArsenalStrings.GetArrayElementAtIndex(i);
                    string weaponName = weaponProperty.stringValue;
                    EditorGUILayout.LabelField("[" + i + "] = " + weaponName);
                }
                EditorGUI.indentLevel--;
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // All the scripted weapons
        AllScriptedWeapons = EditorGUILayout.BeginFoldoutHeaderGroup(AllScriptedWeapons, "All Weapons");
        if (AllScriptedWeapons) 
        {
            EditorGUILayout.PropertyField(PlayerPistol);
            EditorGUILayout.PropertyField(PlayerBirdshot);
            EditorGUILayout.PropertyField(PlayerBuckshot);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }
}
