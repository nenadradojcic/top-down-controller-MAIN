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

    private void Awake() {
        questDescriptionText.text = string.Empty;
    }

    public void UpdateQuestList() {

        foreach (Transform go in questButtonHolder.transform) {
            Destroy(go.gameObject);
            print("Destroying all children in questButtonHolder.");
        }

        instantiatedQuestButtons.Clear();
        print("Cleard instantiatedQuestButtons List");

        if (TopDownRpgQuestHub.instance.startedQuests.Count > 0) {

            print("There are more than 0 started quests.");

            activeQuests = TopDownRpgQuestHub.instance.startedQuests;
            print("We got quests to activeQuests.");

            if (activeQuests.Count > 0) { 
                for (int q = 0; q < activeQuests.Count; q++) {
                    if (activeQuests[q].questFinished == false) { //This we need so we are able to clear instantiated ui button from quest button holder
                        if (instantiatedQuestButtons.Count == 0) {
                            GameObject questButton = Instantiate(questButtonPrefab);
                            questButton.transform.SetParent(questButtonHolder.transform, false);
                            questButton.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questNameText.text = activeQuests[q].questName;
                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questSlotted = activeQuests[q];
                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questDescription = questDescriptionText;

                        if(questDescriptionText.text == string.Empty) {
                            questDescriptionText.text = activeQuests[q].questDescription;
                        }

                        print("Added quest `" + activeQuests[q].questName + "` to log as first one.");
                        }
                        else {
                            GameObject questButton = Instantiate(questButtonPrefab);
                            questButton.transform.SetParent(questButtonHolder.transform, false);
                            questButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -(instantiatedQuestButtons.Count * 14f));

                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questSlotted = activeQuests[q];
                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questDescription = questDescriptionText;

                            print("Added quest `" + activeQuests[q].questName + "` to log as next one.");
                        }

                        instantiatedQuestButtons.Add(activeQuests[q]);
                        print("Aded physical quest button to log for quest `" + activeQuests[q].questName + "`");
                    }
                }
            }
        }
    }
}
