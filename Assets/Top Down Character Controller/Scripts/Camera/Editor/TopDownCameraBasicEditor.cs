using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopDownCameraBasic))]
[DisallowMultipleComponent]
public class TopDownCameraBasicEditor : Editor {

    private TopDownCameraBasic td_target;

    private Texture TopDownIcon;

    public void OnEnable() {

        td_target = (TopDownCameraBasic)target;

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
        EditorGUILayout.LabelField("Top Down Character Controller", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is games main camera. It is used to follow our characters around the scene, or to move it freely.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUI.BeginDisabledGroup(true);
        td_target.cameraType = (CameraType)EditorGUILayout.EnumPopup("Camera Mode:", td_target.cameraType);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Camera Position", simpleTitleLable);
        td_target.yAxisOffset = EditorGUILayout.FloatField("Offset:", td_target.yAxisOffset);
        EditorGUILayout.HelpBox("Here we determine how much will camera be offset based on characters position at Y axis. (Used so the camera wont be centered on characters feet.)", MessageType.Info);
        EditorGUILayout.Space();
        td_target.cameraAngleMin = EditorGUILayout.FloatField("Minimum X Angle:", td_target.cameraAngleMin);
        td_target.cameraAngleMax = EditorGUILayout.FloatField("Maximum X Angle:", td_target.cameraAngleMax);
        EditorGUILayout.HelpBox("Determines minimum and maximum X axis angle.", MessageType.Info);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Distance Options", simpleTitleLable);
        td_target.distanceDefault = EditorGUILayout.FloatField("Default Distance:", td_target.distanceDefault);
        td_target.distanceMin = EditorGUILayout.FloatField("Minimum Distance:", td_target.distanceMin);
        td_target.distanceMax = EditorGUILayout.FloatField("Maximum Distance:", td_target.distanceMax);
        EditorGUILayout.HelpBox("Default Distance represents starting distance and current distance while game is live. Minimum and Maximum values determine how near or far camera can zoom.", MessageType.Info);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Speed Options", simpleTitleLable);
        td_target.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed:", td_target.rotationSpeed);
        EditorGUILayout.HelpBox("Rotation Speed determines at what speed will camera rotate.", MessageType.Info);
        td_target.freeCameraSpeed = EditorGUILayout.FloatField("Free Camera Mode Speed:", td_target.freeCameraSpeed);
        EditorGUILayout.HelpBox("Free Camera Mode Speed determines at what speed will camera move when in Free Camera Mode.", MessageType.Info);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
