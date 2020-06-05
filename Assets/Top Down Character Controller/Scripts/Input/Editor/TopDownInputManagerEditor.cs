using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownInputManager))]
[DisallowMultipleComponent]
public class TopDownInputManagerEditor : Editor {

    private TopDownInputManager td_target;

    private Texture TopDownIcon;

    public void OnEnable() {

        td_target = (TopDownInputManager)target;

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
        EditorGUILayout.LabelField("Input Manager", boldCenteredLabel);
        EditorGUILayout.HelpBox("We use this component to set all keys that are used in gameplay.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUILayout.HelpBox("Labels set here point to Unity's built-in Input Manager Axes names.", MessageType.None);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontalAxisName"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalAxisName"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mouseXName"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mouseYName"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mouseScrollwheelName"), true);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUILayout.HelpBox("Using keycodes we are setting various commands that will impact gameplay.", MessageType.None);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pauseGameKey"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("interactKey"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("inventoryKeyCode"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("questLogKeyCode"), true);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUILayout.HelpBox("This keys will change our camera from following to free and will enable rotation of camera.", MessageType.None);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("changeCamera"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateCamera"), true);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.HelpBox("This is used to check what key is currently pressed for easier reference.", MessageType.None);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentKeyPressed"), true);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
