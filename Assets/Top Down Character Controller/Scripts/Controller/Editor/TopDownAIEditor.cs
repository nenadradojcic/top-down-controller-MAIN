using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownAI))]
[DisallowMultipleComponent]
public class TopDownAIEditor : Editor {

    private TopDownAI td_target;

    private Texture TopDownIcon;

    public void OnEnable() {

        td_target = (TopDownAI)target;

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
        EditorGUILayout.LabelField("AI Component", boldCenteredLabel);
        EditorGUILayout.HelpBox("There is nothing needed to set here. For all AI options refer to TopDownCharacterCard component. You can only use this component to set Dialog for this character.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (!td_target.aiDialog) {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
            if (GUILayout.Button("Setup Dialog")) {
                GameObject branch = new GameObject { name = "Main Dialog" };
                branch.transform.SetParent(td_target.transform);
                branch.transform.localPosition = Vector3.zero;
                branch.AddComponent<TopDownUIDialog>();
                Selection.activeObject = branch;
                td_target.aiDialog = branch.GetComponent<TopDownUIDialog>();
            }
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
