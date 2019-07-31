using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownStartupItemsSetup : MonoBehaviour {

    public List<TopDownItemObject> itemsToEquip;
    public List<TopDownItemObject> itemsToPlaceInInventory;
    //private List<TopDownItemObject> itemsInInventory;

    //private TopDownEquipmentManager td_equipmentManager;

    private TopDownCharacterCard td_CharacterCard;
    private TopDownUIInventory td_Inventory;

    public List<TopDownItemObject> itemsEquipped;
    public List<TopDownItemObject> itemsInInventory;

    private void Start() {

        //td_equipmentManager = GetComponent<TopDownEquipmentManager>();
        td_CharacterCard = gameObject.GetComponent<TopDownCharacterCard>();
        td_Inventory = TopDownUIInventory.instance;

        if (td_Inventory != null) {
            if (itemsToPlaceInInventory.Count > 0) {
                for (int i = 0; i < itemsToPlaceInInventory.Count; i++) {
                    td_Inventory.AddItemJustAsset(itemsToPlaceInInventory[i]);
                    itemsInInventory.Add(itemsToPlaceInInventory[i]);
                }
            }
            if (itemsToEquip.Count > 0) {
                for (int i = 0; i < itemsToEquip.Count; i++) {

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
            this.enabled = false;
        }
    }
}
