using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownUITooltipInfo))]
[CanEditMultipleObjects]
public class TopDownUITooltipInfoEditor : Editor {

    private Texture TopDownIcon;

    private TopDownUITooltipInfo td_target;

    private void OnEnable() {
        td_target = (TopDownUITooltipInfo)target;

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
        EditorGUILayout.LabelField("UI Element Tooltip", boldCenteredLabel);
        EditorGUILayout.HelpBox("When placed on UI element it will trigger a tooltip on mouse over showing text that is written in the text box bellow.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.Space();
        td_target.tooltipInfo = EditorGUILayout.TextField("Tooltip Text:", td_target.tooltipInfo, GUILayout.MinHeight(100));
        EditorGUILayout.HelpBox("Here write text that you want to appear when mouse over this UI element.", MessageType.Info);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Custom UI Element Offset");
        td_target.tooltipOffset = EditorGUILayout.Vector2Field(string.Empty, td_target.tooltipOffset);
        EditorGUILayout.HelpBox("Offset value of all UI tooltips is set in the TopDownUIManager script. Here you can set custom offset if you want to overwrite that general setting.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
