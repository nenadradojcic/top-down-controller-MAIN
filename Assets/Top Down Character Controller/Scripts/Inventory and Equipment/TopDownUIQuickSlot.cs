using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownUIQuickSlot : TopDownUIItemSlot {

    public TopDownUIItemSlot originalSlot;

    public KeyCode slotActivateKey;

    public Text keyText;

    public void Update() {
        if(itemInSlot != null) {

            if(slotActivateKey.ToString().Contains("Alpha")) {
                string tmp = slotActivateKey.ToString();
                string tmp1 = tmp.Replace("Alpha", "");
                keyText.text = tmp1;
            }

            if (Input.GetKeyDown(slotActivateKey)) {

                if (itemInSlot.weaponHoldingType == WeaponHoldingType.TwoHanded) {
                    if (TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[3].itemInSlot != null) {

                        TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[3].UseSlottedItem();
                        print(TopDownUIInventory.instance.currentEquipmentManager.gameObject.name);

                        TopDownUIInventory.instance.MoveItemToInventory(TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[3]);
                        ClearSlot(TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[3]);
                    }
                }

                //If item we are trying to equip is shield, we want to check if there is a two handed weapon equipped and to deequip it
                if (itemInSlot.itemType == ItemType.Shield) {
                    if (TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[2].itemInSlot != null && TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[2].itemInSlot.weaponHoldingType == WeaponHoldingType.TwoHanded) {

                        TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[2].UseSlottedItem();
                        print(TopDownUIInventory.instance.currentEquipmentManager.gameObject.name);

                        TopDownUIInventory.instance.MoveItemToInventory(TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[2]);
                        ClearSlot(TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[2]);
                    }
                }

                if (TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].itemInSlot == null) {
                    TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].AddItemToSlot(itemInSlot);
                    TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].slottedInQuick = this;
                    TopDownUIInventory.instance.RemoveItem(originalSlot.itemInSlot);
                    originalSlot.slottedInQuick = null;
                    ClearSlot(originalSlot);
                    originalSlot = TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType];
                }
                else if(TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].itemInSlot == itemInSlot) {
                    TopDownUIInventory.instance.MoveItemToInventory(TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType]);
                    ClearSlot(TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType]);

                }
                else if (TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].itemInSlot != itemInSlot) {
                    //first we need to unequip old item and move it to inventory
                    //then equip new item
                    TopDownUIInventory.instance.MoveItemToInventory(TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType]);
                    TopDownUIInventory.instance.currentEquipmentManager.UnequipItem(TopDownUIInventory.instance.currentEquipmentManager.currentEquipment[(int)itemInSlot.itemType]);
                    ClearSlot(TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType]);

                    TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].AddItemToSlot(itemInSlot);
                    TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].slottedInQuick = this;
                    TopDownUIInventory.instance.RemoveItem(originalSlot.itemInSlot);

                    originalSlot.slottedInQuick = null;
                    ClearSlot(originalSlot);
                    originalSlot = TopDownUIInventory.instance.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType];
                }



                UseSlottedItem();
                //print(TopDownUIInventory.instance.currentEquipmentManager.gameObject.name);
                //We need to move item from inventory slot to equipment slot
            }
        }
        else {
            keyText.text = string.Empty;
        }
    }
}
