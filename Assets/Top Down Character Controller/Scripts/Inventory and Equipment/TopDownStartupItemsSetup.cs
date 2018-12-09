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

    private void Start() {
        //td_equipmentManager = GetComponent<TopDownEquipmentManager>();
        td_CharacterCard = GetComponent<TopDownCharacterCard>();
        td_Inventory = TopDownUIInventory.instance;

        for(int i = 0; i < itemsToPlaceInInventory.Count; i++) {
            td_Inventory.AddItemJustAsset(itemsToPlaceInInventory[i]);
        }

        for (int i = 0; i < itemsToEquip.Count; i++) {
            td_Inventory.AddItemJustAsset(itemsToEquip[i]);
            itemsToEquip[i].slotOfThisItem.UseSlottedItem();

            TopDownItemObject tmp = itemsToEquip[i];
            td_CharacterCard.characterInventory.equipmentSlots[(int)itemsToEquip[i].itemType].ClearSlot(itemsToEquip[i].slotOfThisItem);
            td_CharacterCard.characterInventory.equipmentSlots[(int)itemsToEquip[i].itemType].AddItemToSlot(tmp);
        }
    }
}
