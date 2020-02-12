using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopDownStartupItemsSetup))]
[DisallowMultipleComponent]
public class TopDownStartupItemsSetupEditor : Editor {

    private Texture TopDownIcon;

    private void OnEnable() {
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
        EditorGUILayout.LabelField("Startup Items", boldCenteredLabel);
        EditorGUILayout.HelpBox("With this component we can assign items that will be equiped and placed in characters inventory on scene startup.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUILayout.LabelField("- Items to equip -", boldCenteredLabel);
        EditorGUILayout.HelpBox("Here we place items that we want for character to equip.", MessageType.Info);
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemsToEquip"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemsToPlaceInInventory"), true);

        EditorGUILayout.Space();

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();
    }
}
