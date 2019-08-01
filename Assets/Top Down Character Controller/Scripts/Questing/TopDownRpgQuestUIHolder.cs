using UnityEngine;
using UnityEngine.UI;

public class TopDownRpgQuestUIHolder : MonoBehaviour {

    public Text questNameText;

    public TopDownRpgQuest questSlotted;

    public Text questDescription;

    public void ButtonActivated() {
        questDescription.text = questSlotted.questDescription;
    }
}
