using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownUICharacterButton : MonoBehaviour {

    public TopDownControllerMain characterInSlot;

    public GameObject holder;
    public new Text name;
    public Image portrait;
    public Image health;
    public Image energy;

    public GameObject thisSlotInventory;
    public TopDownCharacterEquipmentSlots inventory;

    public bool occupied = false;

    public TopDownCharacterManager characterManager;
    public TopDownCameraBasic cameraBasic;

    private void Start() {
        if(characterInSlot != null) {
            SetCharacterUI();
            occupied = true;
        }

        characterManager = TopDownCharacterManager.instance;
        cameraBasic = GameObject.FindObjectOfType<TopDownCameraBasic>();
    }

    public void SetCharacterUI() {
        if(characterInSlot != null) {
            if(characterInSlot.GetComponent<TopDownCharacterCard>().character != null) {
                name.text = characterInSlot.GetComponent<TopDownCharacterCard>().character.name;
                portrait.enabled = true;
                portrait.sprite = characterInSlot.GetComponent<TopDownCharacterCard>().character.icon;
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
        portrait.sprite = null;
        portrait.enabled = false;
        if (thisSlotInventory != null) {
            inventory = null;
            thisSlotInventory.SetActive(false);
        }
    }

    public void SetActiveCharacter() {
        if (characterManager.activeCharacter == null) {
            characterInSlot.GetComponent<TopDownControllerInteract>().enabled = true;
            characterInSlot.GetComponent<TopDownCharacterCard>().inventoryCamera.SetActive(true);
            characterManager.activeCharacter = characterInSlot.gameObject;
            TopDownUIInventory.instance.currentEquipmentManager = characterInSlot.GetComponent<TopDownEquipmentManager>();
            cameraBasic.td_Target = characterInSlot.transform;
            cameraBasic.cameraType = CameraType.CharacterCamera;
        }
        else if (characterManager.activeCharacter != characterInSlot.gameObject) {

            characterManager.activeCharacter.GetComponent<TopDownControllerInteract>().focusedTarget = null;
            characterManager.activeCharacter.GetComponent<TopDownControllerInteract>().enabled = false;
            characterManager.activeCharacter.GetComponent<TopDownCharacterCard>().inventoryCamera.SetActive(false);

            characterInSlot.GetComponent<TopDownControllerInteract>().enabled = true;
            characterInSlot.GetComponent<TopDownCharacterCard>().inventoryCamera.SetActive(true);
            characterInSlot.GetComponent<TopDownCharacterCard>().DeactivateAi();
            characterManager.activeCharacter = characterInSlot.gameObject;

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