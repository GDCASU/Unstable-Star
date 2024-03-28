using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// This script's purpose is to improve the Player script appereance on the Editor/Inspector
// Since it started to contain too many variables that made it difficult to work with
// NOTE: Any new variable added to the player has to be added here for it to show on the
// inspector and editor window

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    //Change this bool if you want to enable/disable the custom editor for the player
    bool EnableCustomEditor = true;

    #region SerializedProperties

    //Stats
    SerializedProperty health;
    SerializedProperty shield;
    SerializedProperty shieldPerSecond;
    SerializedProperty isInvulnerable;
    SerializedProperty isShootingLocked;
    SerializedProperty isAbilityLocked;
    SerializedProperty timeLeftInvulnerable;
    SerializedProperty isShieldBroken;

    //Settings
    SerializedProperty MAX_HEALTH;
    SerializedProperty MAX_SHIELD;
    SerializedProperty dmgInvulnTime;
    SerializedProperty shieldRegenDelayTime;

    //Collisions
    SerializedProperty collisionDamage;
    SerializedProperty isIgnoringCollisions;

    //Debugging
    SerializedProperty IsDebugLogging;
    SerializedProperty DamageTest;
    SerializedProperty HealTest;
    SerializedProperty GainShieldTest;
    SerializedProperty DeathTest;
    SerializedProperty InvulnerabilityTest;
    SerializedProperty shieldFloat;

    #endregion

    //Foldouts, true or false determine if the foldouts are open or closed by default
    bool StatsGroup = true;
    bool SettingsGroup = false;
    bool CollisionsGroup = false;
    bool DebuggingGroup = false;
    bool warningMsgGroup = false;

    private void OnEnable()
    {
        //Stats
        health = serializedObject.FindProperty("health");
        shield = serializedObject.FindProperty("shield");
        shieldPerSecond = serializedObject.FindProperty("shieldPerSecond");
        isInvulnerable = serializedObject.FindProperty("isInvulnerable");
        isShootingLocked = serializedObject.FindProperty("isShootingLocked");
        isAbilityLocked = serializedObject.FindProperty("isAbilityLocked");
        timeLeftInvulnerable = serializedObject.FindProperty("timeLeftInvulnerable");
        isShieldBroken = serializedObject.FindProperty("isShieldBroken");

        //Settings
        MAX_HEALTH = serializedObject.FindProperty("MAX_HEALTH");
        MAX_SHIELD = serializedObject.FindProperty("MAX_SHIELD");
        dmgInvulnTime = serializedObject.FindProperty("dmgInvulnTime");
        shieldRegenDelayTime = serializedObject.FindProperty("shieldRegenDelayTime");

        //Collisions
        collisionDamage = serializedObject.FindProperty("collisionDamage");
        isIgnoringCollisions = serializedObject.FindProperty("isIgnoringCollisions");

        //Debugging
        IsDebugLogging = serializedObject.FindProperty("IsDebugLogging");
        DamageTest = serializedObject.FindProperty("DamageTest");
        HealTest = serializedObject.FindProperty("HealTest");
        GainShieldTest = serializedObject.FindProperty("GainShieldTest");
        DeathTest = serializedObject.FindProperty("DeathTest");
        InvulnerabilityTest = serializedObject.FindProperty("InvulnerabilityTest");
        shieldFloat = serializedObject.FindProperty("shieldFloat");
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
            EditorGUILayout.ObjectField("Editor Script", MonoScript.FromScriptableObject(this), typeof(PlayerEditor), false);

            // Message in case Editor Script gets in the way of adding new entries
            string warningMessage = "\nNote: The Above script is modifying how this insepctor window looks.\n";
            warningMessage += "This means that if you ADD A VARIABLE for it to show on the inspector, ";
            warningMessage += "IT WONT SHOW unless you PROGRAM IT into the editor script yourself.\n";
            warningMessage += "Go to that script and change the \"EnableCustomEditor\" boolean to disable the custom editor.\n";
            GUILayout.Label(warningMessage, EditorStyles.wordWrappedLabel);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Add the little shortcut box to the scripts
        EditorGUILayout.ObjectField("Player Script", MonoScript.FromMonoBehaviour((Player)target), typeof(Player), false);

        //Stats Foldout
        StatsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(StatsGroup, "Statistics");
        if (StatsGroup)
        {
            EditorGUILayout.PropertyField(health);
            EditorGUILayout.PropertyField(shield);
            EditorGUILayout.PropertyField(shieldPerSecond);
            EditorGUILayout.PropertyField(isShootingLocked);
            EditorGUILayout.PropertyField(isAbilityLocked); 
            EditorGUILayout.PropertyField(isShieldBroken);
            EditorGUILayout.PropertyField(isInvulnerable);
            EditorGUILayout.PropertyField(timeLeftInvulnerable);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //Settings
        SettingsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(SettingsGroup, "Settings");
        if (SettingsGroup)
        {
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
            EditorGUILayout.PropertyField(isIgnoringCollisions); 
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //Debugging foldout
        DebuggingGroup = EditorGUILayout.BeginFoldoutHeaderGroup(DebuggingGroup, "Debugging");
        if (DebuggingGroup)
        {
            EditorGUILayout.PropertyField(IsDebugLogging);
            EditorGUILayout.PropertyField(DamageTest);
            EditorGUILayout.PropertyField(HealTest);
            EditorGUILayout.PropertyField(GainShieldTest);
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
