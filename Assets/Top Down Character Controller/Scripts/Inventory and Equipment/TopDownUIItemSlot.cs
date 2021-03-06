﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SlotType {
    Inventory = 0,
    Equipment = 1,
    Quickslot = 2,
}

public class TopDownUIItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public SlotType slotType;

    public TopDownItemObject itemInSlot;

    public Color normalSlotColor;
    public Color equipedSlotColor;
    public Image itemIcon;

    public TopDownUIQuickSlot slottedInQuick;

    //private TopDownEquipmentManager equipmentManager;
    private TopDownUIInventory inventory;
    private TopDownControllerMain main;

    private void Start() {
        inventory = TopDownUIInventory.instance;
    }

    public void AddItemToSlot(TopDownItemObject item) {
        item.slotOfThisItem = this;
        itemInSlot = item;
        itemIcon.sprite = item.itemIcon;
        itemIcon.enabled = true;
    }

    public void ClearSlot(TopDownUIItemSlot slot) {
        if (slot != null) {
            if (slot.itemInSlot != null) {
                slot.itemInSlot.slotOfThisItem = null;
                slot.itemInSlot = null;
            }
            slot.itemIcon.sprite = null;
            slot.itemIcon.enabled = false;
        }
    }

    public void UseSlottedItem() {
        if(itemInSlot != null) {
            itemInSlot.UseItem();
        }
    }

    public void GrabItemInSlot() {
        inventory.holdingItem = itemInSlot;

        inventory.holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = itemInSlot.itemIcon;
        inventory.holdingItemSlot.GetComponent<CanvasGroup>().alpha = 1f;

        inventory.previousSlot = this;

        ClearTooltip();
    }

    public void OnPointerClick(PointerEventData eventData) {

        main = TopDownCharacterManager.instance.controllingCharacter.GetComponent<TopDownControllerMain>();

        if (main.tdcm_animator.GetBool("TargetInFront") == false) { //We should be able to use slots only when out of combat
            if (eventData.button == PointerEventData.InputButton.Right) {
                if (itemInSlot != null && inventory.holdingItem == null) {
                    if (GetComponent<TopDownUIItemSlot>().slotType != SlotType.Quickslot) {
                        if (itemInSlot.isItem == true) {
                            //If item we are trying to equip is two handed or ranged weapon, we want to check if there is a shield equipped and to deequip it
                            if (itemInSlot.weaponHoldingType == WeaponHoldingType.TwoHanded || itemInSlot.weaponType == WeaponType.Ranged) {
                                if (inventory.currentEquipmentSlots.equipmentSlots[3].itemInSlot) {

                                    inventory.currentEquipmentSlots.equipmentSlots[3].UseSlottedItem();
                                    //print(inventory.currentEquipmentManager.gameObject.name);

                                    inventory.MoveItemToInventory(inventory.currentEquipmentSlots.equipmentSlots[3]);
                                    ClearSlot(inventory.currentEquipmentSlots.equipmentSlots[3]);
                                }
                            }

                            //If item we are trying to equip is shield, we want to check if there is a two handed weapon equipped and to deequip it
                            if (itemInSlot.itemType == ItemType.Shield) {
                                if (inventory.currentEquipmentSlots.equipmentSlots[2].itemInSlot != null && (inventory.currentEquipmentSlots.equipmentSlots[2].itemInSlot.weaponHoldingType == WeaponHoldingType.TwoHanded ||
                                    inventory.currentEquipmentSlots.equipmentSlots[2].itemInSlot.weaponType == WeaponType.Ranged)) {

                                    inventory.currentEquipmentSlots.equipmentSlots[2].UseSlottedItem();
                                    //print(inventory.currentEquipmentManager.gameObject.name);

                                    inventory.MoveItemToInventory(inventory.currentEquipmentSlots.equipmentSlots[2]);
                                    ClearSlot(inventory.currentEquipmentSlots.equipmentSlots[2]);
                                }
                            }

                            if (inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType] != null) {
                                if (inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].itemInSlot == null) {

                                    //Debug.Log("No item equiped.");

                                    UseSlottedItem();
                                    //print(inventory.currentEquipmentManager.gameObject.name);

                                    inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].AddItemToSlot(itemInSlot);
                                    if (slottedInQuick != null) {
                                        inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].slottedInQuick = slottedInQuick;
                                        slottedInQuick = null;
                                    }
                                    inventory.RemoveItem(itemInSlot);
                                }
                                else if (inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].itemInSlot == itemInSlot) {

                                    //Debug.Log("This item is equiped.");

                                    UseSlottedItem();
                                    //print(inventory.currentEquipmentManager.gameObject.name);

                                    inventory.MoveItemToInventory(inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType]);
                                    inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].AddItemToSlot(itemInSlot);
                                }
                                else if (inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].itemInSlot != itemInSlot) {

                                    //Debug.Log("Other item is equiped.");

                                    inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].UseSlottedItem();
                                    //print(inventory.currentEquipmentManager.gameObject.name);

                                    inventory.MoveItemToInventory(inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType]);
                                    inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].AddItemToSlot(itemInSlot);

                                    UseSlottedItem();
                                    //print(inventory.currentEquipmentManager.gameObject.name);

                                    inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].AddItemToSlot(itemInSlot);
                                    if (slottedInQuick != null) {
                                        inventory.currentEquipmentSlots.equipmentSlots[(int)itemInSlot.itemType].slottedInQuick = slottedInQuick;
                                        slottedInQuick = null;
                                    }
                                    inventory.RemoveItem(itemInSlot);
                                }
                            }

                            ClearSlot(this);

                            /*if (TopDownAudioManager.instance.inventoryItemUseAudio != null) {
                                Instantiate(TopDownAudioManager.instance.inventoryItemUseAudio, Vector3.zero, Quaternion.identity);
                            }*/
                        }
                        else { //If this is scroll we want different behaviour

                            if (TopDownAudioManager.instance.spellUseAudio != null) {
                                Instantiate(TopDownAudioManager.instance.spellUseAudio, Vector3.zero, Quaternion.identity);
                            }

                            TopDownCharacterManager.instance.controllingCharacter.GetComponent<TopDownRpgSpellcaster>().activeSpell = itemInSlot;
                            TopDownCharacterManager.instance.controllingCharacter.GetComponent<TopDownRpgSpellcaster>().spellItemSlot = this;
                            TopDownCharacterManager.instance.controllingCharacter.GetComponent<TopDownRpgSpellcaster>().castingSpell = true;

                            if (TopDownUIManager.instance.charInfoPanel.inventoryActive) {
                                if (TopDownUIManager.instance.inventory.GetComponent<CanvasGroup>().alpha > 0) {
                                    TopDownUIManager.instance.SetUIState(TopDownUIManager.instance.inventory);
                                    TopDownUIManager.instance.charInfoPanel.inventoryActive = false;
                                }
                            }
                            if (TopDownUIManager.instance.charInfoPanel.questLogActive) {
                                if (TopDownUIManager.instance.questLog.GetComponent<CanvasGroup>().alpha > 0) {
                                    TopDownUIManager.instance.SetUIState(TopDownUIManager.instance.questLog);
                                    TopDownUIManager.instance.charInfoPanel.inventoryActive = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Left) {
                if (inventory.holdingItem != null) {
                    if (GetComponent<TopDownUIItemSlot>().slotType == SlotType.Equipment) { //Is the slot we clicked on of Equipment type
                        if (GetComponent<TopDownUIEquipmentSlot>().equipmentType == inventory.holdingItem.itemType) { //Is the item we are holding of the same type as the Equipment Slot (so we do not equip helm on weapon etc...)
                            if (GetComponent<TopDownUIEquipmentSlot>().itemInSlot != inventory.holdingItem && GetComponent<TopDownUIEquipmentSlot>() != inventory.previousSlot) { //We do not want to swap the same item

                                //If item we are trying to equip is two handed weapon, we want to check if there is a shield equipped and to deequip it
                                if(inventory.holdingItem.weaponHoldingType == WeaponHoldingType.TwoHanded) {
                                    if (inventory.currentEquipmentSlots.equipmentSlots[3].itemInSlot != null) {

                                        inventory.currentEquipmentSlots.equipmentSlots[3].UseSlottedItem();

                                        inventory.MoveItemToInventory(inventory.currentEquipmentSlots.equipmentSlots[3]);
                                        ClearSlot(inventory.currentEquipmentSlots.equipmentSlots[3]);
                                    }
                                }
                                
                                //If item we are trying to equip is shield, we want to check if there is a two handed weapon equipped and to deequip it
                                if (inventory.holdingItem.itemType == ItemType.Shield) {
                                    if (inventory.currentEquipmentSlots.equipmentSlots[2].itemInSlot != null && inventory.currentEquipmentSlots.equipmentSlots[2].itemInSlot.weaponHoldingType == WeaponHoldingType.TwoHanded) {

                                        inventory.currentEquipmentSlots.equipmentSlots[2].UseSlottedItem();

                                        inventory.MoveItemToInventory(inventory.currentEquipmentSlots.equipmentSlots[2]);
                                        ClearSlot(inventory.currentEquipmentSlots.equipmentSlots[2]);
                                    }
                                }
                                
                                TopDownItemObject tmpItem = inventory.currentEquipmentManager.currentEquipment[(int)inventory.holdingItem.itemType];

                                if (inventory.previousSlot.slottedInQuick != null) { //If item we are holding is slotted in Quick Slot, we want to keep the same Quick Slot reference
                                    slottedInQuick = inventory.previousSlot.slottedInQuick;
                                    inventory.previousSlot.slottedInQuick = null;
                                }
                                ClearSlot(inventory.previousSlot); //Clear previous Slot
                                inventory.previousSlot = null;
                                if (itemInSlot != null) {
                                    inventory.currentEquipmentManager.UnequipItem(itemInSlot);
                                }
                                AddItemToSlot(inventory.holdingItem);
                                UseSlottedItem();

                                if (tmpItem != null) {
                                    inventory.holdingItem = tmpItem;
                                    inventory.holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = inventory.holdingItem.itemIcon;
                                }
                                else {
                                    inventory.holdingItem = null;
                                    inventory.holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = null;
                                    inventory.holdingItemSlot.GetComponent<CanvasGroup>().alpha = 0f;
                                }
                            }
                            else {
                                inventory.holdingItem = null;
                                inventory.holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = null;
                                inventory.holdingItemSlot.GetComponent<CanvasGroup>().alpha = 0f;
                            }
                        }
                        else {
                            //Debug.Log("This is NOT the same equipment type");
                        }
                    }
                    else if (GetComponent<TopDownUIItemSlot>().slotType == SlotType.Inventory) {
                        if (itemInSlot == null) {
                            if (inventory.previousSlot != null) {
                                if (inventory.currentEquipmentManager.currentEquipment[(int)inventory.previousSlot.itemInSlot.itemType] == inventory.previousSlot.itemInSlot) {
                                    inventory.previousSlot.itemInSlot.UnuseItem();
                                }
                                if (inventory.previousSlot.slottedInQuick != null) {
                                    inventory.previousSlot.slottedInQuick.originalSlot = this;
                                    slottedInQuick = inventory.previousSlot.slottedInQuick;
                                    inventory.previousSlot.slottedInQuick = null;
                                }
                                ClearSlot(inventory.previousSlot);
                                inventory.previousSlot = null;
                            }
                            AddItemToSlot(inventory.holdingItem);
                            inventory.holdingItem = null;
                            inventory.holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = null;
                            inventory.holdingItemSlot.GetComponent<CanvasGroup>().alpha = 0f;
                        }
                        else {
                            if (itemInSlot == inventory.holdingItem && GetComponent<TopDownUIItemSlot>() == inventory.previousSlot) { //This is the same item, remove holding item (we must check if it is the same slot, because we can have multiple same items in inventory)
                                inventory.holdingItem = null;
                                inventory.holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = null;
                                inventory.holdingItemSlot.GetComponent<CanvasGroup>().alpha = 0f;
                            }
                            else {
                                if (inventory.previousSlot != null) {
                                    if (inventory.previousSlot.slottedInQuick != null) {
                                        inventory.previousSlot.slottedInQuick.originalSlot = this;
                                        slottedInQuick = inventory.previousSlot.slottedInQuick;
                                        inventory.previousSlot.slottedInQuick = null;
                                    }

                                    TopDownItemObject tmpItemToGrab = itemInSlot;

                                    ClearSlot(inventory.previousSlot);
                                    inventory.previousSlot = null;

                                    AddItemToSlot(inventory.holdingItem);

                                    inventory.holdingItem = tmpItemToGrab;

                                    inventory.holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = tmpItemToGrab.itemIcon;
                                    inventory.holdingItemSlot.GetComponent<CanvasGroup>().alpha = 1f;

                                    ClearTooltip();
                                }
                                /*
                                TopDownItemObject tmpItem = inventory.holdingItem;

                                if (inventory.previousSlot != null) {
                                    if (inventory.currentEquipmentManager.currentEquipment[(int)inventory.previousSlot.itemInSlot.itemType] == inventory.previousSlot.itemInSlot) {
                                        inventory.previousSlot.itemInSlot.UnuseItem();
                                    }
                                    if (inventory.previousSlot.slottedInQuick != null) {
                                        inventory.previousSlot.slottedInQuick.originalSlot = this;
                                        slottedInQuick = inventory.previousSlot.slottedInQuick;
                                        inventory.previousSlot.slottedInQuick = null;
                                    }
                                    ClearSlot(inventory.previousSlot);
                                    inventory.previousSlot = null;
                                }

                                GrabItemInSlot();

                                AddItemToSlot(tmpItem);
                                */
                            }
                        }
                    }
                    else if (GetComponent<TopDownUIItemSlot>().slotType == SlotType.Quickslot) {
                        if (GetComponent<TopDownUIQuickSlot>().originalSlot != null) {
                            GetComponent<TopDownUIQuickSlot>().originalSlot.slottedInQuick = null;
                        }
                        ClearSlot(this);
                        AddItemToSlot(inventory.holdingItem);
                        if (inventory.previousSlot.slotType != SlotType.Quickslot) {
                            if (inventory.previousSlot.slottedInQuick == null) {
                                inventory.previousSlot.slottedInQuick = GetComponent<TopDownUIQuickSlot>();
                            }
                            else {
                                if (inventory.previousSlot.slottedInQuick != this) {
                                    inventory.previousSlot.slottedInQuick.originalSlot = null;
                                    ClearSlot(inventory.previousSlot.slottedInQuick);
                                    inventory.previousSlot.slottedInQuick = GetComponent<TopDownUIQuickSlot>();
                                }
                            }
                            GetComponent<TopDownUIQuickSlot>().originalSlot = inventory.previousSlot;
                        }
                        else {
                            ClearSlot(inventory.previousSlot);
                            GetComponent<TopDownUIQuickSlot>().originalSlot = inventory.previousSlot.GetComponent<TopDownUIQuickSlot>().originalSlot;
                            inventory.previousSlot.GetComponent<TopDownUIQuickSlot>().originalSlot.slottedInQuick = GetComponent<TopDownUIQuickSlot>();
                            inventory.previousSlot.GetComponent<TopDownUIQuickSlot>().originalSlot = null;

                        }

                        inventory.holdingItem = null;
                        inventory.holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = null;
                        inventory.holdingItemSlot.GetComponent<CanvasGroup>().alpha = 0f;
                    }
                }
                else {
                    if (itemInSlot != null) {
                        GrabItemInSlot();
                    }
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        SetTooltip(itemInSlot);
    }

    public void OnPointerExit(PointerEventData eventData) {
        ClearTooltip();
    }

    public void SetTooltip(TopDownItemObject item) {
        if (item != null) {
            if (item.damageModifier != 0) {
                TopDownUIManager.instance.itemTooltip.GetComponent<TopDownUIItemTooltip>().itemNameTxt.text = item.itemName + " +" +item.damageModifier;
            }
            else if(item.armorModifier != 0) {
                TopDownUIManager.instance.itemTooltip.GetComponent<TopDownUIItemTooltip>().itemNameTxt.text = item.itemName + " +" + item.armorModifier;
            }
            else {
                TopDownUIManager.instance.itemTooltip.GetComponent<TopDownUIItemTooltip>().itemNameTxt.text = item.itemName;
            }
            /*TopDownUIManager.instance.itemTooltip.GetComponent<TopDownUIItemTooltip>().itemStatsTxt.text = "Strength +" + item.strengthModifier +
                                                                                                           "\nDexterity +" + item.dexterityModifier +
                                                                                                           "\nConstitution +" + item.constitutionModifier +
                                                                                                           "\nWillpower +" + item.willpowerModifier;*/
            TopDownUIManager.instance.itemTooltip.GetComponent<TopDownUIItemTooltip>().itemDescrTxt.text = item.itemDescription;
            if (TopDownUIManager.instance.itemTooltip.GetComponent<CanvasGroup>().alpha == 0f) {
                TopDownUIManager.instance.itemTooltip.GetComponent<CanvasGroup>().alpha = 1f;
            }

            TopDownUIManager.instance.itemTooltip.GetComponent<TopDownUIItemTooltip>().slotType = slotType;
        }
    }

    public void ClearTooltip() {
            if (TopDownUIManager.instance.itemTooltip.GetComponent<CanvasGroup>().alpha == 1f) {
                TopDownUIManager.instance.itemTooltip.GetComponent<CanvasGroup>().alpha = 0f;
            }
            TopDownUIManager.instance.itemTooltip.GetComponent<TopDownUIItemTooltip>().itemNameTxt.text = string.Empty;
            //TopDownUIManager.instance.itemTooltip.GetComponent<TopDownUIItemTooltip>().itemStatsTxt.text = string.Empty;
            TopDownUIManager.instance.itemTooltip.GetComponent<TopDownUIItemTooltip>().itemDescrTxt.text = string.Empty;
    }
}
