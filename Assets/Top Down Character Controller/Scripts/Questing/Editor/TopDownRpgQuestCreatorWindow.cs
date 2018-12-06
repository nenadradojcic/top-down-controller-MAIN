using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TopDownRpgQuestCreatorWindow : EditorWindow {

    private static Texture TopDownIcon;

    private string questName;
    private QuestType questType;
    private GameObject questTarget;
    private List<GameObject> questTargets;
    private string questFinishDialog;

    [MenuItem("Top Down RPG/Setup/New Quest", false, 2)]
    static void Init() {
        TopDownRpgQuestCreatorWindow window = (TopDownRpgQuestCreatorWindow)EditorWindow.GetWindow(typeof(TopDownRpgQuestCreatorWindow));
        window.Show();

        if (TopDownIcon == null) {
            TopDownIcon = Resources.Load("TopDownIcon") as Texture;
        }
    }

    void OnGUI() {

        GUIStyle boldCenteredLabel = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };

        EditorStyles.textField.wordWrap = true;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.LabelField(new GUIContent(TopDownIcon), boldCenteredLabel, GUILayout.ExpandWidth(true), GUILayout.Height(32));
        EditorGUILayout.LabelField("- TOP DOWN RPG -", boldCenteredLabel);
        EditorGUILayout.LabelField("Setup New Quest", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is used to setup new quest.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        questName = EditorGUILayout.TextField("Quest Title:", questName);
        EditorGUILayout.HelpBox("This is the name of the Quest that will show up in the questlog.", MessageType.Info);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
