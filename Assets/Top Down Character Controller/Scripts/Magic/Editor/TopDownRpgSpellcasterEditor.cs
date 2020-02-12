using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopDownRpgSpellcaster))]
[DisallowMultipleComponent]
public class TopDownRpgSpellcasterEditor : Editor {

    private Texture TopDownIcon;

    private TopDownRpgSpellcaster td_target;

    private void OnEnable() {
        td_target = (TopDownRpgSpellcaster)target;

        if (TopDownIcon == null) {
            TopDownIcon = Resources.Load("TopDownIcon") as Texture;
        }
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();

        GUIStyle boldCenteredLabel = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };

        GUIStyle simpleStyle = new GUIStyle(EditorStyles.foldout);
        simpleStyle.fontStyle = FontStyle.Bold;
        simpleStyle.fontSize = 11;
        simpleStyle.active.textColor = Color.black;
        simpleStyle.focused.textColor = Color.black;
        simpleStyle.onHover.textColor = Color.black;
        simpleStyle.normal.textColor = Color.black;
        simpleStyle.onNormal.textColor = Color.black;
        simpleStyle.onActive.textColor = Color.black;
        simpleStyle.onFocused.textColor = Color.black;

        EditorStyles.textField.wordWrap = true;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.LabelField(new GUIContent(TopDownIcon), boldCenteredLabel, GUILayout.ExpandWidth(true), GUILayout.Height(32));
        EditorGUILayout.LabelField("- TOP DOWN RPG -", boldCenteredLabel);
        EditorGUILayout.LabelField("Spellcaster Component", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is the main component that will be used for spellcasting.", MessageType.Info);

        EditorGUILayout.LabelField("Spell Playback Speed:");
        td_target.speed = EditorGUILayout.FloatField(string.Empty, td_target.speed);
        EditorGUILayout.HelpBox("This is the speed at which the spell will be played at.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();
    }
}
