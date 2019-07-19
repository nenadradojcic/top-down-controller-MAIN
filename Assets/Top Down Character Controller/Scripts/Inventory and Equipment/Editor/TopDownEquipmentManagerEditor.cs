using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopDownEquipmentManager))]
[DisallowMultipleComponent]
public class TopDownEquipmentManagerEditor : Editor {

    private Texture TopDownIcon;

    private TopDownEquipmentManager td_target;

    private void OnEnable() {
        td_target = (TopDownEquipmentManager)target;

        if (TopDownIcon == null) {
            TopDownIcon = Resources.Load("TopDownIcon") as Texture;
        }
    }

    public override void OnInspectorGUI() {

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
        EditorGUILayout.LabelField("Equipment Manager", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is our equipment manager. Here we will set all needed parameters so our characters can equip new equipment and weapons, and also we will use" +
            "set variables to determine our character stats and looks.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        EditorGUILayout.LabelField("- Equipment Info -", boldCenteredLabel);
        EditorGUILayout.HelpBox("Here You can preview your character equipment status.", MessageType.Info);
        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(true);
        td_target.weaponTypeUsed = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type Used:", td_target.weaponTypeUsed);
        td_target.weaponHoldingType = (WeaponHoldingType)EditorGUILayout.EnumPopup("Weapon Type Used:", td_target.weaponHoldingType);
        td_target.armorPointsValue = EditorGUILayout.IntField("Armor Points: ", td_target.armorPointsValue);
        td_target.damagePointsValue = EditorGUILayout.IntField("Damage Points: ", td_target.damagePointsValue);
        EditorGUILayout.Space();
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

        if (td_target.tag == "Player" || td_target.GetComponent<TopDownControllerInteract>()) {
            EditorGUILayout.LabelField("- Equipment Settings -", boldCenteredLabel);
            EditorGUILayout.HelpBox("This are weapon/shield hold and holster points. Be sure to align them to your character models.", MessageType.Info);
            td_target.weaponMountPoint = (Transform)EditorGUILayout.ObjectField("Weapon Mount Point:", td_target.weaponMountPoint, typeof(Transform), true);
            td_target.shieldMountPoint = (Transform)EditorGUILayout.ObjectField("Shield Mount Point:", td_target.shieldMountPoint, typeof(Transform), true);
            td_target.weaponHolsterMountPoint = (Transform)EditorGUILayout.ObjectField("Weapon Holster Point:", td_target.weaponHolsterMountPoint, typeof(Transform), true);
            td_target.shieldHolsterMountPoint = (Transform)EditorGUILayout.ObjectField("Shield Holster Point:", td_target.shieldHolsterMountPoint, typeof(Transform), true);

            EditorGUILayout.HelpBox("Equipment Type Used will determine in which way are equipment shown on character.\n" +
                                    "\n1. Instantiate Mesh will instantiate gameObject at runtime as child of set transform.\n" +
                                    "\n2. Replace Mesh will replace mesh in SkinnedMesh Component. Best used if all meshes use the same material.\n" +
                                    "\n3. Enable Mesh will search for specified mesh name already part of character model to enable/disable it.\n", MessageType.Info);
            td_target.characterEquipmentType = (CharacterEquipementType)EditorGUILayout.EnumPopup("Equipment Type Used:", td_target.characterEquipmentType);

            if (td_target.characterEquipmentType == CharacterEquipementType.InstantiateMesh) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Equipment Points", boldCenteredLabel);
                td_target.bodyTransform = (Transform)EditorGUILayout.ObjectField("Body Parent:", td_target.bodyTransform, typeof(Transform), true);
                td_target.headTransform = (Transform)EditorGUILayout.ObjectField("Head Parent:", td_target.headTransform, typeof(Transform), true);
                td_target.neckTransform = (Transform)EditorGUILayout.ObjectField("Neck Parent:", td_target.neckTransform, typeof(Transform), true);
                td_target.handsTransform = (Transform)EditorGUILayout.ObjectField("Hands Parent:", td_target.handsTransform, typeof(Transform), true);
                td_target.leggsTransform = (Transform)EditorGUILayout.ObjectField("Leggs Parent:", td_target.leggsTransform, typeof(Transform), true);
                td_target.helmTransform = (Transform)EditorGUILayout.ObjectField("Helm Parent:", td_target.helmTransform, typeof(Transform), true);

                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else if (td_target.characterEquipmentType == CharacterEquipementType.ReplaceMesh) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Base Body Meshes", boldCenteredLabel);
                td_target.defaultBodyMesh = (Mesh)EditorGUILayout.ObjectField("Base Body Mesh:", td_target.defaultBodyMesh, typeof(Mesh), true);
                td_target.defaultHandsMesh = (Mesh)EditorGUILayout.ObjectField("Base Hands Mesh:", td_target.defaultHandsMesh, typeof(Mesh), true);
                td_target.defaultLeggsMesh = (Mesh)EditorGUILayout.ObjectField("Base Legs Mesh:", td_target.defaultLeggsMesh, typeof(Mesh), true);

                EditorGUILayout.LabelField("Equipment Skinned Meshes", boldCenteredLabel);
                td_target.bodyMesh = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Body Skinned Mesh:", td_target.bodyMesh, typeof(SkinnedMeshRenderer), true);
                td_target.headMesh = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Head Skinned Mesh:", td_target.headMesh, typeof(SkinnedMeshRenderer), true);
                td_target.neckMesh = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Neck Skinned Mesh:", td_target.neckMesh, typeof(SkinnedMeshRenderer), true);
                td_target.handsMesh = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Hands Skinned Mesh:", td_target.handsMesh, typeof(SkinnedMeshRenderer), true);
                td_target.leggsMesh = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Leggs Skinned Mesh:", td_target.leggsMesh, typeof(SkinnedMeshRenderer), true);
                td_target.helmMesh = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Helm Skinned Mesh:", td_target.helmMesh, typeof(SkinnedMeshRenderer), true);

                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else if (td_target.characterEquipmentType == CharacterEquipementType.EnableMesh) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemsOnCharacter"), true);

                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
