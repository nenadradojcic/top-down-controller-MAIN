using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DialogType {
    None = 0,
    CloseDialog = 1,
    ImmediateClose = 2,
    BranchDialog = 3,
    CombatDialog = 4,
}

public class TopDownUIDialog : TopDownInteractible {
    
    public DialogCameraPosition cameraPosition;

    public string welcomeDialog;

    public bool showChoiceOne;
    public string choiceOne;
    public string choiceOneDialog;
    public DialogType choiceOneType;
    public bool removeOnChoiceOne;
    public TopDownUIDialog branchOneDialog;
    public string typeOneResponse;
    public UnityEvent choiceOneEvent;

    public bool showChoiceTwo;
    public string choiceTwo;
    public string choiceTwoDialog;
    public DialogType choiceTwoType;
    public bool removeOnChoiceTwo;
    public TopDownUIDialog branchTwoDialog;
    public string typeTwoResponse;
    public UnityEvent choiceTwoEvent;

    public bool showChoiceThree;
    public string choiceThree;
    public string choiceThreeDialog;
    public DialogType choiceThreeType;
    public bool removeOnChoiceThree;
    public TopDownUIDialog branchThreeDialog;
    public string typeThreeResponse;
    public UnityEvent choiceThreeEvent;

    public bool showChoiceFour;
    public string choiceFour;
    public string choiceFourDialog;
    public DialogType choiceFourType;
    public bool removeOnChoiceFour;
    public TopDownUIDialog branchFourDialog;
    public string typeFourResponse;
    public UnityEvent choiceFourEvent;

    public int numberOfChoices = 0;
    
    public TopDownUIDialog branchedFrom;

    private void Reset() {
        interactDistance = 2f;
    }

    private void Awake() {

        interactDistance = 2.5f;

        if (choiceOne != string.Empty) {
            numberOfChoices = 1;
        }
        else {
            return;
        }
        if (choiceTwo != string.Empty) {
            numberOfChoices = 2;
        }
        else {
            return;
        }
        if (choiceThree != string.Empty) {
            numberOfChoices = 3;
        }
        else {
            return;
        }
        if (choiceFour != string.Empty) {
            numberOfChoices = 4;
        }
        else {
            return;
        }
    }

    private void Start() {

        if(choiceOneType == DialogType.BranchDialog && choiceOneDialog != string.Empty) {
            branchOneDialog.welcomeDialog = choiceOneDialog;
        }
        if (choiceTwoType == DialogType.BranchDialog && choiceTwoDialog != string.Empty) {
            branchTwoDialog.welcomeDialog = choiceTwoDialog;
        }
        if (choiceThreeType == DialogType.BranchDialog && choiceThreeDialog != string.Empty) {
            branchThreeDialog.welcomeDialog = choiceThreeDialog;
        }
        if (choiceFourType == DialogType.BranchDialog && choiceFourDialog != string.Empty) {
            branchFourDialog.welcomeDialog = choiceFourDialog;
        }
    }


    public override void Interact() {
        base.Interact();

        TopDownUIManager.instance.SetUIState(TopDownUIManager.instance.uiHolder);
        TopDownUIManager.instance.SetUIState(TopDownUIManager.instance.dialog);

        TopDownCharacterManager.instance.activeCharacter.GetComponent<TopDownControllerInteract>().tempDisable = true;

        TopDownUIDialogMain.instance.dialogCameraPosition = cameraPosition;

        TopDownUIDialogMain.instance.ClearDialog();
        TopDownUIDialogMain.instance.ShowDialog(this);
    }
}
