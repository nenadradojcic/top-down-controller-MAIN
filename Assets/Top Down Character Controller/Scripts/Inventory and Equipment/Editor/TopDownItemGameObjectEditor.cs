using UnityEngine;
using UnityEditor;

public class TopDownItemGameObjectEditor {

    [MenuItem("GameObject/TopDownRpg/New Item", false, 10)]

    static void CreateCustomGameObject(MenuCommand menuCommand) {
        
        GameObject item = GameObject.Instantiate(Resources.Load("TD_ItemWorldObjectPrefab", typeof(GameObject))) as GameObject;
        item.name = "New Item";

        GameObjectUtility.SetParentAndAlign(item, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(item, "Create " + item.name);

        Selection.activeObject = item;
    }
}
