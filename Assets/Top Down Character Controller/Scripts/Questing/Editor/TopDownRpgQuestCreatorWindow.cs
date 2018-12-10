using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TopDownRpgQuestCreatorWindow : EditorWindow {

    private static Texture TopDownIcon;

    public string questName;
    public QuestType questType;
    public GameObject questTarget;
    public List<GameObject> questTargets;
    public QuestEnding questEnding;
    public string questFinishDialogChoice;
    public string questFinishDialogReply;

    [MenuItem("Top Down RPG/New Quest %#q", false, 4)]
    static void Init() {
        TopDownRpgQuestCreatorWindow window = (TopDownRpgQuestCreatorWindow)EditorWindow.GetWindow(typeof(TopDownRpgQuestCreatorWindow));
        window.Show();

        if (TopDownIcon == null) {
            TopDownIcon = Resources.Load("TopDownIcon") as Texture;
        }

        window.minSize = new Vector2(500f, 575f);
    }

    void OnGUI() {

        GUIStyle boldCenteredLabel = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };

        EditorStyles.textField.wordWrap = true;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        EditorGUILayout.LabelField(new GUIContent(TopDownIcon), boldCenteredLabel, GUILayout.ExpandWidth(true), GUILayout.Height(32));
        EditorGUILayout.LabelField("- TOP DOWN RPG -", boldCenteredLabel);
        EditorGUILayout.LabelField("Setup New Quest", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is used to setup new quest.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        questName = EditorGUILayout.TextField("Quest Title:", questName);
        EditorGUILayout.HelpBox("This is the name of the Quest that will show up in the questlog.", MessageType.Info);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        questType = (QuestType)EditorGUILayout.EnumPopup("Quest Type:", questType);
        EditorGUILayout.HelpBox("What kind of quest this is. This basicly sets its main objective.", MessageType.Info);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (questType == QuestType.KillAllTargets) {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

            ScriptableObject target = this;
            SerializedObject so = new SerializedObject(target);
            SerializedProperty stringsProperty = so.FindProperty("questTargets");

            EditorGUILayout.PropertyField(stringsProperty, true);
            so.ApplyModifiedProperties();

            EditorGUILayout.HelpBox("Here we will set our quests objective.", MessageType.Info);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        else {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

            questTarget = (GameObject)EditorGUILayout.ObjectField("Quest Objective:", questTarget, typeof(GameObject), true);
            EditorGUILayout.HelpBox("Here we will set our quests objective.", MessageType.Info);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        questEnding = (QuestEnding)EditorGUILayout.EnumPopup("Quest Ending:", questEnding);
        EditorGUILayout.HelpBox("Determines in what manner the quest will finish.", MessageType.Info);

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (questEnding == QuestEnding.ReturnToNpc) {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

            questFinishDialogChoice = EditorGUILayout.TextField("Finish Quest Dialog Choice:", questFinishDialogChoice);
            EditorGUILayout.HelpBox("If quest is started by NPC, this choice will be visible in his dialog menu when quest is done.", MessageType.Info);

            questFinishDialogReply = EditorGUILayout.TextField("Finish Quest Dialog Reply:", questFinishDialogReply, GUILayout.MinHeight(90));
            EditorGUILayout.HelpBox("This will be the reply the npc will give us when the above choice is activated.", MessageType.Info);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        if (GUILayout.Button("Create Quest")) {
            GameObject questGo = new GameObject();
            questGo.name = "Quest - " + questName;

            Transform questHub = GameObject.FindObjectOfType<TopDownRpgQuestHub>().transform;
            questGo.transform.SetParent(questHub.transform.Find("TD_Quests").transform);

            TopDownRpgQuest quest = questGo.AddComponent<TopDownRpgQuest>();
            quest.questName = questName;
            quest.questType = questType;
            quest.questTarget = questTarget;
            quest.questTargets = questTargets;
            quest.questEnding = questEnding;
            quest.questFinishChoice = questFinishDialogChoice;
            quest.questFinishDialog = questFinishDialogReply;

            Debug.Log("Quest named '"+questName+"' added to scene.");
        }

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
