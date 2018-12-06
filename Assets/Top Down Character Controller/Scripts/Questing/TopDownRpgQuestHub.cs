using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownRpgQuestHub : MonoBehaviour {

    public List<TopDownRpgQuest> allQuestsInScene = new List<TopDownRpgQuest>();
    public List<TopDownRpgQuest> startedQuests = new List<TopDownRpgQuest>();
    public List<TopDownRpgQuest> finishedQuests = new List<TopDownRpgQuest>();
    public List<TopDownRpgQuest> failedQuests = new List<TopDownRpgQuest>();

    public TopDownRpgQuest activeQuest;

    public static TopDownRpgQuestHub instance;

    private void Awake() {
        instance = this;
    }
}
