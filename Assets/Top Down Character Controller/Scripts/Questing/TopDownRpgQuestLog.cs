using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownRpgQuestLog : MonoBehaviour {

    public Text questDescriptionText;

    public RectTransform questButtonHolder;

    public GameObject questTypePrefab;
    public GameObject questButtonPrefab;

    public List<TopDownRpgQuest> activeQuests;
    public List<TopDownRpgQuest> instantiatedQuestButtons;

    public void UpdateQuestList() {
        if (TopDownRpgQuestHub.instance.startedQuests.Count > 0) {

            if (instantiatedQuestButtons.Count > 0) {
                foreach (Transform go in questButtonHolder.transform) {
                    Destroy(go.gameObject);
                }

                instantiatedQuestButtons.Clear();
            }
            
            activeQuests = TopDownRpgQuestHub.instance.startedQuests;
            print(activeQuests.Count.ToString());

            if (activeQuests.Count > 0) {
                for (int q = 0; q < activeQuests.Count; q++) {
                    if (instantiatedQuestButtons.Count == 0) {
                        GameObject questButton = Instantiate(questButtonPrefab);
                        questButton.transform.SetParent(questButtonHolder.transform, false);
                        questButton.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                        questButton.GetComponent<TopDownRpgQuestUIHolder>().questSlotted = activeQuests[q];
                        questButton.GetComponent<TopDownRpgQuestUIHolder>().questDescription = questDescriptionText;
                    }
                    else {
                        GameObject questButton = Instantiate(questButtonPrefab);
                        questButton.transform.SetParent(questButtonHolder.transform, false);
                        questButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -(instantiatedQuestButtons.Count * 14f));

                        questButton.GetComponent<TopDownRpgQuestUIHolder>().questSlotted = activeQuests[q];
                        questButton.GetComponent<TopDownRpgQuestUIHolder>().questDescription = questDescriptionText;
                    }

                    instantiatedQuestButtons.Add(activeQuests[q]);
                }
            }
        }
    }
}
