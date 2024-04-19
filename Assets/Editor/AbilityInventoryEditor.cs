using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary> A custom editor layout for reading the ability inventory of the player </summary>
[CustomEditor(typeof(AbilityInventory))]
public class AbilityInventoryEditor : Editor
{
    //Change this bool if you want to enable/disable the custom editor for the loadout object
    private bool EnableCustomEditor = true;

    //SerializedProperties
    SerializedProperty doDebugLog;
    SerializedProperty abilityInventoryStrings;
    SerializedProperty currAbilityIndex;
    SerializedProperty loadInspectorAbility;

    //Serialized Ability
    SerializedProperty inputAbility;

    // Foldouts
    private bool DebuggingGroup = true;
    private bool AbilityInventoryGroup = true;
    private bool InputAbility = false;
    private bool warningMsgGroup = false;

    private void OnEnable()
    {
        doDebugLog = serializedObject.FindProperty("doDebugLog");
        abilityInventoryStrings = serializedObject.FindProperty("abilityInventoryStrings");
        currAbilityIndex = serializedObject.FindProperty("currAbilityIndex");
        loadInspectorAbility = serializedObject.FindProperty("loadInspectorAbility");
        inputAbility = serializedObject.FindProperty("inputAbility");
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
            EditorGUILayout.ObjectField("Editor Script", MonoScript.FromScriptableObject(this), typeof(AbilityInventoryEditor), false);

            // Message in case Editor Script gets in the way of adding new entries
            string warningMessage = "\nNote: The Above script is modifying how this insepctor window looks.\n";
            warningMessage += "This means that if you ADD A VARIABLE for it to show on the inspector, ";
            warningMessage += "IT WONT SHOW unless you PROGRAM IT into the editor script yourself.\n";
            warningMessage += "Go to that script and change the \"EnableCustomEditor\" boolean to to disable the custom editor.\n";
            GUILayout.Label(warningMessage, EditorStyles.wordWrappedLabel);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Add the little shortcut box to the script
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((AbilityInventory)target), typeof(AbilityInventory), false);

        // Debug Settings
        DebuggingGroup = EditorGUILayout.BeginFoldoutHeaderGroup(DebuggingGroup, "Settings/Debugging");
        if (DebuggingGroup)
        {
            EditorGUILayout.PropertyField(doDebugLog);
            EditorGUILayout.PropertyField(loadInspectorAbility);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Currently Stored Abilities
        AbilityInventoryGroup = EditorGUILayout.BeginFoldoutHeaderGroup(AbilityInventoryGroup, "Stored Abilities");
        if (AbilityInventoryGroup)
        {
            // If the list is empty, add a label to notify it, else show index and a list
            if (abilityInventoryStrings.arraySize <= 0)
            {
                EditorGUILayout.LabelField("No abilities stored yet");
            }
            else
            {
                EditorGUILayout.LabelField("Current Index: [" + currAbilityIndex.intValue + "]");
                EditorGUI.indentLevel++;
                // Display the string of each ability within the list
                for (int i = 0; i < abilityInventoryStrings.arraySize; i++)
                {
                    SerializedProperty abilityProperty = abilityInventoryStrings.GetArrayElementAtIndex(i);
                    string abilityName = abilityProperty.stringValue;
                    EditorGUILayout.LabelField("[" + i + "] = " + abilityName);
                }
                EditorGUI.indentLevel--;
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Input abilities
        InputAbility = EditorGUILayout.BeginFoldoutHeaderGroup(InputAbility, "Input Ability");
        if (InputAbility)
        {
            EditorGUILayout.PropertyField(inputAbility);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }
}
