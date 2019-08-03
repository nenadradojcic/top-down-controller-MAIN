using UnityEngine;

public enum CharacterPortraitType { Static = 0, Runtime = 1}
public class TopDownUIManager : MonoBehaviour {

    public float uiSpeed = 1f;

    public GameObject uiHolder;

    public GameObject quickSlotBar;
    public GameObject inventory;
    public GameObject dialog;
    public GameObject questLog;
    public GameObject abilitesLog;

    public GameObject pausedGameNotification;

    public GameObject itemTooltip;
    public Vector2 itemTooltipOffset;
    public GameObject statsTooltip;
    public Vector2 statsTooltipOffset;
    public GameObject abilityTooltip;
    public Vector2 abilityTooltipOffset;

    public GameObject itemWorldName;
    public GameObject npcWorldName;

    public GameObject notificationText;

    public bool pausedGame = false;

    public CharacterPortraitType characterPortraitType;
    public TopDownUICharacterButton[] characterPortraits;

    public TopDownCheckUI checkUi;
    public TopDownUICharacterInfoPanel charInfoPanel;

    public static TopDownUIManager instance;

    private void Awake() {
        instance = this;

        SetUIState(inventory);
        SetUIState(questLog);
        SetUIState(abilitesLog);
        SetUIState(pausedGameNotification);
        SetUIState(itemTooltip);
        SetUIState(statsTooltip);
        SetUIState(abilityTooltip);

        itemWorldName.GetComponent<TopDownUIItemName>().nameText.text = string.Empty;
        npcWorldName.GetComponent<TopDownUINpcNameBar>().nameText.text = string.Empty;
    }

    public void LateUpdate() {
        if (Input.GetKeyDown(TopDownInputManager.instance.pauseGameKey)) {
            PauseGame();
        }
        else if (Input.GetKeyDown(TopDownInputManager.instance.inventoryKeyCode)) {
            if (TopDownAudioManager.instance.inventoryOpenAudio != null) {
                Instantiate(TopDownAudioManager.instance.inventoryOpenAudio, Vector3.zero, Quaternion.identity);
            }
            SetCharUIState(inventory);
        }
        else if (Input.GetKeyDown(TopDownInputManager.instance.questLogKeyCode)) {
            if (TopDownAudioManager.instance.inventoryOpenAudio != null) {
                Instantiate(TopDownAudioManager.instance.inventoryOpenAudio, Vector3.zero, Quaternion.identity);
            }
            SetCharUIState(questLog);
        }
        else if (Input.GetKeyDown(TopDownInputManager.instance.abilitesLogKeyCode)) {
            if (TopDownAudioManager.instance.inventoryOpenAudio != null) {
                Instantiate(TopDownAudioManager.instance.inventoryOpenAudio, Vector3.zero, Quaternion.identity);
            }
            SetCharUIState(abilitesLog);
        }
    }

    public void SetUIState(GameObject uiObject) {
        if(uiObject.GetComponent<CanvasGroup>()) {
            if (uiObject.GetComponent<CanvasGroup>().interactable == true) {
                uiObject.GetComponent<CanvasGroup>().alpha = 0f;
                uiObject.GetComponent<CanvasGroup>().interactable = false;
                uiObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else {
                uiObject.GetComponent<CanvasGroup>().interactable = true;
                uiObject.GetComponent<CanvasGroup>().alpha = 1f;
                uiObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        else {
            uiObject.SetActive(!uiObject.activeSelf);
        }
    }

    public void SetCharUIState(GameObject uiObject) {
        if (uiObject.GetComponent<CanvasGroup>()) {
            if (uiObject.GetComponent<CanvasGroup>().interactable == true) {
                uiObject.GetComponent<CanvasGroup>().alpha = 0f;
                uiObject.GetComponent<CanvasGroup>().interactable = false;
                uiObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else {
                uiObject.GetComponent<CanvasGroup>().interactable = true;
                uiObject.GetComponent<CanvasGroup>().alpha = 1f;
                uiObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        else {
            uiObject.SetActive(!uiObject.activeSelf);
        }

        if(uiObject == inventory) {
            charInfoPanel.inventoryActive = !charInfoPanel.inventoryActive;
        }
        else if (uiObject == questLog) {
            charInfoPanel.questLogActive = !charInfoPanel.questLogActive;
        }
        else if (uiObject == abilitesLog) {
            charInfoPanel.abilitiesLogActive = !charInfoPanel.abilitiesLogActive;
        }

        if (charInfoPanel.inventoryActive && charInfoPanel.questLogActive && charInfoPanel.abilitiesLogActive) {
            inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.inventoryNewXPos, charInfoPanel.inventoryNewYPos);
            questLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.questLogNewXPos, charInfoPanel.questLogNewYPos);
            abilitesLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.abilitiesLogNewXPos, charInfoPanel.abilitiesLogNewYPos);
        }
        else if (!charInfoPanel.inventoryActive && charInfoPanel.questLogActive && charInfoPanel.abilitiesLogActive) {
            inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.inventoryNorPos.x, charInfoPanel.inventoryNorPos.x);
            questLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.questLogNewXPos, charInfoPanel.questLogNorPos.y);
            abilitesLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.abilitiesLogNewXPos, charInfoPanel.abilitiesLogNorPos.y);
        }
        else if (charInfoPanel.inventoryActive && !charInfoPanel.questLogActive && charInfoPanel.abilitiesLogActive) {
            inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.inventoryNewXPos, charInfoPanel.inventoryNorPos.y);
            questLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.questLogNorPos.x, charInfoPanel.questLogNorPos.y);
            abilitesLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.abilitiesLogNewXPos, charInfoPanel.abilitiesLogNorPos.y);
        }
        else if (charInfoPanel.inventoryActive && charInfoPanel.questLogActive && !charInfoPanel.abilitiesLogActive) {
            inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.inventoryNorPos.x, charInfoPanel.inventoryNewYPos);
            questLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.questLogNorPos.x, charInfoPanel.questLogNewYPos);
            abilitesLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.abilitiesLogNorPos.x, charInfoPanel.abilitiesLogNorPos.y);
        }
        else {
            inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.inventoryNorPos.x, charInfoPanel.inventoryNorPos.y);
            questLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.questLogNorPos.x, charInfoPanel.questLogNorPos.y);
            abilitesLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.abilitiesLogNorPos.x, charInfoPanel.abilitiesLogNorPos.y);
        }
    }

    public void PauseGame() {

        pausedGame = !pausedGame;

        if (pausedGame == true) {
            Time.timeScale = 0f;
            if (TopDownUIManager.instance != null)
                TopDownUIManager.instance.SetUIState(TopDownUIManager.instance.pausedGameNotification);
        }
        else {
            Time.timeScale = 1f;
            TopDownUIManager.instance.SetUIState(TopDownUIManager.instance.pausedGameNotification);
        }
    }
}
