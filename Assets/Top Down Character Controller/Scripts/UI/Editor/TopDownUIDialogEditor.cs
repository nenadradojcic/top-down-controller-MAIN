using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownUIDialog))]
[DisallowMultipleComponent]
public class TopDownUIDialogEditor : Editor {

    private TopDownUIDialog td_target;

    private Texture TopDownIcon;

    public bool responseOne;
    public bool responseTwo;
    public bool responseThree;
    public bool responseFour;

    public void OnEnable() {

        td_target = (TopDownUIDialog)target;

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
        EditorGUILayout.LabelField("Dialog", boldCenteredLabel);
        EditorGUILayout.HelpBox("This component will show dialog when player interact with this Game Object.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        if (td_target.GetComponent<TopDownControllerMain>()) {
            EditorGUILayout.HelpBox("Dialog component should not be put on the parent Game Object of a character.", MessageType.Error);
        }
        else {
            EditorGUILayout.HelpBox("Here we place the dialog line that will be displayed on dialog initialization.", MessageType.Info);
        }
        EditorGUILayout.LabelField("Opening Line Dialog:");
        td_target.welcomeDialog = EditorGUILayout.TextField(string.Empty, td_target.welcomeDialog, GUILayout.Height(80));

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.LabelField("Responses", boldCenteredLabel);
        if (td_target.showChoiceOne == false) {
            if (GUILayout.Button("Add Response")) {
                responseOne = true;
                td_target.showChoiceOne = true;
            }
        }
        else {

            if (td_target.choiceOne == string.Empty) {
                responseOne = EditorGUILayout.Foldout(responseOne, "Response 1");
            }
            else {
                responseOne = EditorGUILayout.Foldout(responseOne, td_target.choiceOne);
            }
            if (responseOne) {

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

                td_target.choiceOneType = (DialogType)EditorGUILayout.EnumPopup("Response Type", td_target.choiceOneType);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("removeOnChoiceOne"), new GUIContent("Remove After Activating"), true);
                EditorGUILayout.LabelField("Response Line:");
                td_target.choiceOne = EditorGUILayout.TextField(string.Empty, td_target.choiceOne, GUILayout.Height(80));
                if (td_target.choiceOneType == DialogType.None) {
                    EditorGUILayout.LabelField("Dialog Line:");
                    td_target.choiceOneDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceOneDialog, GUILayout.Height(80));
                }
                else if (td_target.choiceOneType == DialogType.CloseDialog) {
                    EditorGUILayout.LabelField("Dialog Line:");
                    td_target.choiceOneDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceOneDialog, GUILayout.Height(80));
                }
                else if (td_target.choiceOneType == DialogType.BranchDialog) {
                    EditorGUILayout.LabelField("Dialog Line:");
                    td_target.choiceOneDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceOneDialog, GUILayout.Height(80));
                    if (td_target.branchOneDialog == null) {
                        if (GUILayout.Button("Create New Branch")) {
                            GameObject branch = new GameObject();
                            branch.name = "Dialog Branch";
                            branch.transform.SetParent(td_target.transform);
                            branch.transform.localPosition = Vector3.zero;
                            td_target.branchOneDialog = branch.AddComponent<TopDownUIDialog>();
                            td_target.branchOneDialog.enabled = false;
                        }
                    }
                    else {
                        if (GUILayout.Button("Remove Branch")) {
                            DestroyImmediate(td_target.branchOneDialog.gameObject);
                        }
                    }
                    td_target.branchOneDialog = (TopDownUIDialog)EditorGUILayout.ObjectField("Branch To Dialog:", td_target.branchOneDialog, typeof(TopDownUIDialog), true);
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceOneEvent"), new GUIContent("On Response Click"), true);

