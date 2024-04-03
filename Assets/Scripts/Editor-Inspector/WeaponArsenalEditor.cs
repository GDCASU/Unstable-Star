using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary> A custom editor layout for reading the weapon loadout of the player </summary>
[CustomEditor(typeof(WeaponArsenal))]
public class WeaponArsenalEditor : Editor
{
    //Change this bool if you want to enable/disable the custom editor for the loadout object
    private bool EnableCustomEditor = true;

    //SerializedProperties
    SerializedProperty doDebugLog;
    SerializedProperty weaponArsenalStrings;
    SerializedProperty currWeaponIndex;
    SerializedProperty loadInspectorWeapons;

    //Serialized Weapons
    SerializedProperty primary;
    SerializedProperty secondary;

    // Foldouts
    private bool DebuggingGroup = true;
    private bool WeaponArsenalGroup = true;
    private bool AllScriptedWeapons = false;
    private bool warningMsgGroup = false;

    private void OnEnable()
    {
        doDebugLog = serializedObject.FindProperty("doDebugLog");
        weaponArsenalStrings = serializedObject.FindProperty("weaponArsenalStrings");
        currWeaponIndex = serializedObject.FindProperty("currWeaponIndex");
        loadInspectorWeapons = serializedObject.FindProperty("loadInspectorWeapons");
        primary = serializedObject.FindProperty("primary");
        secondary = serializedObject.FindProperty("secondary");
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

        // Warning message foldout
        warningMsgGroup = EditorGUILayout.BeginFoldoutHeaderGroup(warningMsgGroup, "Inspector Warning");
        if (warningMsgGroup)
        {

            // Create A box so its easier to find this script and disable it
            EditorGUILayout.ObjectField("Editor Script", MonoScript.FromScriptableObject(this), typeof(WeaponArsenalEditor), false);

            // Message in case Editor Script gets in the way of adding new entries
            string warningMessage = "\nNote: The Above script is modifying how this insepctor window looks.\n";
            warningMessage += "This means that if you ADD A VARIABLE for it to show on the inspector, ";
            warningMessage += "IT WONT SHOW unless you PROGRAM IT into the editor script yourself.\n";
            warningMessage += "Go to that script and change the \"EnableCustomEditor\" boolean to to disable the custom editor.\n";
            GUILayout.Label(warningMessage, EditorStyles.wordWrappedLabel);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Add the little shortcut box to the script
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((WeaponArsenal)target), typeof(WeaponArsenal), false);

        // Debug Settings
        DebuggingGroup = EditorGUILayout.BeginFoldoutHeaderGroup(DebuggingGroup, "Settings/Debugging");
        if (DebuggingGroup)
        {
            EditorGUILayout.PropertyField(doDebugLog);
            EditorGUILayout.PropertyField(loadInspectorWeapons);
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
        AllScriptedWeapons = EditorGUILayout.BeginFoldoutHeaderGroup(AllScriptedWeapons, "Presets");
        if (AllScriptedWeapons) 
        {
            EditorGUILayout.PropertyField(primary);
            EditorGUILayout.PropertyField(secondary);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }
}