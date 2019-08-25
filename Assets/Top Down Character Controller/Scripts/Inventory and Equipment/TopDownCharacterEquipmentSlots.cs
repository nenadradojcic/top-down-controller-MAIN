using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownCharacterEquipmentSlots : MonoBehaviour {

    public TopDownCharacterCard characterInSlot;
    public bool occupied = false;

    public Text healthTxt;
    public Text energyTxt;

    public Text armorPointsTxt;
    public Text damagePointsTxt;

    public Text nameTxt;
    public Text levelTxt;

    public TopDownUIEquipmentSlot[] equipmentSlots;

    public void Start() {
        int numSlots = System.Enum.GetNames(typeof(ItemType)).Length;

        if (equipmentSlots.Length != numSlots) {
            equipmentSlots = new TopDownUIEquipmentSlot[numSlots];

            Debug.LogError("Character Equipment slots are not set! You must set them by hand!");
        }
    }

    public void SetUpInventory(TopDownCharacterCard character) {
        if(occupied == false && character != null) {
            characterInSlot = character;
            //character.characterInventory = this;

            healthTxt.text = characterInSlot.health.ToString();
            energyTxt.text = characterInSlot.energy.ToString();

            armorPointsTxt.text = "AP: " + characterInSlot.armorPoints.ToString();
            damagePointsTxt.text = "DP: " + characterInSlot.damagePoints.ToString();

            nameTxt.text = characterInSlot.character.name;
            levelTxt.text = characterInSlot.level.ToString();
        }
    }

    public void ResetInventory() {
        occupied = false;

        healthTxt.text = string.Empty;
        energyTxt.text = string.Empty;

        armorPointsTxt.text = string.Empty;
        damagePointsTxt.text = string.Empty;

        nameTxt.text = string.Empty;
        levelTxt.text = string.Empty;

        characterInSlot.characterInventory = null;

    }
}
