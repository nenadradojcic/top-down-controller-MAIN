using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TopDownUIDialogChoice : MonoBehaviour {

    public Text dialogTxt;
    public Text choiceTxt;

    public string dialog;
    public DialogType type;
    public bool remove;
    public TopDownUIDialog branch;
    public string response;
    public UnityEvent events;

    public int index;

    public TopDownUIDialogMain dialogMain;

    public void Start() {
        dialogMain = TopDownUIDialogMain.instance;
    }

    public void ShowChoiceDialog() {

        TopDownUIDialog dialogInUse = dialogMain.dialogInUse;

        dialogTxt.text = dialog;
        if (events != null) {
            events.Invoke();
        }

        switch(type) {
            case DialogType.BranchDialog:
                StartCoroutine(SetupBranchingDialog());
                break;
            case DialogType.CloseDialog:
                TopDownUIDialogMain.instance.CloseDialogPerType("[Exit]");
                break;
            case DialogType.ImmediateClose:
                CloseTheDialog();
                break;
            case DialogType.CombatDialog:
                if (dialogInUse.GetComponent<TopDownAI>()) {
                    dialogInUse.GetComponent<TopDownAI>().TurnHostile();
                }
                CloseTheDialog();
                break;
            default:

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
                                choice[i].branch = choice[i + 1].branch;
                                choice[i].response = choice[i + 1].response;
                                choice[i].events = choice[i + 1].events;

                                choice[i + 1].GetComponent<Button>().interactable = false;
                                choice[i + 1].choiceTxt.text = string.Empty;

                                choice[i + 1].dialog = string.Empty;
                                choice[i + 1].type = DialogType.None;
                                choice[i + 1].remove = false;
                                choice[i + 1].branch = null;
                                choice[i + 1].response = string.Empty;
                                choice[i + 1].events = null;

                                switch (i) {
                                   case 0:
                                        dialogInUse.choiceOne = choice[i].choiceTxt.text;
                                        dialogInUse.choiceOneDialog = choice[i].dialog;
                                        dialogInUse.choiceOneType = choice[i].type;
                                        dialogInUse.removeOnChoiceOne = choice[i].remove;
                                        dialogInUse.branchOneDialog = choice[i].branch;
                                        dialogInUse.typeOneResponse = choice[i].response;
                                        dialogInUse.choiceOneEvent = choice[i].events;

                                        dialogInUse.choiceTwo = string.Empty;
                                        dialogInUse.choiceTwoDialog = string.Empty;
                                        dialogInUse.choiceTwoType = DialogType.None;
                                        dialogInUse.removeOnChoiceTwo = false;
                                        dialogInUse.branchTwoDialog = choice[i].branch;
                                        dialogInUse.typeTwoResponse = string.Empty;
                                        dialogInUse.choiceTwoEvent = null;
                                        break;
                                    case 1:
                                        dialogInUse.choiceTwo = choice[i].choiceTxt.text;
                                        dialogInUse.choiceTwoDialog = choice[i].dialog;
                                        dialogInUse.choiceTwoType = choice[i].type;
                                        dialogInUse.removeOnChoiceTwo = choice[i].remove;
                                        dialogInUse.branchTwoDialog = choice[i].branch;
                                        dialogInUse.typeTwoResponse = choice[i].response;
                                        dialogInUse.choiceTwoEvent = choice[i].events;

                                        dialogInUse.choiceThree = string.Empty;
                                        dialogInUse.choiceThreeDialog = string.Empty;
                                        dialogInUse.choiceThreeType = DialogType.None;
                                        dialogInUse.removeOnChoiceThree = false;
                                        dialogInUse.branchThreeDialog = choice[i].branch;
                                        dialogInUse.typeThreeResponse = string.Empty;
                                        dialogInUse.choiceThreeEvent = null;
                                        break;
                                    case 2:
                                        dialogInUse.choiceThree = choice[i].choiceTxt.text;
                                        dialogInUse.choiceThreeDialog = choice[i].dialog;
                                        dialogInUse.choiceThreeType = choice[i].type;
                                        dialogInUse.removeOnChoiceThree = choice[i].remove;
                                        dialogInUse.branchThreeDialog = choice[i].branch;
                                        dialogInUse.typeThreeResponse = choice[i].response;
                                        dialogInUse.choiceThreeEvent = choice[i].events;

                                        dialogInUse.choiceFour = string.Empty;
                                        dialogInUse.choiceFourDialog = string.Empty;
                                        dialogInUse.choiceFourType = DialogType.None;
                                        dialogInUse.removeOnChoiceFour = false;
                                        dialogInUse.branchFourDialog = choice[i].branch;
                                        dialogInUse.typeFourResponse = string.Empty;
                                        dialogInUse.choiceFourEvent = null;
                                        break;
                                    case 3:
                                        dialogInUse.choiceFour = string.Empty;
                                        dialogInUse.choiceFourDialog = string.Empty;
                                        dialogInUse.choiceFourType = DialogType.None;
                                        dialogInUse.removeOnChoiceFour = false;
                                        dialogInUse.branchFourDialog = choice[i].branch;
                                        dialogInUse.typeFourResponse = string.Empty;
                                        dialogInUse.choiceFourEvent = null;
                                        dialogInUse.choiceFour = string.Empty;
                                        break;
                                    /*case 4:
                                        dialogInUse.choiceFour = string.Empty;
                                        dialogInUse.choiceFourDialog = string.Empty;
                                        dialogInUse.choiceFourType = DialogType.None;
                                        dialogInUse.removeOnChoiceFour = false;
                                        dialogInUse.branchFourDialog = choice[i].branch;
                                        dialogInUse.typeFourResponse = string.Empty;
                                        dialogInUse.choiceFourEvent = null;
                                        break;*/
                                }
                            }
                        }
                    }
                }
            }
                break;
        }
    }

    public void CloseTheDialog() {
        dialogMain.ClearDialog();

        TopDownUIManager.instance.SetUIState(TopDownUIManager.instance.dialog);
        TopDownUIManager.instance.SetUIState(TopDownUIManager.instance.uiHolder);

        TopDownCharacterManager.instance.controllingCharacter.GetComponent<TopDownControllerInteract>().tempDisable = false;

    }

    IEnumerator SetupBranchingDialog() {

        string dialogTmp = dialog;
        
        branch.branchedFrom = dialogMain.dialogInUse;
        dialogMain.ClearDialog();

        yield return new WaitForEndOfFrame();

        dialogTxt.text = dialogTmp;
        dialogMain.ShowDialog(branch);
    }
}
