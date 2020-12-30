using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterManager : MonoBehaviour {

    public GameObject defaultCharacter;

    public GameObject controllingCharacter;
    public GameObject oldActiveCharacter;

	public List<TopDownControllerMain> activeCharacters;

    public TopDownUICharacterButton[] characterButtonsUi;

    public TopDownCameraBasic characterCamera;

    private TopDownUIInventory td_Inventory;

    public static TopDownCharacterManager instance;

    public void Awake() {
        instance = this;
        td_Inventory = TopDownUIInventory.instance;
    }

    public void Start() {

        if (TopDownUIManager.instance != null) {
            characterButtonsUi = TopDownUIManager.instance.characterPortraits;
        }

        characterCamera = GameObject.FindObjectOfType<TopDownCameraBasic>();

        if (defaultCharacter != null) {
            AddCharacterToParty(defaultCharacter);
        }

        if (defaultCharacter == null && activeCharacters.Count < 1) {
            if (GameObject.FindGameObjectWithTag("Player")) {
                AddCharacterToParty(GameObject.FindGameObjectWithTag("Player").gameObject);
            }
            else {
                Debug.LogError("No character with TopDownControllerMain.cs found in scene!");
                return;
            }
        }
    }

    /// <summary>
    /// Used when we want to add new character to our party.
    /// </summary>
    /// <param name="newCharacter"></param>
    public void AddCharacterToParty(GameObject newCharacter) {
        if(activeCharacters.Count < 4) {
            for(int i = 0; i < characterButtonsUi.Length; i++) {
                if(characterButtonsUi[i].occupied == false) {
                    activeCharacters.Add(newCharacter.GetComponent<TopDownControllerMain>());
                    characterButtonsUi[i].characterInSlot = newCharacter.GetComponent<TopDownControllerMain>();
                    characterButtonsUi[i].SetCharacterUI();

                    newCharacter.GetComponent<TopDownCharacterCard>().td_characterIndex = i;
                    newCharacter.GetComponent<TopDownCharacterCard>().characterInventory = td_Inventory.charEquipmentSlots[i];
                    newCharacter.GetComponent<TopDownCharacterCard>().characterInventory.SetUpInventory(newCharacter.GetComponent<TopDownCharacterCard>());
                    newCharacter.GetComponent<TopDownCharacterCard>().UpdatePortrait();

                    
                    //With this method we are adding equipment to usable equipment manager and inventory from npc
                    if (td_Inventory.currentEquipmentSlots != null && td_Inventory.currentEquipmentManager != null) { //We want to do this only if this is not our first cha
                        TopDownCharacterEquipmentSlots lastSlots = td_Inventory.currentEquipmentSlots;
                        TopDownEquipmentManager lastManager = td_Inventory.currentEquipmentManager;
                        
                        lastManager.gameObject.GetComponent<TopDownCharacterCard>().characterInventory.gameObject.SetActive(false);

                        td_Inventory.currentEquipmentSlots = newCharacter.GetComponent<TopDownCharacterCard>().characterInventory;
                        td_Inventory.currentEquipmentManager = newCharacter.GetComponent<TopDownCharacterCard>().equipmentManager;

                        //Check if NPC has equipment on himself, so we can add it to character inventory window.
                        if (newCharacter.GetComponent<TopDownStartupItemsSetup>()) {
                            TopDownStartupItemsSetup startUpItems = newCharacter.GetComponent<TopDownStartupItemsSetup>();

                            if (startUpItems.itemsEquipped.Count > 0) {
                                for (int s = 0; s < startUpItems.itemsEquipped.Count; s++) {

                                    TopDownItemObject tmp = startUpItems.itemsEquipped[s];
                                    td_Inventory.AddItemJustAsset(startUpItems.itemsEquipped[s]);
                                    newCharacter.GetComponent<TopDownCharacterCard>().characterInventory.equipmentSlots[(int)startUpItems.itemsEquipped[s].itemType].ClearSlot(startUpItems.itemsEquipped[s].slotOfThisItem);
                                    newCharacter.GetComponent<TopDownCharacterCard>().characterInventory.equipmentSlots[(int)startUpItems.itemsEquipped[s].itemType].AddItemToSlot(tmp);
                                }
                            }

                            if (startUpItems.itemsToPlaceInInventory.Length > 0) {
                                for (int inv = 0; inv < startUpItems.itemsToPlaceInInventory.Length; inv++) {
                                    td_Inventory.AddItemJustAsset(startUpItems.itemsToPlaceInInventory[inv]);
                                    startUpItems.itemsInInventory.Add(startUpItems.itemsToPlaceInInventory[inv]);
                                }

                                startUpItems.itemsToPlaceInInventory = null;
                            }
                        }

                        td_Inventory.currentEquipmentSlots = lastSlots;
                        td_Inventory.currentEquipmentManager = lastManager;

                        lastManager.gameObject.GetComponent<TopDownCharacterCard>().characterInventory.gameObject.SetActive(true);

                        newCharacter.GetComponent<TopDownCharacterCard>().characterInventory.armorPointsTxt.text = "AP: " + newCharacter.GetComponent<TopDownEquipmentManager>().armorPointsValue.ToString();
                        newCharacter.GetComponent<TopDownCharacterCard>().characterInventory.damagePointsTxt.text = "DP: " + newCharacter.GetComponent<TopDownEquipmentManager>().damagePointsValue.ToString();

                        newCharacter.GetComponent<TopDownCharacterCard>().characterInventory.gameObject.SetActive(false);
                    }

                    if (controllingCharacter == null) {
                        controllingCharacter = newCharacter;
                        controllingCharacter.GetComponent<TopDownControllerInteract>().enabled = true;
                        if (controllingCharacter.GetComponent<TopDownCharacterCard>().inventoryCamera != null) {
                            controllingCharacter.GetComponent<TopDownCharacterCard>().inventoryCamera.SetActive(true);
                        }

                        TopDownUIInventory.instance.currentEquipmentManager = newCharacter.GetComponent<TopDownEquipmentManager>();
                        TopDownUIInventory.instance.currentEquipmentSlots = newCharacter.GetComponent<TopDownCharacterCard>().characterInventory;
                    }
                    else {
                        newCharacter.GetComponent<TopDownCharacterCard>().SetAiActive();
                    }

                    newCharacter.tag = "Player";


                    if (characterCamera.td_Target == null) {
                        characterCamera.td_Target = newCharacter.transform;
                    }

                    characterButtonsUi[i].occupied = true;

                    return;
                }
            }
        }
        else {
            Debug.Log("No more room in party. We already have max number(4) of character.");
        }
    }

    /// <summary>
    /// When called will remove specified character from party.
    /// </summary>
    /// <param name="removeCharacter"></param>
    public void RemoveCharacterFromParty(GameObject removeCharacter) {
        if(activeCharacters.Count > 1) {
            for(int i = 0; i < activeCharacters.Count; i++) {
                if(activeCharacters[i] == removeCharacter.GetComponent<TopDownControllerMain>()) {

                    activeCharacters.Remove(removeCharacter.GetComponent<TopDownControllerMain>());

                    removeCharacter.GetComponent<TopDownCharacterCard>().td_characterIndex = -1;

                    removeCharacter.GetComponent<TopDownCharacterCard>().characterInventory = null;

                    ClearAllPortraits();
                    UpdatePortraitsUI();

                    removeCharacter.GetComponent<TopDownCharacterCard>().DeactivateAi();
                    removeCharacter.tag = "NPC";

                    if (controllingCharacter == removeCharacter) {

                        if (activeCharacters.Count > 1) {
                            oldActiveCharacter = controllingCharacter;
                            characterButtonsUi[i - 1].SetActiveCharacter();
                            TopDownUIInventory.instance.currentEquipmentSlots = activeCharacters[i - 1].GetComponent<TopDownCharacterCard>().characterInventory;
                            //Debug.Log(characterButtonsUi[i - 1].characterInSlot.td_Character.name + " set as active character");
                        }
                        else if (activeCharacters.Count == 1) {
                            characterButtonsUi[0].SetActiveCharacter();
                            TopDownUIInventory.instance.currentEquipmentSlots = activeCharacters[0].GetComponent<TopDownCharacterCard>().characterInventory;
                            //Debug.Log(characterButtonsUi[0].characterInSlot.td_Character.name + " set as active character");
                        }
                    }

                    return;
                }
            }
        }
        else {
            Debug.Log("You can not remove the last one character left in your party.");
        }
    }

    public void UpdatePortraitsUI() {

        for(int i = 0; i < characterButtonsUi.Length; i++) {
            if (i < activeCharacters.Count) {
                characterButtonsUi[i].characterInSlot = activeCharacters[i];
                characterButtonsUi[i].characterInSlot.GetComponent<TopDownCharacterCard>().td_characterIndex = i;
                characterButtonsUi[i].occupied = true;
                characterButtonsUi[i].SetCharacterUI();
            }
            else {
                characterButtonsUi[i].ClearCharacterUI();
            }
        }
    }

    public void ClearAllPortraits() {
        for (int i = 0; i < characterButtonsUi.Length; i++) {
            characterButtonsUi[i].ClearCharacterUI();
        }
    }
}
