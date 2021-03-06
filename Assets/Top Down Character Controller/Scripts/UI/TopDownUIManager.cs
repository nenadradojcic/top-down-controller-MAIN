﻿using UnityEngine;

public enum CharacterPortraitType { Static = 0, Runtime = 1}
public class TopDownUIManager : MonoBehaviour {

    public float uiSpeed = 1f;

    public GameObject uiHolder;

    public GameObject quickSlotBar;
    public GameObject inventory;
    public GameObject dialog;
    public GameObject questLog;

    public GameObject pausedGameNotification;

    public GameObject itemTooltip;
    public Vector2 itemTooltipOffset;
    public GameObject statsTooltip;
    public Vector2 statsTooltipOffset;

    public GameObject itemWorldName;
    public GameObject npcWorldName;
    public GameObject genericWorldTooltip;

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
        SetUIState(pausedGameNotification);
        SetUIState(itemTooltip);
        SetUIState(statsTooltip);

        itemWorldName.GetComponent<TopDownUIItemName>().nameText.text = string.Empty;
        npcWorldName.GetComponent<TopDownUINpcNameBar>().nameText.text = string.Empty;
        genericWorldTooltip.GetComponent<TopDownUIGeneralWorldTooltip>().tooltipText.text = string.Empty;
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

        if (charInfoPanel.inventoryActive && charInfoPanel.questLogActive) {
            inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.inventoryNorPos.x, charInfoPanel.inventoryNewYPos);
            questLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.questLogNorPos.x, charInfoPanel.questLogNewYPos);
        }
        else if (!charInfoPanel.inventoryActive && charInfoPanel.questLogActive) {
            inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.inventoryNorPos.x, charInfoPanel.inventoryNorPos.x);
            questLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.questLogNorPos.x, charInfoPanel.questLogNorPos.y);
        }
        else if (charInfoPanel.inventoryActive && !charInfoPanel.questLogActive) {
            inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.inventoryNorPos.x, charInfoPanel.inventoryNorPos.y);
            questLog.GetComponent<RectTransform>().anchoredPosition = new Vector2(charInfoPanel.questLogNorPos.x, charInfoPanel.questLogNorPos.y);
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
