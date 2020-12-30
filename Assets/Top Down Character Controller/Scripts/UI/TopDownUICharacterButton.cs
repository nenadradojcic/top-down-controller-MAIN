using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownUICharacterButton : MonoBehaviour {

    public TopDownControllerMain characterInSlot;

    public GameObject holder;
    public new Text name;
    public Image portraitPrechosen;
    public RawImage portraitRuntime;

    public Image health;
    public Image energy;

    public GameObject thisSlotInventory;
    public TopDownCharacterEquipmentSlots inventory;

    public bool occupied = false;

    public TopDownCharacterManager characterManager;
    public TopDownCameraBasic cameraBasic;

    public TopDownUIManager uiManager;
    public TopDownUIInventory uiInventory;

    private void Start() {
        if(characterInSlot != null) {
            SetCharacterUI();
            occupied = true;
        }

        characterManager = TopDownCharacterManager.instance;
        uiManager = TopDownUIManager.instance;
        uiInventory = TopDownUIInventory.instance;

        cameraBasic = GameObject.FindObjectOfType<TopDownCameraBasic>();
    }

    public void SetCharacterUI() {
        if(characterInSlot != null) {
            if(characterInSlot.GetComponent<TopDownCharacterCard>().character != null) {
                name.text = characterInSlot.GetComponent<TopDownCharacterCard>().character.name;
                if (uiManager.characterPortraitType == CharacterPortraitType.Static) {
                    portraitRuntime.enabled = false;
                    portraitPrechosen.enabled = true;
                    portraitPrechosen.sprite = characterInSlot.GetComponent<TopDownCharacterCard>().character.icon;
                }
                else if(uiManager.characterPortraitType == CharacterPortraitType.Runtime) {
                    portraitPrechosen.enabled = false;
                    portraitRuntime.enabled = true;
                    characterInSlot.GetComponent<TopDownCharacterCard>().portraitImage = portraitRuntime;
                }
                holder.SetActive(true);

                if (thisSlotInventory != null) {
                    thisSlotInventory.SetActive(true);
                    inventory = thisSlotInventory.GetComponent<TopDownCharacterEquipmentSlots>();
                }
            }
            else {
                Debug.LogWarning("td_Character not set for " + characterInSlot.gameObject.name);
            }
        }
    }

    public void ClearCharacterUI() {
        occupied = false;
        characterInSlot = null;
        name.text = string.Empty;
        if (uiManager.characterPortraitType == CharacterPortraitType.Static) {
            portraitPrechosen.sprite = null;
            portraitPrechosen.enabled = false;
        }
        else if (uiManager.characterPortraitType == CharacterPortraitType.Runtime) {
            portraitRuntime.enabled = false;
        }
        if (thisSlotInventory != null) {
            inventory = null;
            thisSlotInventory.SetActive(false);
        }
    }

    public void SetActiveCharacter() {
        if (characterInSlot != null) {
            if (characterManager.controllingCharacter == null) {
                characterInSlot.GetComponent<TopDownControllerInteract>().enabled = true;
                if (characterManager.controllingCharacter.GetComponent<TopDownCharacterCard>().inventoryCamera != null) {
                    characterInSlot.GetComponent<TopDownCharacterCard>().inventoryCamera.SetActive(true);
                }
                characterManager.controllingCharacter = characterInSlot.gameObject;
                TopDownUIInventory.instance.currentEquipmentManager = characterInSlot.GetComponent<TopDownEquipmentManager>();
                cameraBasic.td_Target = characterInSlot.transform;
                cameraBasic.cameraType = CameraType.CharacterCamera;
            }
            else if (characterManager.controllingCharacter != characterInSlot.gameObject) {

                characterManager.controllingCharacter.GetComponent<TopDownControllerInteract>().focusedTarget = null;
                characterManager.controllingCharacter.GetComponent<TopDownControllerInteract>().enabled = false;
                if (characterManager.controllingCharacter.GetComponent<TopDownCharacterCard>().inventoryCamera != null) {
                    characterManager.controllingCharacter.GetComponent<TopDownCharacterCard>().inventoryCamera.SetActive(false);
                }

                characterInSlot.GetComponent<TopDownControllerInteract>().enabled = true;
                if (characterManager.controllingCharacter.GetComponent<TopDownCharacterCard>().inventoryCamera != null) {
                    characterInSlot.GetComponent<TopDownCharacterCard>().inventoryCamera.SetActive(true);
                }
                characterInSlot.GetComponent<TopDownCharacterCard>().DeactivateAi();
                characterManager.controllingCharacter = characterInSlot.gameObject;

                for (int i = 0; i < TopDownUIInventory.instance.charEquipmentSlots.Length; i++) {
                    TopDownUIInventory.instance.charEquipmentSlots[i].gameObject.SetActive(false);
                }

                for (int i = 0; i < characterManager.activeCharacters.Count; i++) {
                    if (characterManager.activeCharacters[i] != characterInSlot.gameObject) {
                        characterManager.activeCharacters[i].GetComponent<TopDownCharacterCard>().SetAiActive();
                    }
                }

                characterInSlot.GetComponent<TopDownCharacterCard>().DeactivateAi();
                characterInSlot.GetComponent<TopDownCharacterCard>().characterInventory.gameObject.SetActive(true);
                TopDownUIInventory.instance.currentEquipmentManager = characterInSlot.GetComponent<TopDownEquipmentManager>();
                TopDownUIInventory.instance.currentEquipmentSlots = characterInSlot.GetComponent<TopDownCharacterCard>().characterInventory;
                cameraBasic.td_Target = characterInSlot.transform;
                cameraBasic.cameraType = CameraType.CharacterCamera;

                if (characterInSlot.GetComponent<TopDownCharacterCard>().enemyFocus != null) {
                    characterInSlot.GetComponent<TopDownControllerInteract>().SetFocus(characterInSlot.GetComponent<TopDownCharacterCard>().enemyFocus);
                }
            }

            //We also need to reset holding item of our Invetory because we changed character
            if(uiInventory != null) {
                uiInventory.holdingItem = null;
                uiInventory.holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = null;
                uiInventory.holdingItemSlot.GetComponent<CanvasGroup>().alpha = 0f;
            }
        }
    }

    public void LateUpdate() {
        if(occupied == true) {
            //We now show health
            health.fillAmount = characterInSlot.GetComponent<TopDownCharacterCard>().health / characterInSlot.GetComponent<TopDownCharacterCard>().maxHealth;
            energy.fillAmount = characterInSlot.GetComponent<TopDownCharacterCard>().energy / characterInSlot.GetComponent<TopDownCharacterCard>().maxEnergy;

            inventory.healthTxt.text = characterInSlot.GetComponent<TopDownCharacterCard>().health.ToString() + "/" + characterInSlot.GetComponent<TopDownCharacterCard>().maxHealth.ToString();
            inventory.energyTxt.text = characterInSlot.GetComponent<TopDownCharacterCard>().energy.ToString() + "/" + characterInSlot.GetComponent<TopDownCharacterCard>().maxEnergy.ToString();
        }
        else {
            if(holder.activeSelf == true) {
                ClearCharacterUI();
                holder.SetActive(false);
            }
        }
    }
}