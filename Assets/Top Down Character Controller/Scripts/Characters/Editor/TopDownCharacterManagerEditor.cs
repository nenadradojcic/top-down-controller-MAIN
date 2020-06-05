using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownCharacterManager))]
[DisallowMultipleComponent]
public class TopDownCharacterManagerEditor : Editor
{

    private TopDownCharacterManager td_target;

    private Texture TopDownIcon;

    public void OnEnable() {

        td_target = (TopDownCharacterManager)target;

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
        EditorGUILayout.LabelField("Player Characters Manager", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is main hub of playable characters. With this we keep track of currently acrive player character and all player characters in general. " +
            "We are also using this component to add or remove characters from players party.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUILayout.HelpBox("Here we place the character that we want to start the game with. If we do not place any character here, system will look for " +
            "first playable character and make him as a default playable character.", MessageType.None);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultCharacter"), true);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUI.BeginDisabledGroup(true); 
        EditorGUILayout.HelpBox("Here we can see what character player is currently controlling and who do we have in our party as active characters.", MessageType.None);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("controllingCharacter"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("activeCharacters"), true);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
