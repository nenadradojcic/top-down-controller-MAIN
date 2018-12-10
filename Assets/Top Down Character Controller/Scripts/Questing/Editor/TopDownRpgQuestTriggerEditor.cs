using UnityEditor;
using UnityEngine;

public class TopDownRpgQuestTriggerEditor {

    [MenuItem("GameObject/TopDownRpg/Quest/Quest Starter Trigger", false, 20)]

    static void CreateCustomGameObject(MenuCommand menuCommand) {

        GameObject trigger = new GameObject();
        trigger.layer = 4;
        trigger.name = "New Quest Trigger Starter";
        trigger.AddComponent<TopDownRpgQuestStartOnTrigger>();
        trigger.GetComponent<SphereCollider>().isTrigger = true;

        GameObjectUtility.SetParentAndAlign(trigger, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(trigger, "Create " + trigger.name);

        Selection.activeObject = trigger;
    }
}
