using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class TopDownSetupCharacterEditorWindow : EditorWindow {

    private static Texture TopDownIcon;

    private GameObject characterModel;
    private TopDownCharacter characterAsset;

    [MenuItem("Top Down RPG/New Player Character  %#c", false, 2)]
    static void Init() {
        TopDownSetupCharacterEditorWindow window = (TopDownSetupCharacterEditorWindow)EditorWindow.GetWindow(typeof(TopDownSetupCharacterEditorWindow));
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
        EditorGUILayout.LabelField("Setup New Character", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is used to setup new player characters \nYou should setup your desired model and desired character info asset that will be used to setup new player character.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        characterModel = (GameObject)EditorGUILayout.ObjectField("Character Model:", characterModel, typeof(GameObject), true);
        characterAsset = (TopDownCharacter)EditorGUILayout.ObjectField("Character Asset:", characterAsset, typeof(TopDownCharacter), true);

        if(characterAsset == null) {
            if(GUILayout.Button("Create new character card")) {
                TopDownCharacter charCard = new TopDownCharacter();
                AssetDatabase.CreateAsset(charCard, "Assets/New Character Card.asset");
                characterAsset = charCard;
            }
        }

        if (characterModel != null && characterAsset != null) {
            if (GUILayout.Button("Create Character")) {

                #region CREATE CHARACTER BUTTON FUNCTIONS
                //GameObject charObject = new GameObject();
                GameObject charObject = Instantiate(characterModel);

                charObject.name = "Top Down Character Controller";

                charObject.tag = "Player";
                charObject.layer = 8;

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
                charAgent.stoppingDistance = 0.2f;
                charAgent.height = 1.5f;

                charObject.AddComponent<TopDownControllerMain>();

                TopDownControllerInteract charInteract = charObject.AddComponent<TopDownControllerInteract>();
                charInteract.noFocusWalkPoint = Resources.Load("TD_MoveTargetParticle") as GameObject;

                TopDownEquipmentManager charEquipManager = charObject.AddComponent<TopDownEquipmentManager>();
                //charEquipManager.itemsOnCharacter = charObject.GetComponentsInChildren<SkinnedMeshRenderer>();

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

                TopDownCharacterCard charCard = charObject.AddComponent<TopDownCharacterCard>();
                charCard.character = characterAsset;

                GameObject goCamTmp = new GameObject();
                goCamTmp.name = "Profile Camera";
                Camera cam = goCamTmp.AddComponent<Camera>();
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.black;
                cam.orthographic = false;
                cam.fieldOfView = 40f;
                cam.nearClipPlane = 0.15f;
                cam.farClipPlane = 0.9f;
                cam.depth = 10;

                GameObject goLightTmp = new GameObject();
                goLightTmp.transform.SetParent(cam.transform);
                goLightTmp.transform.localPosition = Vector3.zero;
                Light light = goLightTmp.AddComponent<Light>();
                light.type = LightType.Point;
                light.range = 10f;
                light.intensity = 0.5f;
                light.bounceIntensity = 0f;

                for (int i = 0; i < allChildren.Length; i++) {
                    if (allChildren[i].name.Contains("head") || allChildren[i].name.Contains("HEAD") || allChildren[i].name.Contains("Head")) {
                        goCamTmp.transform.SetParent(allChildren[i].transform);
                        Debug.Log("Setting up camera for Runtime Profile support. Be sure to check that camera game object is set as a child of Head Bone of your characters skeleton and to adjust it to your liking.");
                        break;
                    }
                }

                charCard.portraitCamera = cam;
                charCard.portraitLight = light;

                if (charObject.GetComponent<TopDownAI>()) {
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

                        charObject.GetComponent<TopDownCharacterCard>().aiVision = col;

                        //Debug.LogFormat("Setting up <b><color=yellow>Vision collider</color></b> as child of <b><color=red>" + charObject.gameObject.name + "</color></b>.");
                    }
                }
                else if (charObject.GetComponent<TopDownControllerInteract>()) {
                    if (charObject.transform.Find("TD_CharacterCamera") == false) {
                        GameObject tmp = Resources.Load("TD_CharacterCamera") as GameObject;
                        GameObject inventoryCamera = Instantiate(tmp);
                        inventoryCamera.name = "TD_CharacterCamera";
                        inventoryCamera.transform.SetParent(charObject.transform);
                        inventoryCamera.transform.localPosition = new Vector3(0f, 0.9f, 2f);
                        inventoryCamera.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

                        charObject.GetComponent<TopDownCharacterCard>().inventoryCamera = inventoryCamera;
                        //Debug.LogFormat("Setting up <b><color=yellow>Inventory Camera</color></b> as a child of <b><color=blue>" + charObject.gameObject.name + "</color></b> player character.");
                    }

                    if (charObject.gameObject.layer == 8) {
                        TopDownControllerInteract[] allCharacters = GameObject.FindObjectsOfType<TopDownControllerInteract>();

                        for (int i = 0; i < allCharacters.Length; i++) {
                            if (charObject.gameObject.layer == allCharacters[i].gameObject.layer) {
                                if (charObject.name != allCharacters[i].name) {
                                    //Debug.LogFormat("You need to change layer of <b><color=yellow>" + charObject.gameObject.name + "</color></b> because <b><color=blue>" + allCharacters[i].name + "</color></b> is already set to that layer. If you do not change it <b><color=red>charObject.inventoryCamera</color></b> will render both characters.");
                                }
                            }
                        }
                    }
                }

                characterModel.SetActive(false);

                Debug.LogFormat("New Character has been set up. You need to adjust weapon and shield mount and holster points for better visual.");
            }
            #endregion

        }
        else {
            EditorGUILayout.HelpBox("You need to set your characters model and character card before you can create your character.", MessageType.Warning);
        }
    }
}