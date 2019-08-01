using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TopDownRpgQuestStartOnTrigger : MonoBehaviour {

    public TopDownRpgQuest questToStart;

    public void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            if (questToStart != null) {
                if (questToStart.questState == QuestState.NotStarted) {
                    questToStart.StartQuest();
                }
                Destroy(gameObject);
            }
            else {
                Debug.LogWarning("Quest To Start not set in component.");
            }
        }
    }
}
