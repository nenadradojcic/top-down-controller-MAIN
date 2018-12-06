using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(TopDownItemObject))]
[DisallowMultipleComponent]
public class TopDownItemObjectEditor : Editor {

    private bool basicBool;
    private bool typeBool;
    private bool attributesBool;
    private bool visualisationBool;

    private Texture TopDownIcon;

    private TopDownItemObject td_target;

    private void OnEnable() {
        td_target = (TopDownItemObject)target;

        if (TopDownIcon == null) {
            TopDownIcon = Resources.Load("TopDownIcon") as Texture;
        }
    }

    public override void OnInspectorGUI() {

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
        EditorGUILayout.LabelField("Item", boldCenteredLabel);
        EditorGUILayout.HelpBox("This is item base. Here you will set everything about your item that the game will use in gameplay.", MessageType.Info);
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        basicBool = EditorGUILayout.Foldout(basicBool, "Basic Info", simpleStyle);

        if (basicBool == true) {
            EditorGUILayout.LabelField("Item Name:", simpleTitleLable);
            td_target.itemName = EditorGUILayout.TextField(string.Empty, td_target.itemName);
            EditorGUILayout.HelpBox("This is the item name that will be displayed in game.", MessageType.Info);

            EditorGUILayout.LabelField("Item Icon:", simpleTitleLable);
            td_target.itemIcon = (Sprite)EditorGUILayout.ObjectField(string.Empty, td_target.itemIcon, typeof(Sprite), true);
            EditorGUILayout.HelpBox("This is the item icon that will be displayed in game.", MessageType.Info);

            EditorGUILayout.LabelField("Item Stack:", simpleTitleLable);
            td_target.itemStack = EditorGUILayout.IntField(td_target.itemStack);
            EditorGUILayout.HelpBox("This represents how many times will item be slotted into one slot before it will take up another inventory slot.", MessageType.Info);
            
            EditorGUILayout.LabelField("Item Description:", simpleTitleLable);
            td_target.itemDescription = EditorGUILayout.TextField(td_target.itemDescription, GUILayout.MinHeight(90));
            EditorGUILayout.HelpBox("Use this to write description of your item. It will be shown when hovered over item in inventory.", MessageType.Info);
        }
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        typeBool = EditorGUILayout.Foldout(typeBool, "Type Info", simpleStyle);

        if (typeBool == true) {
            EditorGUILayout.LabelField("Item Type:", simpleTitleLable);
            td_target.itemType = (ItemType)EditorGUILayout.EnumPopup(string.Empty, td_target.itemType);
            EditorGUILayout.HelpBox("Determines what kind of item this is.", MessageType.Info);

            if (td_target.itemType == ItemType.Weapon) {
                EditorGUILayout.LabelField("Weapon Type:", simpleTitleLable);
                td_target.weaponType = (WeaponType)EditorGUILayout.EnumPopup(string.Empty, td_target.weaponType);
                EditorGUILayout.HelpBox("Determines what kind of weapon this is.", MessageType.Info);

                if (td_target.weaponType == WeaponType.Melee) {
                    EditorGUILayout.LabelField("Weapon Holding:", simpleTitleLable);
                    td_target.weaponHoldingType = (WeaponHoldingType)EditorGUILayout.EnumPopup(string.Empty, td_target.weaponHoldingType);
                    if (td_target.weaponHoldingType == WeaponHoldingType.None) {
                        EditorGUILayout.HelpBox("Determines how will this weapon be wield \n(This weapon will not be shown as game object).", MessageType.Info);
                    }
                    else if (td_target.weaponHoldingType == WeaponHoldingType.OneHanded) {
                        EditorGUILayout.HelpBox("Determines how will this weapon be wield \n(When this weapon is equiped, player will play ONE HANDED combat animations. He can carry shield with this weapon).", MessageType.Info);
                    }
                    else if (td_target.weaponHoldingType == WeaponHoldingType.TwoHanded) {
                        EditorGUILayout.HelpBox("Determines how will this weapon be wield \n(When this weapon is equiped, player will play TWO HANDED combat animations He can not carry shield with this weapon).", MessageType.Info);
                    }
                }

                EditorGUILayout.LabelField("Damage Modifier:", simpleTitleLable);
                td_target.damageModifier = EditorGUILayout.IntField(string.Empty, td_target.damageModifier);
                EditorGUILayout.HelpBox("Damage modifier represents how much damage this weapon will deal in combat.", MessageType.Info);
            }

            if (td_target.itemType == ItemType.Amulet || td_target.itemType == ItemType.RingL || td_target.itemType == ItemType.RingR || td_target.itemType == ItemType.Chest ||
               td_target.itemType == ItemType.Head || td_target.itemType == ItemType.Feet || td_target.itemType == ItemType.Legs || td_target.itemType == ItemType.Shield) {
                EditorGUILayout.LabelField("Armor Modifier:", simpleTitleLable);
                td_target.armorModifier = EditorGUILayout.IntField(string.Empty, td_target.armorModifier);
                EditorGUILayout.HelpBox("Armor modifier represents how much protection will this character have while in combat.", MessageType.Info);
            }
            if (td_target.itemType == ItemType.Misc || td_target.itemType == ItemType.Scroll || td_target.itemType == ItemType.Potion) {
                EditorGUILayout.HelpBox("This is not implemented yet.", MessageType.Error);
            }
        }
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
        attributesBool = EditorGUILayout.Foldout(attributesBool, "Attribute Modifiers", simpleStyle);

        if(attributesBool == true) {
            td_target.strengthModifier = EditorGUILayout.IntField("Strength Modifier: ", td_target.strengthModifier);
            td_target.dexterityModifier = EditorGUILayout.IntField("Dexterity Modifier: ", td_target.dexterityModifier);
            td_target.constitutionModifier = EditorGUILayout.IntField("Constitution Modifier: ", td_target.constitutionModifier);
            td_target.willpowerModifier = EditorGUILayout.IntField("Willpower Modifier: ", td_target.willpowerModifier);
            EditorGUILayout.HelpBox("Here we you can set if this item will benefit any particular attribute. \n(It can add or deduct from it)", MessageType.Info);
        }
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (td_target.itemType != ItemType.Potion && td_target.itemType != ItemType.Scroll && td_target.itemType != ItemType.Misc) {

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));
            visualisationBool = EditorGUILayout.Foldout(visualisationBool, "Visualisation", simpleStyle);

