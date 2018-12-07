using UnityEditor;
using UnityEngine;

public class TopDownSetupSceneEditorWindow : EditorWindow {

    private static Texture TopDownIcon;

    [MenuItem("Top Down RPG/Setup New Scene", false, 1)]
    static void Init() {
        TopDownSetupSceneEditorWindow window = (TopDownSetupSceneEditorWindow)EditorWindow.GetWindow(typeof(TopDownSetupSceneEditorWindow));
        window.Show();

        if (TopDownIcon == null) {
            TopDownIcon = Resources.Load("TopDownIcon") as Texture;
        }
    }

    void OnGUI() {

        GUIStyle boldCenteredLabel = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };

        EditorStyles.textField.wordWrap = true;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.LabelField(new GUIContent(TopDownIcon), boldCenteredLabel, GUILayout.ExpandWidth(true), GUILayout.Height(32));
        EditorGUILayout.LabelField("- TOP DOWN RPG -", boldCenteredLabel);
        EditorGUILayout.LabelField("Setup New SCENE", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is used to setup new scene.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        //aiModel = (GameObject)EditorGUILayout.ObjectField("AI Model:", aiModel, typeof(GameObject), true);

        if(GUILayout.Button("Setup Currently Opened Scene")) {

            if(GameObject.FindObjectOfType<Camera>()) {
                DestroyImmediate(GameObject.FindObjectOfType<Camera>().gameObject);
                Debug.LogFormat("Camera component detected. <i>Destroying it.</i>");
            }

            if (GameObject.Find("TD_GameManager") == null) {
                GameObject gm = Instantiate(Resources.Load("TD_GameManager") as GameObject);
                gm.name = gm.name.Replace("(Clone)", "").Trim();
            }
            else {
                Debug.LogErrorFormat("TD_GameManager already exists in the scene. Setting up scene <b>FAILED</b>.");
                return;
            }

            if (GameObject.Find("TD_Camera") == null) {
                GameObject cm = Instantiate(Resources.Load("TD_Camera") as GameObject);
                cm.name = cm.name.Replace("(Clone)", "").Trim();
            }
            else {
                Debug.LogErrorFormat("TD_Camera already exists in the scene. Setting up scene <b>FAILED</b>.");
                return;
            }

            if (GameObject.Find("TD_UI") == null) {
                GameObject ui = Instantiate(Resources.Load("TD_UI") as GameObject);
                ui.name = ui.name.Replace("(Clone)", "").Trim();
            }
            else {
                Debug.LogErrorFormat("TD_UI already exists in the scene. Setting up scene <b>FAILED</b>.");
                return;
            }

            if (GameObject.Find("TD_EventSystem") == null) {
                GameObject es = Instantiate(Resources.Load("TD_EventSystem") as GameObject);
                es.name = es.name.Replace("(Clone)", "").Trim();
            }
            else {
                Debug.LogErrorFormat("TD_EventSystem already exists in the scene. Setting up scene <b>FAILED</b>.");
                return;
            }

            Debug.Log("Scene has been setup. You now have everything needed to run your game. You now have to setup player character, ai and edit their values to your liking.");
        }
    }
}
