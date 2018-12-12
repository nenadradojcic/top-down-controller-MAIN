using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownRpgQuestHub))]
[DisallowMultipleComponent]
public class TopDownRpgQuestHubEditor : Editor {

    private TopDownRpgQuestHub td_target;

    private Texture TopDownIcon;

    public void OnEnable() {

        td_target = (TopDownRpgQuestHub)target;

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
        EditorGUILayout.LabelField("Quest Hub", boldCenteredLabel);
        EditorGUILayout.HelpBox("This component is used to track all quests present in the scene, and their current status (undiscovered, started, finished, failed). " +
            "We will use this component to show quests in the UI Quest Log.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUILayout.LabelField("- Quests Info -", boldCenteredLabel);

        EditorGUILayout.LabelField("Currently Active Quest For Tracking:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        if (td_target.activeQuest != null) {
            EditorGUILayout.LabelField(td_target.activeQuest.questName);
        }
        else {
            EditorGUILayout.LabelField("NO QUEST");
        }
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startedQuests"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("finishedQuests"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("failedQuests"), true);
        EditorGUILayout.Space();
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
