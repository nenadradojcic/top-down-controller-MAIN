using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownRpgQuest))]
[CanEditMultipleObjects]
public class TopDownRpgQuestEditor : Editor
{

    private Texture TopDownIcon;

    private TopDownRpgQuest td_target;

    private void OnEnable() {
        td_target = (TopDownRpgQuest)target;

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
        EditorGUILayout.LabelField("Quest", boldCenteredLabel);
        EditorGUILayout.HelpBox("This component holds all thing needed for one functional quest.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("questName"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("questCategory"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("questType"), true);

        if (td_target.questType == QuestType.KillTargets) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("questTargets"), true);
        }
        else if (td_target.questType == QuestType.TalkToNpc || td_target.questType == QuestType.GoToLocation) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("questTarget"), true);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("questDescription"), true);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("questState"), true);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
