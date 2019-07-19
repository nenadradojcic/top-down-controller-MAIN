using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(TopDownUINpcNameBar))]
[CanEditMultipleObjects]
public class TopDownUINpcNameBarEditor : Editor {

    private Texture TopDownIcon;

    private TopDownUINpcNameBar td_target;

    private void OnEnable() {
        td_target = (TopDownUINpcNameBar)target;

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
        EditorGUILayout.LabelField("Npc Name and Health Bar", boldCenteredLabel);
        EditorGUILayout.HelpBox("If this is enabled it will place name and health over NPC that you put your mouse cursor over.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.Space();

        td_target.placeBarOverHead = EditorGUILayout.Toggle("Put bar over head?", td_target.placeBarOverHead);

        if (td_target.placeBarOverHead == true) {
            EditorGUILayout.LabelField("Offset on Y Axis");
            td_target.yOffsetOverHead = EditorGUILayout.FloatField(td_target.yOffsetOverHead);
            EditorGUILayout.HelpBox("An offset that will put text in place on Y axis.", MessageType.Info);
        }
        else {
            EditorGUILayout.LabelField("Offset on Y Axis");
            td_target.yOffset = EditorGUILayout.FloatField(td_target.yOffset);
            EditorGUILayout.HelpBox("An offset that will put text in place on Y axis.", MessageType.Info);
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("UI Elements");
        EditorGUILayout.HelpBox("Here setup your UI elements. Text component for name and Image component for health bar.", MessageType.Info);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Health Bar Image Component");
        td_target.healthBar = (Image)EditorGUILayout.ObjectField(string.Empty, td_target.healthBar, typeof(Image), true);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Energy Bar Image Component");
        td_target.energyBar = (Image)EditorGUILayout.ObjectField(string.Empty, td_target.energyBar, typeof(Image), true);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Portrait Image Component");
        td_target.portraitImage = (Image)EditorGUILayout.ObjectField(string.Empty, td_target.portraitImage, typeof(Image), true);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Name Text Component");
        td_target.nameText = (Text)EditorGUILayout.ObjectField(string.Empty, td_target.nameText, typeof(Text), true);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

}
