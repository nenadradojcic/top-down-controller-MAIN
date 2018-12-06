using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(TopDownCharacter))]
[DisallowMultipleComponent]
public class TopDownCharacterEditor : Editor {

    private Texture TopDownIcon;

    private TopDownCharacter td_target;

    private void OnEnable() {
        td_target = (TopDownCharacter)target;

        if (TopDownIcon == null) {
            TopDownIcon = Resources.Load("TopDownIcon") as Texture;
        }
    }

    public override void OnInspectorGUI() {

        GUIStyle boldCenteredLabel = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };

        GUIStyle simpleTitleLable = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleLeft };

        EditorStyles.textField.wordWrap = true;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.LabelField(new GUIContent(TopDownIcon), boldCenteredLabel, GUILayout.ExpandWidth(true), GUILayout.Height(32));
        EditorGUILayout.LabelField("- TOP DOWN RPG -", boldCenteredLabel);
        EditorGUILayout.LabelField("Character Card", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is character base. Here you will set everything about your character that the game will use in gameplay.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUILayout.LabelField("Character Portrait:", simpleTitleLable);
        td_target.icon = (Sprite)EditorGUILayout.ObjectField(string.Empty, td_target.icon, typeof(Sprite), true);
        EditorGUILayout.HelpBox("This is the portrait that will represent your character in game.", MessageType.Info);

        EditorGUILayout.LabelField("Character Name:", simpleTitleLable);
        td_target.name = EditorGUILayout.TextField(string.Empty, td_target.name);
        EditorGUILayout.HelpBox("This is the name of the character.", MessageType.Info);

        EditorGUILayout.LabelField("Character Health:", simpleTitleLable);
        td_target.health = EditorGUILayout.FloatField(string.Empty, td_target.health);
        EditorGUILayout.HelpBox("This is the starting health value of this character.", MessageType.Info);

        EditorGUILayout.LabelField("Character Energy:", simpleTitleLable);
        td_target.energy = EditorGUILayout.FloatField(string.Empty, td_target.energy);
        EditorGUILayout.HelpBox("This is the starting energy value of this character.", MessageType.Info);

        EditorGUILayout.LabelField("Character Voice Set:", simpleTitleLable);
        td_target.voiceSet = (TopDownVoiceSet)EditorGUILayout.ObjectField(string.Empty, td_target.voiceSet, typeof(TopDownVoiceSet), true);
        EditorGUILayout.HelpBox("If this is set then this character will play setted voices in the game.", MessageType.Info);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
