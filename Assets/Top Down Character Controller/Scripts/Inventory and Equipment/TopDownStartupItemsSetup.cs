using System.Collections.Generic;
using UnityEngine;

public class TopDownStartupItemsSetup : MonoBehaviour {

    public TopDownItemObject[] itemsToEquip;
    public TopDownItemObject[] itemsToPlaceInInventory;

    private TopDownCharacterCard td_CharacterCard;
    private TopDownUIInventory td_Inventory;

    public List<TopDownItemObject> itemsEquipped;
    public List<TopDownItemObject> itemsInInventory;

    public TopDownCharacterManager td_characterManager;
    public bool characterNotYetInParty = false;

    private void Start() {
        
        td_CharacterCard = gameObject.GetComponent<TopDownCharacterCard>();

        td_Inventory = TopDownUIInventory.instance;
        td_characterManager = TopDownCharacterManager.instance;

        if (td_Inventory != null) {
            if (td_characterManager != null) {
                for (int character = 0; character < td_characterManager.activeCharacters.Count; character++) {
                    if (td_characterManager.activeCharacters[character] == GetComponent<TopDownControllerMain>()) {
                        if (itemsToPlaceInInventory.Length > 0) {
                            for (int i = 0; i < itemsToPlaceInInventory.Length; i++) {
                                td_Inventory.AddItemJustAsset(itemsToPlaceInInventory[i]);
                                itemsInInventory.Add(itemsToPlaceInInventory[i]);
                            }

                            itemsToPlaceInInventory = null;
                        }
                    }
                }

                if (itemsToEquip.Length > 0) {
                    for (int i = 0; i < itemsToEquip.Length; i++) {

                        TopDownItemObject tmp = itemsToEquip[i];
                        if (td_CharacterCard.characterInventory != null) {
                            td_Inventory.AddItemJustAsset(itemsToEquip[i]);
                            itemsToEquip[i].slotOfThisItem.UseSlottedItem();
                            td_CharacterCard.characterInventory.equipmentSlots[(int)itemsToEquip[i].itemType].ClearSlot(itemsToEquip[i].slotOfThisItem);
                            td_CharacterCard.characterInventory.equipmentSlots[(int)itemsToEquip[i].itemType].AddItemToSlot(tmp);
                        }
                        else {
                            td_CharacterCard.equipmentManager.EquipItemForNpc(itemsToEquip[i]);
                            itemsEquipped.Add(itemsToEquip[i]);
                        }
                    }
                }
            }
            else {
                if (itemsToPlaceInInventory.Length > 0) {
                    for (int i = 0; i < itemsToPlaceInInventory.Length; i++) {
                        td_Inventory.AddItemJustAsset(itemsToPlaceInInventory[i]);
                        itemsInInventory.Add(itemsToPlaceInInventory[i]);
                    }
                }
                if (itemsToEquip.Length > 0) {
                    for (int i = 0; i < itemsToEquip.Length; i++) {

                        TopDownItemObject tmp = itemsToEquip[i];
                        if (td_CharacterCard.characterInventory != null) {
                            td_Inventory.AddItemJustAsset(itemsToEquip[i]);
                            itemsToEquip[i].slotOfThisItem.UseSlottedItem();
                            td_CharacterCard.characterInventory.equipmentSlots[(int)itemsToEquip[i].itemType].ClearSlot(itemsToEquip[i].slotOfThisItem);
                            td_CharacterCard.characterInventory.equipmentSlots[(int)itemsToEquip[i].itemType].AddItemToSlot(tmp);
                        }
                        else {
                            td_CharacterCard.equipmentManager.EquipItemForNpc(itemsToEquip[i]);
                            itemsEquipped.Add(itemsToEquip[i]);
                        }
                    }
                }
            }
        }
        else {
            this.enabled = false;
        }
    }
}