            if (visualisationBool == true) {
                EditorGUILayout.LabelField("Visualisation:", simpleTitleLable);
                td_target.itemVisualisation = (ItemVisualisation)EditorGUILayout.EnumPopup(string.Empty, td_target.itemVisualisation);
                if (td_target.itemVisualisation == ItemVisualisation.None) {
                    EditorGUILayout.HelpBox("Determines how will this item be visualised on character when equiped. \n(It will not be visualised in any manner.)", MessageType.Info);
                }
                if (td_target.itemType == ItemType.Weapon) {
                    if (td_target.itemVisualisation == ItemVisualisation.InstantiateObject) {
                        td_target.itemGameObject = (GameObject)EditorGUILayout.ObjectField(string.Empty, td_target.itemGameObject, typeof(GameObject), true);
                        EditorGUILayout.HelpBox("Determines how will this item be visualised on character when equiped. \n(It will be instantiated under Weapon Mount Point set in Equipment Manager.)", MessageType.Info);
                    }
                }
                else if (td_target.itemType == ItemType.Shield) {
                    if (td_target.itemVisualisation == ItemVisualisation.InstantiateObject) {
                        td_target.itemGameObject = (GameObject)EditorGUILayout.ObjectField(string.Empty, td_target.itemGameObject, typeof(GameObject), true);
                        EditorGUILayout.HelpBox("Determines how will this item be visualised on character when equiped. \n(It will be instantiated under Shield Mount Point set in Equipment Manager.)", MessageType.Info);
                    }
                }
                if (td_target.itemVisualisation == ItemVisualisation.FindSkinedMesh) {
                    td_target.itemSkinnedMeshName = EditorGUILayout.TextField(td_target.itemSkinnedMeshName, GUILayout.MinHeight(30));
                    EditorGUILayout.HelpBox("Determines how will this item be visualised on character when equiped. \n(Game will look for skinned mesh named as set above under all character children game objects.)", MessageType.Info);
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}
