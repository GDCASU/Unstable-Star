// This script's purpose is to improve the Player script appereance on the Editor/Inspector
// Since it started to contain too many variables that made it difficult to work with
// NOTE: Any new variable added to the player has to be added here for it to show on the
// inspector and editor window

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    //Change this bool if you want to enable/disable the custom editor for the player
    bool EnableCustomEditor = true;

    #region SerializedProperties

    //Stats
    SerializedProperty health;
    SerializedProperty shield;
    SerializedProperty shieldRegenPercent;
    SerializedProperty isInvulnerable;
    SerializedProperty timeLeftInvulnerable;
    SerializedProperty isShieldBroken;

    //Settings
    SerializedProperty ModelObject;
    SerializedProperty WeaponAnchor;
    SerializedProperty MAX_HEALTH;
    SerializedProperty MAX_SHIELD;
    SerializedProperty dmgInvulnTime;
    SerializedProperty shieldRegenDelayTime;

    //Collisions
    SerializedProperty collisionDamage;

    //Debugging
    SerializedProperty IsDebugLogging;
    SerializedProperty DamageTest;
    SerializedProperty HealTest;
    SerializedProperty DeathTest;
    SerializedProperty InvulnerabilityTest;
    SerializedProperty shieldFloat;

    #endregion

    //Foldouts, true or false determine if the foldouts are open or closed by default
    bool StatsGroup = true;
    bool SettingsGroup = false;
    bool CollisionsGroup = false;
    bool DebuggingGroup = false;

    private void OnEnable()
    {
        //Stats
        health = serializedObject.FindProperty("health");
        shield = serializedObject.FindProperty("shield");
        shieldRegenPercent = serializedObject.FindProperty("shieldRegenPercent");
        isInvulnerable = serializedObject.FindProperty("isInvulnerable");
        timeLeftInvulnerable = serializedObject.FindProperty("timeLeftInvulnerable");
        isShieldBroken = serializedObject.FindProperty("isShieldBroken");

        //Settings
        ModelObject = serializedObject.FindProperty("ModelObject");
        WeaponAnchor = serializedObject.FindProperty("WeaponAnchor");
        MAX_HEALTH = serializedObject.FindProperty("MAX_HEALTH");
        MAX_SHIELD = serializedObject.FindProperty("MAX_SHIELD");
        dmgInvulnTime = serializedObject.FindProperty("dmgInvulnTime");
        shieldRegenDelayTime = serializedObject.FindProperty("shieldRegenDelayTime");

        //Collisions
        collisionDamage = serializedObject.FindProperty("collisionDamage");

        //Debugging
        IsDebugLogging = serializedObject.FindProperty("IsDebugLogging");
        DamageTest = serializedObject.FindProperty("DamageTest");
        HealTest = serializedObject.FindProperty("HealTest");
        DeathTest = serializedObject.FindProperty("DeathTest");
        InvulnerabilityTest = serializedObject.FindProperty("InvulnerabilityTest");
        shieldFloat = serializedObject.FindProperty("shieldFloat");
    }

    public override void OnInspectorGUI()
    {
        if (!EnableCustomEditor)
        {
            base.OnInspectorGUI();
            return;
        }
        
        //On Inspector Update
        serializedObject.Update();

        //Stats Foldout
        StatsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(StatsGroup, "Statistics");
        if (StatsGroup)
        {
            EditorGUILayout.PropertyField(health);
            EditorGUILayout.PropertyField(shield);
            EditorGUILayout.PropertyField(shieldRegenPercent);
            EditorGUILayout.PropertyField(isShieldBroken);
            EditorGUILayout.PropertyField(isInvulnerable);
            EditorGUILayout.PropertyField(timeLeftInvulnerable);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //Settings
        SettingsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(SettingsGroup, "Settings");
        if (SettingsGroup)
        {
            EditorGUILayout.PropertyField(ModelObject);
            EditorGUILayout.PropertyField(WeaponAnchor);
            EditorGUILayout.PropertyField(MAX_HEALTH);
            EditorGUILayout.PropertyField(MAX_SHIELD);
            EditorGUILayout.PropertyField(dmgInvulnTime);
            EditorGUILayout.PropertyField(shieldRegenDelayTime);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //Collisions foldout
        CollisionsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(CollisionsGroup, "Collisions");
        if (CollisionsGroup)
        {
            EditorGUILayout.PropertyField(collisionDamage);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //Debugging foldout
        DebuggingGroup = EditorGUILayout.BeginFoldoutHeaderGroup(DebuggingGroup, "Debugging");
        if (DebuggingGroup)
        {
            EditorGUILayout.PropertyField(IsDebugLogging);
            EditorGUILayout.PropertyField(DamageTest);
            EditorGUILayout.PropertyField(HealTest);
            EditorGUILayout.PropertyField(DeathTest);
            EditorGUILayout.PropertyField(InvulnerabilityTest); 
            EditorGUILayout.LabelField("Note: I suggest not modifying these readouts");
            EditorGUILayout.PropertyField(shieldFloat);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //Apply Changes
        serializedObject.ApplyModifiedProperties();
    }
}
