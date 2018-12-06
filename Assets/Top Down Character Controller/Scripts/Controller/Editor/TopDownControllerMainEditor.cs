using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopDownControllerMain))]
[DisallowMultipleComponent]
public class TopDownControllerMainEditor : Editor {

    private TopDownControllerMain td_target;

    private Texture TopDownIcon;

    public void OnEnable() {

        td_target = (TopDownControllerMain)target;

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
        EditorGUILayout.LabelField("Top Down Character Controller", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is the main component of Top Down Rpg. With this component we are making our character move.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        td_target.tdcm_movingSpeed = EditorGUILayout.FloatField("Moving Speed:", td_target.tdcm_movingSpeed);
        td_target.tdcm_turningSpeed = EditorGUILayout.FloatField("Moving Speed:", td_target.tdcm_turningSpeed);
        td_target.tdcm_animPlaySpeed = EditorGUILayout.FloatField("Animation Playback Speed:", td_target.tdcm_animPlaySpeed);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
