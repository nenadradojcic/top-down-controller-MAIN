using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum QuestState {
    NONE = 0,
    NotStarted = 1,
    Started = 2,
    Finished = 3,
    Failed = 4,
}

public enum QuestType {
    NONE = 0,
    KillTargets = 1,
    GoToLocation = 2,
    TalkToNpc = 3,
}

public enum QuestEnding {
    NONE = 0,
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
    public DialogType questFinishDialogType;
    public string questFinishChoice;
    public string questFinishDialog;
    public UnityEvent questStartEvents;
    public UnityEvent questFinishEvents;

    public TopDownRpgQuestHub questHub;
    public TopDownUIManager uiManager;

    private void Start() {
        questHub = TopDownRpgQuestHub.instance;
        questHub.allQuestsInScene.Add(this);

        uiManager = TopDownUIManager.instance;

        if (questTargets.Count > 0) {
            for (int i = 0; i < questTargets.Count; i++) {
                if (questTargets[i].GetComponent<TopDownAI>() == null) {
                    Debug.LogWarningFormat(questTargets[i].gameObject.name + " object from your quest targets list is not an AI!");
                    return;
                }
                else {
                    questTargetsCards.Add(questTargets[i].GetComponent<TopDownCharacterCard>());
                }
            }
        }

        questFinishEvents.AddListener(this.FinishQuest);

        if(questState == QuestState.Started) {
            StartQuest();
        }

        this.enabled = false;
    }

    public void StartQuest() {
        if (questHub != null && questState != QuestState.Started && questFinished == false) {

            this.enabled = true;

            questState = QuestState.Started;

            questHub.startedQuests.Add(this);
            questHub.activeQuest = this;

            uiManager.questLog.GetComponent<TopDownRpgQuestLog>().UpdateQuestList();

            questStartEvents.Invoke();

            //SOME NOTIFICATION THAT QUEST STARTED
            uiManager.notificationText.GetComponent<TopDownUIShowNotification>().ShowNotification("Quest Started \n''" + questName + "''");
        }
    }

    public void StartQuest(TopDownUIDialog dialog) {
        if(questHub != null && questState != QuestState.Started && questFinished == false) {

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

            questStartEvents.Invoke();

            //SOME NOTIFICATION THAT QUEST STARTED
            uiManager.notificationText.GetComponent<TopDownUIShowNotification>().ShowNotification("Quest Started \n''" + questName + "''");
        }
    }

    public void LateUpdate() {
        if(questState == QuestState.Started) {
            CheckQuestStatus();
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
        if (questType == QuestType.KillTargets) {
            if (questTargetsCards.Count > 0) {
                for (int i = 0; i < questTargetsCards.Count; i++) {
                    if (questTargetsCards[i] == null) {
                        questTargets.RemoveAt(i);
                        questTargetsCards.RemoveAt(i);
                    }
                }
            }
            else {
                //All targets killed, return to quest giver and report
                if (questEnding == QuestEnding.ReturnToNpc) {
                    if (questGiverDialog != null) {
                        TopDownUIDialogMain.instance.AddNewChoice(this, questGiverDialog, questFinishChoice, questFinishDialog, questFinishDialogType, TopDownUIDialogMain.ChoicePosition.Top);
                        questGiverDialog = null;
                    }
                }
                else if (questEnding == QuestEnding.EndInPlace) {
                    FinishQuest();
                }
            }
        }
        else if (questType == QuestType.TalkToNpc) {
        }
        else if (questType == QuestType.GoToLocation) {
            float dist = Vector3.Distance(TopDownCharacterManager.instance.controllingCharacter.transform.position, questTarget.transform.position);
            if (dist <= 4) {
                if (questEnding == QuestEnding.ReturnToNpc) {
                    if (questGiverDialog != null) {
                        TopDownUIDialogMain.instance.AddNewChoice(this, questGiverDialog, questFinishChoice, questFinishDialog, questFinishDialogType, TopDownUIDialogMain.ChoicePosition.Top);
                        questGiverDialog = null;
                    }
                }
                else if (questEnding == QuestEnding.EndInPlace) {
                }
            }
        }
    }

    public void FinishQuest() {

        //SOME NOTIFICATION THAT QUEST FINISHED
        uiManager.notificationText.GetComponent<TopDownUIShowNotification>().ShowNotification("Quest Finished \n''" + questName + "''");

        questFinished = true;
        questState = QuestState.Finished;

        uiManager.questLog.GetComponent<TopDownRpgQuestLog>().UpdateQuestList();
    }
}
