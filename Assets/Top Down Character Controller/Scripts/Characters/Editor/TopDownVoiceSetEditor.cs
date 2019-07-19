using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownVoiceSet))]
[DisallowMultipleComponent]
public class TopDownVoiceSetEditor : Editor {

    private Texture TopDownIcon;

    private TopDownVoiceSet td_target;

    private void OnEnable() {
        td_target = (TopDownVoiceSet)target;

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
        EditorGUILayout.LabelField("Voice Set", boldCenteredLabel);
        EditorGUILayout.HelpBox("Voice sets are used to give characters life by assigning them voice. Here you will set audio clips that the character where this is assigned will play in certain ocassions.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.HelpBox("Here you do not assign audio clips. Audio clips are assigned to TopDownAudioClipPlayer component in game object containing Audio Source component. Then that game object is saved as prefab and assigned here.", MessageType.Warning);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.LabelField("Voice Set Name:", simpleTitleLable);
        td_target.setName = EditorGUILayout.TextField(string.Empty, td_target.setName);
        EditorGUILayout.HelpBox("This is the name of this voice set.", MessageType.Info);

        EditorGUILayout.LabelField("Idle Audio:", simpleTitleLable);
        td_target.idleVoice = (GameObject)EditorGUILayout.ObjectField(string.Empty, td_target.idleVoice, typeof(GameObject), true);
        EditorGUILayout.HelpBox("This will be played randomly while character is idling.", MessageType.Info);

        EditorGUILayout.LabelField("Detect Audio:", simpleTitleLable);
        td_target.detectVoice = (GameObject)EditorGUILayout.ObjectField(string.Empty, td_target.detectVoice, typeof(GameObject), true);
        EditorGUILayout.HelpBox("This is used only for AI to react when they detect player character.", MessageType.Info);

        EditorGUILayout.LabelField("Get Hit Audio:", simpleTitleLable);
        td_target.getHitVoice = (GameObject)EditorGUILayout.ObjectField(string.Empty, td_target.getHitVoice, typeof(GameObject), true);
        EditorGUILayout.HelpBox("This will be played when character is hit/hurt in combat.", MessageType.Info);

        EditorGUILayout.LabelField("Death Audio:", simpleTitleLable);
        td_target.deathVoice = (GameObject)EditorGUILayout.ObjectField(string.Empty, td_target.deathVoice, typeof(GameObject), true);
        EditorGUILayout.HelpBox("This will be played when character dies.", MessageType.Info);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
