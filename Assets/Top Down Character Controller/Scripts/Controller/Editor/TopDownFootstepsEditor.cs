using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopDownFootsteps))]
[CanEditMultipleObjects]
public class TopDownFootstepsEditor : Editor {

    private Texture TopDownIcon;

    public TopDownFootsteps td_target;

    public void OnEnable() {
        td_target = (TopDownFootsteps)target;

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
        EditorGUILayout.LabelField("Footsteps Manager", boldCenteredLabel);
        EditorGUILayout.HelpBox("This component we place on characters that we want to play footsteps sound when moving. It will be played on timed interval by calling TopDownAudioManager component and using footstep file set in it.", MessageType.Info);
        serializedObject.FindProperty("stepRunInterval").floatValue = EditorGUILayout.FloatField("Footstep Play Interval:", td_target.stepRunInterval);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
