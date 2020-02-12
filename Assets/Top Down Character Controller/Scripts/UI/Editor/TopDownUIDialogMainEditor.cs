using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownUIDialogMain))]
[DisallowMultipleComponent]
public class TopDownUIDialogMainEditor : Editor {

    private TopDownUIDialogMain td_target;

    private Texture TopDownIcon;

    public void OnEnable() {

        td_target = (TopDownUIDialogMain)target;

        if (TopDownIcon == null) {
            TopDownIcon = Resources.Load("TopDownIcon") as Texture;
        }
    }

    public override void OnInspectorGUI() {

        GUIStyle boldCenteredLabel = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };

        EditorStyles.textField.wordWrap = true;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.LabelField(new GUIContent(TopDownIcon), boldCenteredLabel, GUILayout.ExpandWidth(true), GUILayout.Height(32));
        EditorGUILayout.LabelField("- TOP DOWN RPG -", boldCenteredLabel);
        EditorGUILayout.LabelField("Main Dialog Component", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is main Dialog System component.", MessageType.Info);


        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.LabelField("Use key to select dialog option?");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("useKeyNumShortcuts"), true);

        if (serializedObject.FindProperty("useKeyNumShortcuts").boolValue == true) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialog1Key"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialog2Key"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialog3Key"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialog4Key"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialog5Key"), true);
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
