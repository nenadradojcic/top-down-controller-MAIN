using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterManager : MonoBehaviour {

    public GameObject defaultCharacter;

    public GameObject activeCharacter;
    public GameObject oldActiveCharacter;

	public List<TopDownControllerMain> activeCharacters;

    public TopDownUICharacterButton[] characterButtonsUi;

    public TopDownCameraBasic characterCamera;

    public static TopDownCharacterManager instance;

    public void Awake() {
        instance = this;
    }

    public void Start() {

        characterButtonsUi = TopDownUIManager.instance.characterPortraits;

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
                    newCharacter.GetComponent<TopDownCharacterCard>().characterInventory = TopDownUIInventory.instance.charEquipmentSlots[i];
                    newCharacter.GetComponent<TopDownCharacterCard>().characterInventory.SetUpInventory(newCharacter.GetComponent<TopDownCharacterCard>());

                    if (activeCharacter == null) {
                        activeCharacter = newCharacter;
                        activeCharacter.GetComponent<TopDownControllerInteract>().enabled = true;
                        if (activeCharacter.GetComponent<TopDownCharacterCard>().inventoryCamera != null) {
                            activeCharacter.GetComponent<TopDownCharacterCard>().inventoryCamera.SetActive(true);
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

                    if (activeCharacter == removeCharacter) {

                        if (activeCharacters.Count > 1) {
                            oldActiveCharacter = activeCharacter;
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
