using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownRpgQuestLog : MonoBehaviour {

    public Text questDescriptionText;

    public RectTransform questButtonHolder;

    public GameObject questCategoryPrefab;
    public GameObject questButtonPrefab;

    public int rows;

    public List<TopDownRpgQuest> activeQuests;
    public List<TopDownRpgQuestUIHolder> instantiatedQuestButtons;

    public List<string> questCategoriesPresent;
    public List<TopDownRpgQuestUITypeHolder> instantiatedQuestCategory;

    private void Awake() {
        questDescriptionText.text = string.Empty;
    }

    public void UpdateQuestList() {

        //We always want to clear all lists and destroy all instantiated objects at first
        foreach (Transform go in questButtonHolder.transform) {
            Destroy(go.gameObject);
        }
        
        instantiatedQuestButtons.Clear();
        questCategoriesPresent.Clear();
        instantiatedQuestCategory.Clear();

        //Then we want to set our quests and categories
        if (TopDownRpgQuestHub.instance.startedQuests.Count > 0) {

            activeQuests = TopDownRpgQuestHub.instance.startedQuests;

            for (int q = 0; q < activeQuests.Count; q++) {
                if (questCategoriesPresent.Count > 0) {
                    if(questCategoriesPresent.Contains(activeQuests[q].questCategory) == true) {
                        //print("Category `" + activeQuests[q].questCategory + "` is already present in the list. We will not add it again.");
                    }
                    else {
                        //print("Category `" + activeQuests[q].questCategory + "` is not present in the list. We will add it.");
                        questCategoriesPresent.Add(activeQuests[q].questCategory);
                    }
                }
                else {
                    questCategoriesPresent.Add(activeQuests[q].questCategory);
                    //print("Added quest" + activeQuests[q].questName + "to log.");
                }
            }

            questCategoriesPresent.Sort();

            //print("activeQuests " + activeQuests.Count);
            //print("questCategoriesPresent " + questCategoriesPresent.Count);
        }

        //Next we are going to instantiate every category and every quest
        for (int category = 0; category < questCategoriesPresent.Count; category++) {

            if (instantiatedQuestButtons.Count <= 0) {
                GameObject questCategory = Instantiate(questCategoryPrefab);
                questCategory.transform.SetParent(questButtonHolder.transform, false);
                questCategory.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                questCategory.GetComponent<TopDownRpgQuestUITypeHolder>().questTypeText.text = questCategoriesPresent[category];
                instantiatedQuestCategory.Add(questCategory.GetComponent<TopDownRpgQuestUITypeHolder>());

                rows++;
            }
            else {
                GameObject questCategory = Instantiate(questCategoryPrefab);
                questCategory.transform.SetParent(questButtonHolder.transform, false);
                questCategory.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -(rows * 14f));

                questCategory.GetComponent<TopDownRpgQuestUITypeHolder>().questTypeText.text = questCategoriesPresent[category];
                instantiatedQuestCategory.Add(questCategory.GetComponent<TopDownRpgQuestUITypeHolder>());

                rows++;
            }

            for (int quest = 0; quest < activeQuests.Count; quest++) {
                if (questCategoriesPresent[category] == activeQuests[quest].questCategory) {
                    if (activeQuests[quest].questFinished == false) { //This we need so we are able to clear instantiated ui button from quest button holder
                        if (instantiatedQuestButtons.Count == 0) {
                            GameObject questButton = Instantiate(questButtonPrefab);
                            questButton.transform.SetParent(questButtonHolder.transform, false);
                            if (activeQuests[quest].questCategory == string.Empty) { //Here we want to move quest button one slot up because it has no category set and will have empty space above
                                rows--;
                            }
                            questButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -(rows * 14f));

                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questNameText.text = activeQuests[quest].questName;
                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questSlotted = activeQuests[quest];
                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questDescription = questDescriptionText;

                            if (questDescriptionText.text == string.Empty) {
                                questDescriptionText.text = activeQuests[quest].questDescription;
                            }
                            instantiatedQuestButtons.Add(questButton.GetComponent<TopDownRpgQuestUIHolder>());

                            rows++;
                        }
                        else {
                            GameObject questButton = Instantiate(questButtonPrefab);
                            questButton.transform.SetParent(questButtonHolder.transform, false);
                            questButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -(rows * 14f));

                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questNameText.text = activeQuests[quest].questName;
                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questSlotted = activeQuests[quest];
                            questButton.GetComponent<TopDownRpgQuestUIHolder>().questDescription = questDescriptionText;

                            instantiatedQuestButtons.Add(questButton.GetComponent<TopDownRpgQuestUIHolder>());

                            rows++;
                        }
                    }
                }
            }
        }

        questDescriptionText.text = string.Empty;
        rows = 0;
    }
}
