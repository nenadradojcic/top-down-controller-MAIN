using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TopDownUIDialogChoice : MonoBehaviour {

    public Text dialogTxt;
    public Text choiceTxt;

    public string dialog;
    public DialogType type;
    public bool remove;
    public string response;
    public UnityEvent events;

    public int index;

    public TopDownUIDialogMain dialogMain;

    public void Start() {
        dialogMain = TopDownUIDialogMain.instance;
    }

    public void ShowChoiceDialog() {
        dialogTxt.text = dialog;
        if (events != null) {
            events.Invoke();
        }

        if(type == DialogType.CloseDialog) {
            TopDownUIDialogMain.instance.CloseDialogPerType("Of we go then!");
        }
        else if(type == DialogType.ImmediateClose) {
            CloseTheDialog();
        }

        if (type == DialogType.BranchDialog) {
            //Here we close current dialog and open one from branch
        }
        else {
            if(remove == true) {
                TopDownUIDialogChoice[] choice = dialogMain.dialogChoices;

                for (int i = 0; i < choice.Length; i++) {
                    if ((i+1) >= index) {
                        if ((i+1) < choice.Length) {
                            if (choice[i + 1].GetComponent<Button>().interactable == true) {
                                choice[i].GetComponent<Button>().interactable = choice[i + 1].GetComponent<Button>().interactable;
                                choice[i].choiceTxt.text = choice[i + 1].choiceTxt.text;

                                choice[i].dialog = choice[i + 1].dialog;
                                choice[i].type = choice[i + 1].type;
                                choice[i].remove = choice[i + 1].remove;
                                choice[i].response = choice[i + 1].response;
                                choice[i].events = choice[i + 1].events;

                                choice[i + 1].GetComponent<Button>().interactable = false;
                                choice[i + 1].choiceTxt.text = string.Empty;

                                choice[i + 1].dialog = string.Empty;
                                choice[i + 1].type = DialogType.None;
                                choice[i + 1].remove = false;
                                choice[i + 1].response = string.Empty;
                                choice[i + 1].events = null;

                                if(i == 0) {
                                    dialogMain.dialogInUse.choiceOne = choice[i].choiceTxt.text;
                                    dialogMain.dialogInUse.choiceOneDialog = choice[i].dialog;
                                    dialogMain.dialogInUse.choiceOneType = choice[i].type;
                                    dialogMain.dialogInUse.removeOnChoiceOne = choice[i].remove;
                                    dialogMain.dialogInUse.typeOneResponse = choice[i].response;
                                    dialogMain.dialogInUse.choiceOneEvent = choice[i].events;
                                }
                                if (i == 1) {
                                    dialogMain.dialogInUse.choiceTwo = choice[i].choiceTxt.text;
                                    dialogMain.dialogInUse.choiceTwoDialog = choice[i].dialog;
                                    dialogMain.dialogInUse.choiceTwoType = choice[i].type;
                                    dialogMain.dialogInUse.removeOnChoiceTwo = choice[i].remove;
                                    dialogMain.dialogInUse.typeTwoResponse = choice[i].response;
                                    dialogMain.dialogInUse.choiceTwoEvent = choice[i].events;

                                    dialogMain.dialogInUse.choiceThree = string.Empty;
                                    dialogMain.dialogInUse.choiceThreeDialog = string.Empty;
                                    dialogMain.dialogInUse.choiceThreeType = DialogType.None;
                                    dialogMain.dialogInUse.removeOnChoiceThree = false;
                                    dialogMain.dialogInUse.typeThreeResponse = string.Empty;
                                    dialogMain.dialogInUse.choiceThreeEvent = null;
                                }
                                if (i == 2) {
                                    dialogMain.dialogInUse.choiceThree = choice[i].choiceTxt.text;
                                    dialogMain.dialogInUse.choiceThreeDialog = choice[i].dialog;
                                    dialogMain.dialogInUse.choiceThreeType = choice[i].type;
                                    dialogMain.dialogInUse.removeOnChoiceThree = choice[i].remove;
                                    dialogMain.dialogInUse.typeThreeResponse = choice[i].response;
                                    dialogMain.dialogInUse.choiceThreeEvent = choice[i].events;

                                    dialogMain.dialogInUse.choiceFour = string.Empty;
                                    dialogMain.dialogInUse.choiceFourDialog = string.Empty;
                                    dialogMain.dialogInUse.choiceFourType = DialogType.None;
                                    dialogMain.dialogInUse.removeOnChoiceFour = false;
                                    dialogMain.dialogInUse.typeFourResponse = string.Empty;
                                    dialogMain.dialogInUse.choiceFourEvent = null;
                                }
                                if (i == 3) {
                                    dialogMain.dialogInUse.choiceFour = choice[i].choiceTxt.text;
                                    dialogMain.dialogInUse.choiceFourDialog = choice[i].dialog;
                                    dialogMain.dialogInUse.choiceFourType = choice[i].type;
                                    dialogMain.dialogInUse.removeOnChoiceFour = choice[i].remove;
                                    dialogMain.dialogInUse.typeFourResponse = choice[i].response;
                                    dialogMain.dialogInUse.choiceFourEvent = choice[i].events;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void CloseTheDialog() {
        TopDownUIDialogMain.instance.ClearDialog();

        TopDownUIManager.instance.SetUIState(TopDownUIManager.instance.dialog);
        TopDownUIManager.instance.SetUIState(TopDownUIManager.instance.uiHolder);

        TopDownCharacterManager.instance.activeCharacter.GetComponent<TopDownControllerInteract>().tempDisable = false;

    }
}