                if (GUILayout.Button("Remove Response")) {
                    if (td_target.showChoiceTwo == true) {
                        td_target.choiceOne = td_target.choiceTwo;
                        td_target.choiceOneDialog = td_target.choiceTwoDialog;
                        td_target.choiceOneType = td_target.choiceTwoType;
                        td_target.showChoiceOne = td_target.showChoiceTwo;
                        td_target.branchOneDialog = td_target.branchTwoDialog;
                        td_target.branchTwoDialog = null;
                        td_target.choiceOneEvent = td_target.choiceTwoEvent;

                        td_target.choiceTwo = string.Empty;
                        td_target.choiceTwoDialog = string.Empty;
                        td_target.choiceTwoType = DialogType.None;
                        td_target.showChoiceTwo = false;
                        if (td_target.branchTwoDialog != null) {
                            DestroyImmediate(td_target.branchTwoDialog.gameObject);
                            td_target.branchTwoDialog = null;
                        }
                        td_target.choiceTwoEvent = null;

                        Repaint();

                        if (td_target.showChoiceThree == true) {
                            td_target.choiceTwo = td_target.choiceThree;
                            td_target.choiceTwoDialog = td_target.choiceThreeDialog;
                            td_target.choiceTwoType = td_target.choiceThreeType;
                            td_target.showChoiceTwo = td_target.showChoiceThree;
                            td_target.branchTwoDialog = td_target.branchThreeDialog;
                            td_target.branchThreeDialog = null;
                            td_target.choiceTwoEvent = td_target.choiceThreeEvent;

                            td_target.choiceThree = string.Empty;
                            td_target.choiceThreeDialog = string.Empty;
                            td_target.choiceThreeType = DialogType.None;
                            td_target.showChoiceThree = false;
                            if (td_target.branchThreeDialog != null) {
                                DestroyImmediate(td_target.branchThreeDialog.gameObject);
                                td_target.branchThreeDialog = null;
                            }
                            td_target.choiceThreeEvent = null;

                            Repaint();

                            if (td_target.showChoiceFour == true) {
                                td_target.choiceThree = td_target.choiceFour;
                                td_target.choiceThreeDialog = td_target.choiceFourDialog;
                                td_target.choiceThreeType = td_target.choiceFourType;
                                td_target.showChoiceThree = td_target.showChoiceFour;
                                td_target.branchThreeDialog = td_target.branchFourDialog;
                                td_target.branchFourDialog = null;
                                td_target.choiceThreeEvent = td_target.choiceFourEvent;

                                td_target.choiceFour = string.Empty;
                                td_target.choiceFourDialog = string.Empty;
                                td_target.choiceFourType = DialogType.None;
                                td_target.showChoiceFour = false;
                                if (td_target.branchFourDialog != null) {
                                    DestroyImmediate(td_target.branchFourDialog.gameObject);
                                    td_target.branchFourDialog = null;
                                }
                                td_target.choiceFourEvent = null;

                                Repaint();
                            }
                            else {
                                td_target.choiceThree = string.Empty;
                                td_target.choiceThreeDialog = string.Empty;
                                td_target.choiceThreeType = DialogType.None;
                                td_target.showChoiceThree = false;
                                if (td_target.branchThreeDialog != null) {
                                    DestroyImmediate(td_target.branchThreeDialog.gameObject);
                                    td_target.branchThreeDialog = null;
                                }
                                td_target.choiceThreeEvent = null;
                            }
                        }
                        else {
                            td_target.choiceTwo = string.Empty;
                            td_target.choiceTwoDialog = string.Empty;
                            td_target.choiceTwoType = DialogType.None;
                            td_target.showChoiceTwo = false;
                            if (td_target.branchTwoDialog != null) {
                                DestroyImmediate(td_target.branchTwoDialog.gameObject);
                                td_target.branchTwoDialog = null;
                            }
                            td_target.choiceTwoEvent = null;
                        }
                    }
                    else {
                        td_target.choiceOne = string.Empty;
                        td_target.choiceOneDialog = string.Empty;
                        td_target.choiceOneType = DialogType.None;
                        td_target.showChoiceOne = false;
                        if (td_target.branchOneDialog != null) {
                            DestroyImmediate(td_target.branchOneDialog.gameObject);
                            td_target.branchOneDialog = null;
                        }
                        td_target.choiceOneEvent = null;
                    }
                }
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            if (td_target.showChoiceTwo == false) {
                if (GUILayout.Button("Add Response")) {
                    responseTwo = true;
                    td_target.showChoiceTwo = true;
                }
            }
            else {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                if (td_target.choiceTwo == string.Empty) {
                    responseTwo = EditorGUILayout.Foldout(responseTwo, "Response 2");
                }
                else {
                    responseTwo = EditorGUILayout.Foldout(responseTwo, td_target.choiceTwo);
                }
                if (responseTwo) {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
                    td_target.choiceTwoType = (DialogType)EditorGUILayout.EnumPopup("Response Type", td_target.choiceTwoType);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("removeOnChoiceTwo"), new GUIContent("Remove After Activating"), true); 
                    EditorGUILayout.LabelField("Response Line:");
                    td_target.choiceTwo = EditorGUILayout.TextField(string.Empty, td_target.choiceTwo, GUILayout.Height(80));
                    if (td_target.choiceTwoType == DialogType.None) {
                        EditorGUILayout.LabelField("Dialog Line:");
                        td_target.choiceTwoDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceTwoDialog, GUILayout.Height(80));
                    }
                    else if (td_target.choiceTwoType == DialogType.CloseDialog) {
                        EditorGUILayout.LabelField("Dialog Line:");
                        td_target.choiceTwoDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceTwoDialog, GUILayout.Height(80));
                    }
                    else if (td_target.choiceTwoType == DialogType.BranchDialog) {
                        EditorGUILayout.LabelField("Dialog Line:");
                        td_target.choiceTwoDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceTwoDialog, GUILayout.Height(80));
                        if (td_target.branchTwoDialog == null) {
                            if (GUILayout.Button("Create New Branch")) {
                                GameObject branch = new GameObject();
                                branch.name = "Dialog Branch";
                                branch.transform.SetParent(td_target.transform);
                                branch.transform.localPosition = Vector3.zero;
                                td_target.branchTwoDialog = branch.AddComponent<TopDownUIDialog>();
                                td_target.branchTwoDialog.enabled = false;
                            }
                        }
                        td_target.branchTwoDialog = (TopDownUIDialog)EditorGUILayout.ObjectField("Branch To Dialog:", td_target.branchTwoDialog, typeof(TopDownUIDialog), true);
                    }

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceTwoEvent"), new GUIContent("On Response Click"), true);

                    if (GUILayout.Button("Remove Response")) {
                        if (td_target.showChoiceThree == true) {
                            td_target.choiceTwo = td_target.choiceThree;
                            td_target.choiceTwoDialog = td_target.choiceThreeDialog;
                            td_target.choiceTwoType = td_target.choiceThreeType;
                            td_target.showChoiceTwo = td_target.showChoiceThree;
                            td_target.branchTwoDialog = td_target.branchThreeDialog;
                            td_target.branchThreeDialog = null;
                            td_target.choiceTwoEvent = td_target.choiceThreeEvent;

                            td_target.choiceThree = string.Empty;
                            td_target.choiceThreeDialog = string.Empty;
                            td_target.choiceThreeType = DialogType.None;
                            td_target.showChoiceThree = false;
                            if (td_target.branchThreeDialog != null) {
                                DestroyImmediate(td_target.branchThreeDialog.gameObject);
                                td_target.branchThreeDialog = null;
                            }
                            td_target.choiceThreeEvent = null;

                            Repaint();

                            if (td_target.showChoiceFour == true) {
                                td_target.choiceThree = td_target.choiceFour;
                                td_target.choiceThreeDialog = td_target.choiceFourDialog;
                                td_target.choiceThreeType = td_target.choiceFourType;
                                td_target.showChoiceThree = td_target.showChoiceFour;
                                td_target.branchThreeDialog = td_target.branchFourDialog;
                                td_target.branchFourDialog = null;
                                td_target.choiceThreeEvent = td_target.choiceFourEvent;
                                if (td_target.branchFourDialog == null) {
                                    td_target.branchFourDialog = null;
                                }

                                td_target.choiceFour = string.Empty;
                                td_target.choiceFourDialog = string.Empty;
                                td_target.choiceFourType = DialogType.None;
                                td_target.showChoiceFour = false;
                                if (td_target.branchFourDialog != null) {
                                    DestroyImmediate(td_target.branchFourDialog.gameObject);
                                    td_target.branchFourDialog = null;
                                }
                                td_target.choiceFourEvent = null;

                                Repaint();
                            }
                            else {
                                td_target.choiceThree = string.Empty;
                                td_target.choiceThreeDialog = string.Empty;
                                td_target.choiceThreeType = DialogType.None;
                                td_target.showChoiceThree = false;
                                if (td_target.branchThreeDialog != null) {
                                    DestroyImmediate(td_target.branchThreeDialog.gameObject);
                                    td_target.branchThreeDialog = null;
                                }
                                td_target.choiceThreeEvent = null;
                            }
                        }
                        else {
                            td_target.choiceTwo = string.Empty;
                            td_target.choiceTwoDialog = string.Empty;
                            td_target.choiceTwoType = DialogType.None;
                            td_target.showChoiceTwo = false;
                            if (td_target.branchTwoDialog != null) {
                                DestroyImmediate(td_target.branchTwoDialog.gameObject);
                                td_target.branchTwoDialog = null;
                            }
                            td_target.choiceTwoEvent = null;
                        }
                    }
                    EditorGUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
                
                if (td_target.showChoiceThree == false) {
                    if (GUILayout.Button("Add Response")) {
                        responseThree = true;
                        td_target.showChoiceThree = true;
                    }
                }
                else {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    if (td_target.choiceThree == string.Empty) {
                        responseThree = EditorGUILayout.Foldout(responseThree, "Response 3");
                    }
                    else {
                        responseThree = EditorGUILayout.Foldout(responseThree, td_target.choiceThree);
                    }
                    if (responseThree) {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
                        td_target.choiceThreeType = (DialogType)EditorGUILayout.EnumPopup("Response Type", td_target.choiceThreeType);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("removeOnChoiceThree"), new GUIContent("Remove After Activating"), true);
                        EditorGUILayout.LabelField("Response Line:");
                        td_target.choiceThree = EditorGUILayout.TextField(string.Empty, td_target.choiceThree, GUILayout.Height(80));
                        if (td_target.choiceThreeType == DialogType.None) {
                            EditorGUILayout.LabelField("Dialog Line:");
                            td_target.choiceThreeDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceThreeDialog, GUILayout.Height(80));
                        }
                        else if (td_target.choiceThreeType == DialogType.CloseDialog) {
                            EditorGUILayout.LabelField("Dialog Line:");
                            td_target.choiceThreeDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceThreeDialog, GUILayout.Height(80));
                        }
                        else if (td_target.choiceThreeType == DialogType.BranchDialog) {
                            EditorGUILayout.LabelField("Dialog Line:");
                            td_target.choiceThreeDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceThreeDialog, GUILayout.Height(80));
                            if (td_target.branchThreeDialog == null) {
                                if (GUILayout.Button("Create New Branch")) {
                                    GameObject branch = new GameObject();
                                    branch.name = "Dialog Branch";
                                    branch.transform.SetParent(td_target.transform);
                                    branch.transform.localPosition = Vector3.zero;
                                    td_target.branchThreeDialog = branch.AddComponent<TopDownUIDialog>();
                                    td_target.branchThreeDialog.enabled = false;
                                }
                            }
                            td_target.branchThreeDialog = (TopDownUIDialog)EditorGUILayout.ObjectField("Branch To Dialog:", td_target.branchThreeDialog, typeof(TopDownUIDialog), true);
                        }

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceThreeEvent"), new GUIContent("On Response Click"), true);

                        if (GUILayout.Button("Remove Response")) {
                            if (td_target.showChoiceFour == true) {
                                td_target.choiceThree = td_target.choiceFour;
                                td_target.choiceThreeDialog = td_target.choiceFourDialog;
                                td_target.choiceThreeType = td_target.choiceFourType;
                                td_target.showChoiceThree = td_target.showChoiceFour;
                                td_target.branchThreeDialog = td_target.branchFourDialog;
                                td_target.branchFourDialog = null;
                                td_target.choiceThreeEvent = td_target.choiceFourEvent;

                                td_target.choiceFour = string.Empty;
                                td_target.choiceFourDialog = string.Empty;
                                td_target.choiceFourType = DialogType.None;
                                td_target.showChoiceFour = false;
                                if (td_target.branchFourDialog != null) {
                                    DestroyImmediate(td_target.branchFourDialog.gameObject);
                                    td_target.branchFourDialog = null;
                                }
                                td_target.choiceFourEvent = null;

                                Repaint();
                            }
                            else {
                                td_target.choiceThree = string.Empty;
                                td_target.choiceThreeDialog = string.Empty;
                                td_target.choiceThreeType = DialogType.None;
                                td_target.showChoiceThree = false;
                                if (td_target.branchThreeDialog != null) {
                                    DestroyImmediate(td_target.branchThreeDialog.gameObject);
                                    td_target.branchThreeDialog = null;
                                }
                                td_target.choiceThreeEvent = null;
                            }
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                    }

                    if (td_target.showChoiceFour == false) {
                        if (GUILayout.Button("Add Response")) {
                            responseFour = true;
                            td_target.showChoiceFour = true;
                        }
                    }
                    else {
                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        if (td_target.choiceFour == string.Empty) {
                            responseFour = EditorGUILayout.Foldout(responseFour, "Response 4");
                        }
                        else {
                            responseFour = EditorGUILayout.Foldout(responseFour, td_target.choiceFour);
                        }
                        if (responseFour) {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
                            td_target.choiceFourType = (DialogType)EditorGUILayout.EnumPopup("Response Type", td_target.choiceFourType);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("removeOnChoiceFour"), new GUIContent("Remove After Activating"), true);
                            EditorGUILayout.LabelField("Response Line:");
                            td_target.choiceFour = EditorGUILayout.TextField(string.Empty, td_target.choiceFour, GUILayout.Height(80));
                            if (td_target.choiceFourType == DialogType.None) {
                                EditorGUILayout.LabelField("Dialog Line:");
                                td_target.choiceFourDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceFourDialog, GUILayout.Height(80));
                            }
                            else if (td_target.choiceFourType == DialogType.CloseDialog) {
                                EditorGUILayout.LabelField("Dialog Line:");
                                td_target.choiceFourDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceFourDialog, GUILayout.Height(80));
                            }
                            else if (td_target.choiceFourType == DialogType.BranchDialog) {
                                EditorGUILayout.LabelField("Dialog Line:");
                                td_target.choiceFourDialog = EditorGUILayout.TextField(string.Empty, td_target.choiceFourDialog, GUILayout.Height(80));
                                if (td_target.branchFourDialog == null) {
                                    if (GUILayout.Button("Create New Branch")) {
                                        GameObject branch = new GameObject();
                                        branch.name = "Dialog Branch";
                                        branch.transform.SetParent(td_target.transform);
                                        branch.transform.localPosition = Vector3.zero;
                                        td_target.branchFourDialog = branch.AddComponent<TopDownUIDialog>();
                                        td_target.branchFourDialog.enabled = false;
                                    }
                                }
                                td_target.branchFourDialog = (TopDownUIDialog)EditorGUILayout.ObjectField("Branch To Dialog:", td_target.branchFourDialog, typeof(TopDownUIDialog), true);
                            }

                            EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceFourEvent"), new GUIContent("On Response Click"), true);

                            if (GUILayout.Button("Remove Response")) {
                                if (td_target.showChoiceFour == true) {
                                    td_target.choiceFour = string.Empty;
                                    td_target.choiceFourDialog = string.Empty;
                                    td_target.choiceFourType = DialogType.None;
                                    td_target.showChoiceFour = false;
                                    if (td_target.branchFourDialog != null) {
                                        DestroyImmediate(td_target.branchFourDialog.gameObject);
                                        td_target.branchFourDialog = null;
                                    }
                                    td_target.choiceFourEvent = null;
                                }
                            }
                            EditorGUILayout.EndVertical();
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
            }
        }

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}