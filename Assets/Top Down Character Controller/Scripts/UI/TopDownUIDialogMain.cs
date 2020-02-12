using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownUIDialogMain : MonoBehaviour {

    public bool useKeyNumShortcuts;
    [SerializeField]
    public KeyCode dialog1Key = KeyCode.Alpha1;
    [SerializeField]
    public KeyCode dialog2Key = KeyCode.Alpha2;
    [SerializeField]
    public KeyCode dialog3Key = KeyCode.Alpha3;
    [SerializeField]
    public KeyCode dialog4Key = KeyCode.Alpha4;
    [SerializeField]
    public KeyCode dialog5Key = KeyCode.Alpha5;

    public TopDownUIDialog dialogInUse;
    public TopDownUIDialogChoice[] dialogChoices;

    public static TopDownUIDialogMain instance;

    public float dialogCamMoveSpeed = 1f;
    private float defCamMoveSpeed;
    public TopDownCameraBasic cameraBasic;
    public Vector3 cameraDialogPos; //This is based on local space of active player character

    public void Awake() {
        instance = this;
    }

    public void Start() {
        cameraBasic = TopDownCameraBasic.instance;
    }

    public void LateUpdate() {
        if(useKeyNumShortcuts && dialogInUse != null) {
            if(Input.GetKeyUp(dialog1Key)) {
                if(!string.IsNullOrEmpty(dialogChoices[0].dialog)) {
                    dialogChoices[0].ShowChoiceDialog();
                }
            }
            else if (Input.GetKeyUp(dialog2Key)) {
                if (!string.IsNullOrEmpty(dialogChoices[1].dialog)) {
                    dialogChoices[1].ShowChoiceDialog();
                }
            }
            else if (Input.GetKeyUp(dialog3Key)) {
                if (!string.IsNullOrEmpty(dialogChoices[2].dialog)) {
                    dialogChoices[2].ShowChoiceDialog();
                }
            }
            else if (Input.GetKeyUp(dialog4Key)) {
                if (!string.IsNullOrEmpty(dialogChoices[3].dialog)) {
                    dialogChoices[3].ShowChoiceDialog();
                }
            }
            else if (Input.GetKeyUp(dialog5Key)) {
                if (!string.IsNullOrEmpty(dialogChoices[4].dialog)) {
                    dialogChoices[4].ShowChoiceDialog();
                }
            }
        }
    }

    public void ClearDialog() {

        if(cameraBasic.td_Target.name == "TempCameraBetweenPosition") {
            Destroy(cameraBasic.td_Target.gameObject);
        }
        cameraBasic.td_Target = TopDownCharacterManager.instance.activeCharacter.transform;

        cameraBasic.cameraFreeze = false;

        dialogChoices[0].dialogTxt.text = string.Empty;

        for (int i = 0; i < dialogChoices.Length; i++) {
            dialogChoices[i].GetComponent<Button>().interactable = false;
            dialogChoices[i].GetComponentInChildren<Text>().text = string.Empty;
        }

        if (dialogInUse != null) {
            if (dialogInUse.choiceOne != string.Empty) {
                dialogInUse.numberOfChoices = 1;
            }
            else {
                dialogInUse = null;
                return;
            }
            if (dialogInUse.choiceTwo != string.Empty) {
                dialogInUse.numberOfChoices = 2;
            }
            else {
                dialogInUse = null;
                return;
            }
            if (dialogInUse.choiceThree != string.Empty) {
                dialogInUse.numberOfChoices = 3;
            }
            else {
                dialogInUse = null;
                return;
            }
            if (dialogInUse.choiceFour != string.Empty) {
                dialogInUse.numberOfChoices = 4;
            }
            else {
                dialogInUse = null;
                return;
            }

            dialogInUse = null;
        }
    }

    private IEnumerator SetCameraPosition() {

        cameraBasic.cameraFreeze = true;
        GameObject camPos = new GameObject();
        camPos.transform.SetParent(TopDownCharacterManager.instance.activeCharacter.transform);
        camPos.transform.localPosition = cameraDialogPos;
        camPos.transform.SetParent(null);
        cameraBasic.transform.localPosition = camPos.transform.position;
        Destroy(camPos);

        yield return new WaitForEndOfFrame();
    }

    public void ShowDialog(TopDownUIDialog dialog) {
        if (dialog != null) {
            dialogInUse = dialog;

            /*cameraBasic.cameraFreeze = true;
            GameObject camPos = new GameObject();
            camPos.transform.SetParent(TopDownCharacterManager.instance.activeCharacter.transform);
            camPos.transform.localPosition = cameraDialogPos;
            camPos.transform.SetParent(null);
            cameraBasic.transform.localPosition = camPos.transform.position;
            Destroy(camPos);*/

            cameraBasic.td_Target = dialog.transform;

            Quaternion npcRotation = Quaternion.LookRotation(TopDownCharacterManager.instance.activeCharacter.transform.position - dialog.transform.position);
            dialog.transform.rotation = Quaternion.Slerp(transform.rotation, npcRotation, Time.deltaTime * 600f);

            Quaternion playerRotation = Quaternion.LookRotation(dialog.transform.position - TopDownCharacterManager.instance.activeCharacter.transform.position);
            TopDownCharacterManager.instance.activeCharacter.transform.rotation = Quaternion.Slerp(TopDownCharacterManager.instance.activeCharacter.transform.rotation, playerRotation, Time.deltaTime * 600f);

            StartCoroutine(SetCameraPosition());

            dialogChoices[0].dialogTxt.text = dialog.welcomeDialog;

            for (int i = 0; i < dialogChoices.Length; i++) {
                if ((i - 1) < dialog.numberOfChoices) {
                    if (i == 1) {
                        dialogChoices[i - 1].GetComponent<Button>().interactable = true;
                        dialogChoices[i - 1].choiceTxt.text = dialog.choiceOne;

                        dialogChoices[i - 1].dialog = dialog.choiceOneDialog;
                        dialogChoices[i - 1].type = dialog.choiceOneType;
                        dialogChoices[i - 1].remove = dialog.removeOnChoiceOne;
                        dialogChoices[i - 1].branch = dialog.branchOneDialog;
                        dialogChoices[i - 1].response = dialog.typeOneResponse;
                        dialogChoices[i - 1].events = dialog.choiceOneEvent;
                        dialogChoices[i - 1].index = i;
                    }
                    if (i == 2) {
                        dialogChoices[i - 1].GetComponent<Button>().interactable = true;
                        dialogChoices[i - 1].choiceTxt.text = dialog.choiceTwo;

                        dialogChoices[i - 1].dialog = dialog.choiceTwoDialog;
                        dialogChoices[i - 1].type = dialog.choiceTwoType;
                        dialogChoices[i - 1].remove = dialog.removeOnChoiceTwo;
                        dialogChoices[i - 1].branch = dialog.branchTwoDialog;
                        dialogChoices[i - 1].response = dialog.typeTwoResponse;
                        dialogChoices[i - 1].events = dialog.choiceTwoEvent;
                        dialogChoices[i - 1].index = 2;
                    }
                    if (i == 3) {
                        dialogChoices[i - 1].GetComponent<Button>().interactable = true;
                        dialogChoices[i - 1].choiceTxt.text = dialog.choiceThree;

                        dialogChoices[i - 1].dialog = dialog.choiceThreeDialog;
                        dialogChoices[i - 1].type = dialog.choiceThreeType;
                        dialogChoices[i - 1].remove = dialog.removeOnChoiceThree;
                        dialogChoices[i - 1].branch = dialog.branchThreeDialog;
                        dialogChoices[i - 1].response = dialog.typeThreeResponse;
                        dialogChoices[i - 1].events = dialog.choiceThreeEvent;
                        dialogChoices[i - 1].index = 3;
                    }
                    if (i == 4) {
                        dialogChoices[i - 1].GetComponent<Button>().interactable = true;
                        dialogChoices[i - 1].choiceTxt.text = dialog.choiceFour;

                        dialogChoices[i - 1].dialog = dialog.choiceFourDialog;
                        dialogChoices[i - 1].type = dialog.choiceFourType;
                        dialogChoices[i - 1].remove = dialog.removeOnChoiceFour;
                        dialogChoices[i - 1].branch = dialog.branchFourDialog;
                        dialogChoices[i - 1].response = dialog.typeFourResponse;
                        dialogChoices[i - 1].events = dialog.choiceFourEvent;
                        dialogChoices[i - 1].index = 4;
                    }
                }
                else {
                    dialogChoices[i - 1].GetComponent<Button>().interactable = false;
                    dialogChoices[i - 1].choiceTxt.text = string.Empty;
                    dialogChoices[i - 1].index = 0;
                }
            }
        }
    }

    public void CloseDialogPerType(string buttonText) {

        for (int i = 0; i < dialogChoices.Length; i++) {
            if (i == 0) {
                dialogChoices[0].GetComponent<Button>().interactable = true;
                dialogChoices[0].choiceTxt.text = buttonText;
                dialogChoices[0].type = DialogType.ImmediateClose;
                dialogChoices[0].events = null;
            }
            else {
                dialogChoices[i].GetComponent<Button>().interactable = false;
                dialogChoices[i].GetComponentInChildren<Text>().text = string.Empty;
                dialogChoices[i - 1].index = 0;
            }
        }
    }

    public enum ChoicePosition { Top = 0, Bottom = 1}
    /// <summary>
    /// Used to add new dialog choices.
    /// </summary>
    /// <param name="dialog">Dialog that we are adding choice to.</param>
    /// <param name="choiceText">Text in the choice button.</param>
    /// <param name="dialogText">Text in the dialog box when choice is activated.</param>
    /// <param name="dialogChoiceType">What type of dialog choice this is?</param>
    /// <param name="choicePosition">Position of choice in the choices list. It can be at Top or at Bottom. If there is four choices already if on Top the last one will be removed and if on Bottom the first one will be removed.</param>
    public void AddNewChoice(TopDownRpgQuest quest, TopDownUIDialog dialog, string choiceText, string dialogText, DialogType dialogChoiceType, ChoicePosition choicePosition) {
        if(choicePosition == ChoicePosition.Top) {

            for (int i = dialog.numberOfChoices; i > 0; i--) {
                if (i == 4) {
                    dialog.choiceFour = dialog.choiceThree;
                    dialog.choiceFourDialog = dialog.choiceThreeDialog;
                    dialog.choiceFourType = dialog.choiceThreeType;
                    dialog.removeOnChoiceFour = dialog.removeOnChoiceThree;
                    dialog.branchFourDialog = dialog.branchThreeDialog;
                    dialog.typeFourResponse = dialog.typeThreeResponse;
                    dialog.choiceFourEvent = dialog.choiceThreeEvent;
                }
                if (i == 3) {
                    dialog.choiceThree = dialog.choiceTwo;
                    dialog.choiceThreeDialog = dialog.choiceTwoDialog;
                    dialog.choiceThreeType = dialog.choiceTwoType;
                    dialog.removeOnChoiceThree = dialog.removeOnChoiceTwo;
                    dialog.branchThreeDialog = dialog.branchTwoDialog;
                    dialog.typeThreeResponse = dialog.typeTwoResponse;
                    dialog.choiceThreeEvent = dialog.choiceTwoEvent;
                }
                if (i == 2) {
                    dialog.choiceTwo = dialog.choiceOne;
                    dialog.choiceTwoDialog = dialog.choiceOneDialog;
                    dialog.choiceTwoType = dialog.choiceOneType;
                    dialog.removeOnChoiceTwo = dialog.removeOnChoiceOne;
                    dialog.branchTwoDialog = dialog.branchOneDialog;
                    dialog.typeTwoResponse = dialog.typeOneResponse;
                    dialog.choiceTwoEvent = dialog.choiceOneEvent;
                }
                if (i == 1) {
                    dialog.choiceOne = choiceText;
                    dialog.choiceOneDialog = dialogText;
                    dialog.choiceOneType = dialogChoiceType;
                    dialog.removeOnChoiceOne = true;
                    dialog.branchOneDialog = null;
                    dialog.typeOneResponse = string.Empty;
                    dialog.choiceOneEvent = null;
                    if(quest == null) {
                        print("No quest");
                    }
                    dialog.choiceOneEvent = quest.questFinishEvents;
                }
            }

            return;
        }
    }
}
