﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownControllerInteract))]
[DisallowMultipleComponent]
public class TopDownControllerInteractEditor : Editor {

    private bool interactibleTagsAndDistances;

    private TopDownControllerInteract td_target;

    private Texture TopDownIcon;

    public void OnEnable() {

        td_target = (TopDownControllerInteract)target;

        if (TopDownIcon == null) {
            TopDownIcon = Resources.Load("TopDownIcon") as Texture;
        }
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();

        GUIStyle boldCenteredLabel = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };

        GUIStyle simpleTitleLable = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleLeft };

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
        EditorGUILayout.LabelField("Top Down Interact", boldCenteredLabel);
        EditorGUILayout.HelpBox("With this component we are able to interact to objects in the game world. Here we set tags for walkable surfaces," +
            "non-player characters, enemies, items and containers. We also set distance at what the player should be before interacting with said object.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        interactibleTagsAndDistances = EditorGUILayout.Foldout(interactibleTagsAndDistances, "Tags and Distances", simpleStyle);

        if (interactibleTagsAndDistances == true) {
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
            EditorGUILayout.LabelField("Walkable Surface", simpleTitleLable);
            serializedObject.FindProperty("walkableTag").stringValue = EditorGUILayout.TextField("Tag:", td_target.walkableTag);
            serializedObject.FindProperty("defaultStopDistance").floatValue = EditorGUILayout.FloatField("Stop Distance:", td_target.defaultStopDistance);
            EditorGUILayout.HelpBox("This tag will be used for walkable surfaces. Character will be able to walk only on surfaces set on this tag. The distance set will" +
                "measure where will player stop when he gets near the selected surface. This distance will also be used for any other interactible activity not preset in this component.", MessageType.Info);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
            EditorGUILayout.LabelField("Non-player Characters", simpleTitleLable);
            serializedObject.FindProperty("npcTag").stringValue = EditorGUILayout.TextField("NPCs Tag:", td_target.npcTag);
            serializedObject.FindProperty("enemyTag").stringValue = EditorGUILayout.TextField("Hostiles Tag:", td_target.enemyTag);
            serializedObject.FindProperty("enemyStopDistance").floatValue = EditorGUILayout.FloatField("Stop Distance Melee:", td_target.enemyStopDistance);
            serializedObject.FindProperty("enemyStopDistanceRanged").floatValue = EditorGUILayout.FloatField("Stop Distance Ranged:", td_target.enemyStopDistanceRanged);
            EditorGUILayout.HelpBox("This tag will be used for non-player characters (npcs). The stopping distance is used to tell when will player stop and interact with npc, or start fight with enemy.", MessageType.Info);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
            EditorGUILayout.LabelField("Items", simpleTitleLable);
            serializedObject.FindProperty("itemTag").stringValue = EditorGUILayout.TextField("Tag:", td_target.itemTag);
            serializedObject.FindProperty("itemStopDistance").floatValue = EditorGUILayout.FloatField("Stop Distance:", td_target.itemStopDistance);
            EditorGUILayout.HelpBox("This tag will be used for items in the game world. The stopping distance is used to tell when will player be able to pick up item in his inventory.", MessageType.Info);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
            EditorGUILayout.LabelField("Containers", simpleTitleLable);
            serializedObject.FindProperty("chestTag").stringValue = EditorGUILayout.TextField("Tag:", td_target.chestTag);
            serializedObject.FindProperty("chestStopDistance").floatValue = EditorGUILayout.FloatField("Stop Distance:", td_target.chestStopDistance);
            EditorGUILayout.HelpBox("This tag will be used for containers. The stopping distance is used to tell when will container open up.", MessageType.Info);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
