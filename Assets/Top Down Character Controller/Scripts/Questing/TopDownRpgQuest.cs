using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum QuestState {
    NotStarted = 1,
    Started = 2,
    Finished = 3,
    Failed = 4,
}

public enum QuestType {
    KillTarget = 1,
    KillAllTargets = 2,
    GoToLocation = 3,
    TalkToNpc = 4,
}

public enum QuestEnding {
    ReturnToNpc = 1,
    EndInPlace = 2,
}

public class TopDownRpgQuest : MonoBehaviour {

    public string questName;
    [TextArea(10, 50)]
    public string questDescription;
    public bool questFinished;
    public QuestState questState;

    public string questCategory;
    public QuestType questType;
    public GameObject questTarget;
    public List<GameObject> questTargets;
    public List<TopDownCharacterCard> questTargetsCards;

    public QuestEnding questEnding;

    public TopDownUIDialog questGiverDialog;
    public string questFinishChoice;
    public string questFinishDialog;
    public UnityEvent questFinishEvent;

    public TopDownRpgQuestHub questHub;
    public TopDownUIManager uiManager;

    private void Start() {
        questHub = TopDownRpgQuestHub.instance;
        questHub.allQuestsInScene.Add(this);

        uiManager = TopDownUIManager.instance;

        if (questTargets.Count > 1) {
            for (int i = 0; i < questTargets.Count; i++) {
                if(questTargets[i].GetComponent<TopDownAI>() == null) {
                    Debug.LogWarningFormat(questTargets[i].gameObject.name + " object from your quest targets list is not an AI!");
                    return;
                }
                questTargetsCards.Add(questTargets[i].GetComponent<TopDownCharacterCard>());
            }
        }
        else {
            if (questTargets.Count == 1) {
                Debug.LogWarning("You have set only one target in questTargets(plural) in " + gameObject.name + " object's quest component. Moving it to questTarget(singular).");
                questTarget = questTargets[0];
                questTargets = new List<GameObject>();
            }
            if (questTarget != null) {
                questTargetsCards.Add(questTarget.GetComponent<TopDownCharacterCard>());
            }
        }

        questFinishEvent.AddListener(this.FinishQuest);

        this.enabled = false;
    }

    public void StartQuest() {
        if (questHub != null && questFinished == false) {

            this.enabled = true;

            questState = QuestState.Started;

            questHub.startedQuests.Add(this);
            questHub.activeQuest = this;

            uiManager.questLog.GetComponent<TopDownRpgQuestLog>().UpdateQuestList();

            //SOME NOTIFICATION THAT QUEST STARTED
            uiManager.notificationText.GetComponent<TopDownUIShowNotification>().ShowNotification("Quest Started \n''" + questName + "''");
        }
    }

    public void StartQuest(TopDownUIDialog dialog) {
        if(questHub != null && questFinished == false) {

            this.enabled = true;
            if (dialog != null) {
                questGiverDialog = dialog;
            }
            else {
                Debug.LogError("You must assign TopDownUIDialog component of quest giver to the dialog event action that fires the StartQuest function of quest: " + questName);
            }

            questState = QuestState.Started;

            questHub.startedQuests.Add(this);
            questHub.activeQuest = this;

            uiManager.questLog.GetComponent<TopDownRpgQuestLog>().UpdateQuestList();

            //SOME NOTIFICATION THAT QUEST STARTED
            uiManager.notificationText.GetComponent<TopDownUIShowNotification>().ShowNotification("Quest Started \n''" + questName + "''");
        }
    }

    public void LateUpdate() {
        if(questState == QuestState.Started) {
            Invoke("CheckQuestStatus", 0.5f);
        }
        else if(questState == QuestState.Finished) {
            this.enabled = false;

            if (questHub.activeQuest == this) {
                questHub.activeQuest = null;
            }
            questHub.startedQuests.Remove(this);
            questHub.finishedQuests.Add(this);
        }
    }

    public void CheckQuestStatus() {
        if (questType == QuestType.KillAllTargets) {
            if (questTargetsCards.Count > 0) {
                for (int i = questTargetsCards.Count - 1; i > -1; i--) {
                    if (questTargetsCards[i] == null) {
                        questTargetsCards.RemoveAt(i);
                        questTargets.RemoveAt(i);
                    }
                }
            }
            else {
                //All targets killed, return to quest giver and report
                if (questEnding == QuestEnding.ReturnToNpc) {
                    if (questGiverDialog != null) {
                        TopDownUIDialogMain.instance.AddNewChoice(this, questGiverDialog, questFinishChoice, questFinishDialog, TopDownUIDialogMain.ChoicePosition.Top);
                        questGiverDialog = null;
                    }
                }
                else if (questEnding == QuestEnding.EndInPlace) {
                }
            }
        }
        else if (questType == QuestType.TalkToNpc) {
        }
    }

    public void FinishQuest() {
        questState = QuestState.Finished;
        questFinished = true;

        uiManager.questLog.GetComponent<TopDownRpgQuestLog>().UpdateQuestList();

        //SOME NOTIFICATION THAT QUEST STARTED
        uiManager.notificationText.GetComponent<TopDownUIShowNotification>().ShowNotification("Quest Finished \n''" + questName + "''");
    }
}
