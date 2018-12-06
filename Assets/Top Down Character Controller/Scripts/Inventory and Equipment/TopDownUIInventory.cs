using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownUIInventory : MonoBehaviour {

    public GameObject holdingItemSlot;
    public TopDownItemObject holdingItem;
    public Vector2 holdingSlotOffset;
    public TopDownUIItemSlot previousSlot; //used for when moving items by hand

    public Transform slotsParent;
    public TopDownUIItemSlot[] slots;

    public TopDownCharacterEquipmentSlots currentEquipmentSlots;
    public TopDownEquipmentManager currentEquipmentManager;

    //public TopDownUIEquipmentSlot[] equipmentSlots;
    public TopDownCharacterEquipmentSlots[] charEquipmentSlots;

    public int itemSpace;
    public List<TopDownItemObject> items;

    public static TopDownUIInventory instance;
    public Camera mainCamera;

    public bool clickedOutOfUi = false;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        slots = slotsParent.GetComponentsInChildren<TopDownUIItemSlot>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void Update() {
        if(holdingItemSlot.GetComponent<CanvasGroup>().alpha == 1f) {
            Vector2 tmp = Input.mousePosition;
            Vector2 namePos = new Vector2(tmp.x + holdingSlotOffset.x, tmp.y + holdingSlotOffset.y);
            holdingItemSlot.transform.position = namePos;
        }

        if (previousSlot != null) {
            if (TopDownUIManager.instance.checkUi.IsPointerOverUIObject() == false) {
                if (Input.GetKeyDown(TopDownInputManager.instance.interactKey)) {
                    print("We are clicking outside UI");
                    if (previousSlot.slotType == SlotType.Inventory) {
                        slots[0].ClearSlot(previousSlot);
                        holdingItem = null;
                        holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = null;
                        holdingItemSlot.GetComponent<CanvasGroup>().alpha = 0f;
                        previousSlot = null;
                        StartCoroutine(ClickedOutOfUiTimer());
                        //PLAY DESTROY ITEM SOUND
                    }
                    else if (previousSlot.slotType == SlotType.Quickslot) {
                        previousSlot.GetComponent<TopDownUIQuickSlot>().originalSlot.slottedInQuick = null;
                        slots[0].ClearSlot(previousSlot);
                        holdingItem = null;
                        holdingItemSlot.GetComponent<TopDownUIHoldingItemSlot>().itemIconImage.sprite = null;
                        holdingItemSlot.GetComponent<CanvasGroup>().alpha = 0f;
                        previousSlot = null;
                        StartCoroutine(ClickedOutOfUiTimer());
                    }
                }
            }
        }
    }

    public IEnumerator ClickedOutOfUiTimer() {
        clickedOutOfUi = true;
        yield return new WaitForSeconds(0.5f);
        clickedOutOfUi = false;
    }

    /// <summary>
    /// Adds specified item to first free inventory slot.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(TopDownItem item) {
        if (items.Count >= itemSpace) {
            Debug.Log("No more room in the inventory.");
        }
        else {
            for (int i = 0; i < slots.Length; i++) {
                if (slots[i].itemInSlot == null) {
                    items.Add(item.item);
                    slots[i].AddItemToSlot(item.item);
                    Destroy(item.gameObject);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Moves an item to first free inventory slot.
    /// </summary>
    /// <param name="item">Item to move.</param>
    public void MoveItemToInventory(TopDownUIItemSlot item) {
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].itemInSlot == null) {
                items.Add(item.itemInSlot);
                slots[i].AddItemToSlot(item.itemInSlot);
                if(item.slottedInQuick != null) {
                    slots[i].slottedInQuick = item.slottedInQuick;
                    item.slottedInQuick.originalSlot = slots[i];
                    item.slottedInQuick = null;
                }
                return;
            }
        }
    }

    public void AddItemJustAsset(TopDownItemObject item) {
        if (items.Count >= itemSpace) {
            Debug.Log("No more room in the inventory.");
        }
        else {
            items.Add(item);
        }
    }

    public void RemoveItem(TopDownItemObject item) {
        items.Remove(item);
    }
}
