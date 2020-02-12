using System.Collections.Generic;
using UnityEngine;

public enum ContainerState {
    Closed = 0,
    Open = 1,
}

[RequireComponent(typeof(SphereCollider))]
public class TopDownItemContainer : TopDownInteractible {

    public ContainerState containerState;

    public string containerName;

    public List<TopDownItemObject> itemsInContainer;

    public Transform itemDropPoint;

    public TopDownUIManager td_UiManager;
    public TopDownUIInventory td_Inventory;

    public bool mouseOver = false;
    public TopDownUIItemName itemName;

    public Camera mainCamera;

    public Animator containerAnimator;

    private void Start() {

        td_Inventory = TopDownUIInventory.instance;
        td_UiManager = TopDownUIManager.instance;

        if (td_UiManager != null) {
            itemName = td_UiManager.itemWorldName.GetComponent<TopDownUIItemName>();
        }

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if(containerAnimator == null) {
            containerAnimator = GetComponent<Animator>();
        }
    }

    public override void Interact() {
        base.Interact();

        if (td_Inventory != null) {
            //if (TopDownAudioManager.instance.inventoryItemPickupAudio != null) {
            //    Instantiate(TopDownAudioManager.instance.inventoryItemPickupAudio, Vector3.zero, Quaternion.identity);
            //}

            //td_Inventory.AddItem(this);

            if (containerState == ContainerState.Closed) {
                containerAnimator.SetBool("OpenChest", true);
                containerState = ContainerState.Open;
                GetComponent<SphereCollider>().enabled = false;

                if (itemsInContainer.Count > 0) {
                    for (int i = 0; i < itemsInContainer.Count; i++) {
                        GameObject item = GameObject.Instantiate(Resources.Load("TD_ItemWorldObjectPrefab", typeof(GameObject))) as GameObject;
                        item.GetComponent<TopDownItem>().item = itemsInContainer[i];
                        item.name = itemsInContainer[i].itemName;

                        if(itemDropPoint != null) {
                            item.transform.SetParent(itemDropPoint);
                            item.transform.localPosition = Vector3.zero;
                            item.transform.SetParent(null);
                        }
                        else {
                            item.transform.SetParent(transform);
                            item.transform.localPosition = Vector3.zero;
                            item.transform.SetParent(null);
                        }
                    }
                }
            }

            mouseOver = false;
            itemName.nameText.text = string.Empty;

            itemName.transform.position = new Vector2(-100f, 0f);

            this.enabled = false;
        }
    }

    public void LateUpdate() {

        if (td_UiManager != null) {
            itemName.screenY = Screen.height;

            if (mouseOver == true) {
                Vector2 tmp = mainCamera.WorldToScreenPoint(transform.position);
                Vector2 namePos = new Vector3(tmp.x, tmp.y + (itemName.yOffset * itemName.screenY), 0f);
                itemName.transform.position = namePos;
            }

            if (hasInteracted == true) {
                mouseOver = false;
                itemName.nameText.text = string.Empty;

                itemName.transform.position = new Vector2(-100f, 0f);
            }
        }
    }

    public void OnMouseOver() {
        mouseOver = true;
        if (td_UiManager != null && td_UiManager.checkUi.IsPointerOverUIObject() == false) {
            itemName.nameText.text = containerName;
        }
    }

    public void OnMouseExit() {
        mouseOver = false;
        if (td_UiManager != null) {
            itemName.nameText.text = string.Empty;

            itemName.transform.position = new Vector2(-100f, 0f);
        }
    }
}
