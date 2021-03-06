﻿using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class TopDownAIEditorWindow : EditorWindow {

    private static Texture TopDownIcon;

    private GameObject aiModel;
    private bool aiHostile;

    [MenuItem("Top Down RPG/New AI NPC %#n", false, 3)]
    static void Init() {
        TopDownAIEditorWindow window = (TopDownAIEditorWindow)EditorWindow.GetWindow(typeof(TopDownAIEditorWindow));
        window.Show();
        window.minSize = new Vector2(450f, 250f);

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
        EditorGUILayout.LabelField("Setup New AI", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is used to setup new AI characters \nYou should setup your desired model that will be used to setup new AI character.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        aiModel = (GameObject)EditorGUILayout.ObjectField("AI Model:", aiModel, typeof(GameObject), true);
        aiHostile = EditorGUILayout.Toggle("Is AI Hostile?", aiHostile);

        if (aiModel != null) {
            if (GUILayout.Button("Create AI")) {

                GameObject charObject = Instantiate(aiModel);

                if (charObject.GetComponent<Animator>() == null) {
                    Animator charAnimator = charObject.AddComponent<Animator>();
                    RuntimeAnimatorController animController = Resources.Load("TopDownCharacterAnimationController") as RuntimeAnimatorController;
                    charAnimator.runtimeAnimatorController = animController;
                }
                else {
                    RuntimeAnimatorController animController = Resources.Load("TopDownCharacterAnimationController") as RuntimeAnimatorController;
                    charObject.GetComponent<Animator>().runtimeAnimatorController = animController;
                }

                Rigidbody charRigidbody = charObject.AddComponent<Rigidbody>();
                charRigidbody.useGravity = false;
                charRigidbody.isKinematic = false;
                charRigidbody.freezeRotation = true;

                CapsuleCollider charCollider = charObject.AddComponent<CapsuleCollider>();
                charCollider.radius = 0.3f;
                charCollider.height = 1.5f;
                charCollider.center = new Vector3(0f, 0.75f, 0f);

                NavMeshAgent charAgent = charObject.AddComponent<NavMeshAgent>();
                charAgent.speed = 1f;
                charAgent.stoppingDistance = 1.5f;
                charAgent.height = 1.5f;

                TopDownAI ai = charObject.AddComponent<TopDownAI>();

                TopDownEquipmentManager charEquipManager = charObject.AddComponent<TopDownEquipmentManager>();

                if (charObject.transform.Find("[Vision]") == false) {
                    GameObject vision = new GameObject();
                    vision.name = "[Vision]";
                    vision.transform.SetParent(charObject.transform);
                    vision.transform.localPosition = Vector3.zero;
                    vision.layer = 1 << 1;

                    SphereCollider col = vision.AddComponent<SphereCollider>();
                    col.isTrigger = true;
                    col.center = Vector3.zero;
                    col.radius = charObject.GetComponent<TopDownCharacterCard>().aiDetectRadius;
                    col.gameObject.layer = 2;

                    charObject.GetComponent<TopDownCharacterCard>().aiVision = col;

                    //Debug.LogFormat("Setting up <b><color=yellow>Vision collider</color></b> as child of <b><color=red>" + charObject.gameObject.name + "</color></b>.");
                }

                Debug.LogFormat("New AI has been set up. You need to adjust weapon and shield mount and holster points for better visual. You should also setup basic values (name, health, voice set) inside TopDownAI component located on your new AI.");
                Transform[] allChildren = charObject.GetComponentsInChildren<Transform>();
                for (int i = 0; i < allChildren.Length; i++) {
                    if (charEquipManager.weaponMountPoint == null) {
                        if ((allChildren[i].name.Contains("hand") || allChildren[i].name.Contains("HAND") || allChildren[i].name.Contains("Hand") ||
                            (allChildren[i].name.Contains("wrist") || allChildren[i].name.Contains("WRIST") || allChildren[i].name.Contains("Wrist")) &&
                            (allChildren[i].name.Contains("right") || allChildren[i].name.Contains("RIGHT") || allChildren[i].name.Contains("Right")))) {
                            GameObject weaponMount = new GameObject();
                            weaponMount.name = "WEAPON_MOUNTPOINT";
                            weaponMount.transform.SetParent(allChildren[i]);
                            weaponMount.transform.localPosition = Vector3.zero;
                            weaponMount.transform.localEulerAngles = Vector3.zero;
                            charEquipManager.weaponMountPoint = weaponMount.transform;
                        }
                    }
                    if (charEquipManager.shieldMountPoint == null) {
                        if ((allChildren[i].name.Contains("hand") || allChildren[i].name.Contains("HAND") || allChildren[i].name.Contains("Hand") ||
                        (allChildren[i].name.Contains("wrist") || allChildren[i].name.Contains("WRIST") || allChildren[i].name.Contains("Wrist")) &&
                        (allChildren[i].name.Contains("left") || allChildren[i].name.Contains("LEFT") || allChildren[i].name.Contains("Left")))) {
                            GameObject shieldMount = new GameObject();
                            shieldMount.name = "SHIELD_MOUNTPOINT";
                            shieldMount.transform.SetParent(allChildren[i]);
                            shieldMount.transform.localPosition = Vector3.zero;
                            shieldMount.transform.localEulerAngles = Vector3.zero;
                            charEquipManager.shieldMountPoint = shieldMount.transform;
                        }
                    }
                    if (charEquipManager.shieldHolsterMountPoint == null) {
                        if (allChildren[i].name.Contains("spine") || allChildren[i].name.Contains("SPINE") || allChildren[i].name.Contains("Spine")) {
                            GameObject weaponHolsterPoint = new GameObject();
                            weaponHolsterPoint.name = "WEAPON_HOLSTER_MOUNTPOINT";
                            weaponHolsterPoint.transform.SetParent(allChildren[i]);
                            weaponHolsterPoint.transform.localPosition = Vector3.zero;
                            weaponHolsterPoint.transform.localEulerAngles = Vector3.zero;
                            charEquipManager.weaponHolsterMountPoint = weaponHolsterPoint.transform;


                            GameObject shieldHolsterPoint = new GameObject();
                            shieldHolsterPoint.name = "SHIELD_HOLSTER_MOUNTPOINT";
                            shieldHolsterPoint.transform.SetParent(allChildren[i]);
                            shieldHolsterPoint.transform.localPosition = Vector3.zero;
                            shieldHolsterPoint.transform.localEulerAngles = Vector3.zero;
                            charEquipManager.shieldHolsterMountPoint = shieldHolsterPoint.transform;
                        }
                    }
                }

                if (aiHostile == true) {
                    charObject.name = "Top Down Enemy";
                    charObject.tag = "Enemy";
                    charObject.layer = 0;
                    ai.hostile = true;
                }
                else {
                    charObject.name = "Top Down NPC";
                    charObject.tag = "NPC";
                    charObject.layer = 0;
                    ai.hostile = false;
                }

                Selection.activeObject = charObject.gameObject;

                aiModel.SetActive(false);
            }
        }
        else {
            EditorGUILayout.HelpBox("You need to set your AI model before you can create your character.", MessageType.Warning);
        }
    }
}
