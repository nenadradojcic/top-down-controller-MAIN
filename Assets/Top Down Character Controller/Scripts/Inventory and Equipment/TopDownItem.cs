using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class TopDownItem : TopDownInteractible {

    public TopDownItemObject item;

    public TopDownUIManager td_UiManager;
    public TopDownUIInventory td_Inventory;

    public bool mouseOver = false;
    public TopDownUIItemName itemName;

    public Camera mainCamera;

    private void Start() {
        td_Inventory = TopDownUIInventory.instance;
        td_UiManager = TopDownUIManager.instance;
        itemName = td_UiManager.itemWorldName.GetComponent<TopDownUIItemName>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public override void Interact() {
        base.Interact();

        if (td_Inventory != null) {
            if (TopDownAudioManager.instance.inventoryItemPickupAudio != null) {
                Instantiate(TopDownAudioManager.instance.inventoryItemPickupAudio, Vector3.zero, Quaternion.identity);
            }

            td_Inventory.AddItem(this);

            mouseOver = false;
            itemName.nameText.text = string.Empty;

            itemName.transform.position = new Vector2(-100f, 0f);
        }
    }

    public void LateUpdate() {

        itemName.screenY = Screen.height;

        if (mouseOver == true) {
            Vector2 tmp = mainCamera.WorldToScreenPoint(transform.position);
            Vector2 namePos = new Vector3(tmp.x, tmp.y + (itemName.yOffset * itemName.screenY));
            itemName.transform.position = namePos;
        }

        if(hasInteracted == true) {
            mouseOver = false;
            itemName.nameText.text = string.Empty;

            itemName.transform.position = new Vector2(-100f, 0f);
        }
    }

    public void OnMouseOver() {
        mouseOver = true;
        itemName.nameText.text = item.itemName;
    }

    public void OnMouseExit() {
        mouseOver = false;
        itemName.nameText.text = string.Empty;

        itemName.transform.position = new Vector2(-100f, 0f);
    }
}
