using UnityEngine;
using UnityEngine.UI;

public class TopDownRpgQuestUITypeHolder : MonoBehaviour {

    public Text questTypeText;

    public void Start() {
        if(questTypeText.text != null) {
            string formatedText = "~ " + questTypeText.text + " ~";
            questTypeText.text = formatedText;
        }
    }
}
