using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownCharacterCard))]
[CanEditMultipleObjects]
[DisallowMultipleComponent]
public class TopDownCharacterCardEditor : Editor {

    private Texture TopDownIcon;

    public TopDownCharacterCard td_target;

    public void OnEnable() {
        td_target = (TopDownCharacterCard)target;

        if (TopDownIcon == null) {
            TopDownIcon = Resources.Load("TopDownIcon") as Texture;
        }

        /*if(td_target.GetComponent<TopDownControllerInteract>()) {
            if (td_target.transform.Find("TD_CharacterCamera") == false) {
                GameObject tmp = Resources.Load("TD_CharacterCamera") as GameObject;
                GameObject inventoryCamera = Instantiate(tmp);
                inventoryCamera.name = "TD_CharacterCamera";
                inventoryCamera.transform.SetParent(td_target.transform);
                inventoryCamera.transform.localPosition = new Vector3(0f, 0.9f, 2f);
                inventoryCamera.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

                td_target.GetComponent<TopDownCharacterCard>().inventoryCamera = inventoryCamera;
                Debug.LogFormat("Setting up <b><color=yellow>Inventory Camera</color></b> as a child of <b><color=blue>" + td_target.gameObject.name + "</color></b> player character.");
            }

            if (td_target.gameObject.layer == 8) {
                TopDownControllerInteract[] allCharacters = GameObject.FindObjectsOfType<TopDownControllerInteract>();

                for(int i = 0; i < allCharacters.Length; i++) {
                    if (td_target.gameObject.layer == allCharacters[i].gameObject.layer) {
                        if (td_target.name != allCharacters[i].name) {
                            //Debug.LogFormat("You need to change layer of <b><color=yellow>" + td_target.gameObject.name + "</color></b> because <b><color=blue>" + allCharacters[i].name + "</color></b> is already set to that layer. If you do not change it <b><color=red>td_target.inventoryCamera</color></b> will render both characters.");
                        }
                    }
                }
            }
        }*/
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();

        GUIStyle boldCenteredLabel = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };

        GUIStyle simpleTitleLable = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleLeft };

        EditorStyles.textField.wordWrap = true;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.LabelField(new GUIContent(TopDownIcon), boldCenteredLabel, GUILayout.ExpandWidth(true), GUILayout.Height(32));
        EditorGUILayout.LabelField("- TOP DOWN RPG -", boldCenteredLabel);
        EditorGUILayout.LabelField("Character Info", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is character info. If this character is playable you can also set character card from your asset folders. \nMainly this is used to store your character info or to set up your AI characters.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (td_target.gameObject.GetComponent<TopDownAI>()) {
            EditorGUILayout.LabelField("AI COMPONENT DETECTED.", boldCenteredLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

            EditorGUILayout.LabelField("- AI Info -", boldCenteredLabel);

            EditorGUILayout.LabelField("Name:", simpleTitleLable);
            serializedObject.FindProperty("aiName").stringValue = EditorGUILayout.TextField(string.Empty, td_target.aiName);
            EditorGUILayout.HelpBox("This is the name of this AI.", MessageType.Info);

            EditorGUILayout.LabelField("Health:", simpleTitleLable);
            serializedObject.FindProperty("health").floatValue = EditorGUILayout.FloatField(string.Empty, td_target.health);
            EditorGUILayout.HelpBox("This is the starting health value of this AI.", MessageType.Info);

            EditorGUILayout.LabelField("Voice Set:", simpleTitleLable);
            serializedObject.FindProperty("aiVoiceSet").objectReferenceValue = (TopDownVoiceSet)EditorGUILayout.ObjectField(string.Empty, td_target.aiVoiceSet, typeof(TopDownVoiceSet), true);
            EditorGUILayout.HelpBox("If this is set then this AI will play configured voices in the game.", MessageType.Info);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            //////////////////////////////////

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

            EditorGUILayout.LabelField("- AI Detect -", boldCenteredLabel);

            EditorGUILayout.LabelField("Field of View:", simpleTitleLable);
            serializedObject.FindProperty("aiFieldOfView").intValue = EditorGUILayout.IntSlider((int)td_target.aiFieldOfView, 60, 180);
            EditorGUILayout.HelpBox("This represents field of view in which AI will detect player.", MessageType.Info);

            EditorGUILayout.LabelField("Detect Radius:", simpleTitleLable);
            serializedObject.FindProperty("aiDetectRadius").intValue = EditorGUILayout.IntSlider((int)td_target.aiDetectRadius, 4, 20);
            EditorGUILayout.HelpBox("This represents radius of detection.", MessageType.Info);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

        }
        if(td_target.GetComponent<TopDownControllerInteract>()) {
            EditorGUILayout.LabelField("PLAYER DETECTED.", boldCenteredLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
            EditorGUILayout.LabelField("Character:", simpleTitleLable);
            serializedObject.FindProperty("character").objectReferenceValue = (TopDownCharacter)EditorGUILayout.ObjectField(string.Empty, td_target.character, typeof(TopDownCharacter), true);
            EditorGUILayout.HelpBox("Here we place our character from assets that we created earlier.", MessageType.Info);
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

            EditorGUILayout.LabelField("- Player Info -", boldCenteredLabel);
            EditorGUILayout.HelpBox("Here You can preview your character info.", MessageType.Info);
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("- Health and Energy -", simpleTitleLable);
            td_target.health = EditorGUILayout.FloatField("Health:", td_target.health);
            td_target.energy = EditorGUILayout.FloatField("Energy:", td_target.energy);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("- Attributes -", simpleTitleLable);
            td_target.strength = EditorGUILayout.IntField("Strength:", td_target.strength);
            td_target.dexterity = EditorGUILayout.IntField("Dexterity:", td_target.dexterity);
            td_target.constitution = EditorGUILayout.IntField("Constitution:", td_target.constitution);
            td_target.willpower = EditorGUILayout.IntField("Willpower:", td_target.willpower);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("- Equipment Modifiers -", simpleTitleLable);
            td_target.armorPoints = EditorGUILayout.IntField("Armor Points:", td_target.armorPoints);
            td_target.damagePoints = EditorGUILayout.IntField("Damage Points:", td_target.damagePoints);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("- Level and Experience -", simpleTitleLable);
            td_target.level = EditorGUILayout.IntField("Level:", td_target.level);
            td_target.experience = EditorGUILayout.IntField("Experience:", td_target.experience);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("- Skills -", simpleTitleLable);
            td_target.skillPoints = EditorGUILayout.IntField("Skill Points:", td_target.skillPoints);
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        if(td_target.gameObject.tag == "NPC") {

        }

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void OnSceneGUI() {
        if (td_target.GetComponent<TopDownAI>()) {
            Handles.color = new Color(204, 0, 0, 0.25f);
            Handles.DrawSolidArc(td_target.transform.position, td_target.transform.up, td_target.transform.forward, td_target.aiFieldOfView / 2, td_target.aiDetectRadius);
            Handles.DrawSolidArc(td_target.transform.position, td_target.transform.up, td_target.transform.forward, -td_target.aiFieldOfView / 2, td_target.aiDetectRadius);
            Handles.color = Color.white;

            Handles.color = new Color(255, 255, 0, 0.25f);
            Handles.DrawSolidDisc(td_target.transform.position, td_target.transform.up, td_target.aiDetectRadius * 0.4f);
            Handles.color = Color.white;
        }
    }
}
