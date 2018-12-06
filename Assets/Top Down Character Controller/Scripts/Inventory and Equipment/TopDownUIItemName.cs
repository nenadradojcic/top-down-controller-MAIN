using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownUIItemName : MonoBehaviour {


    public Text nameText;

    public float yOffset = 1f;
    public float screenY;

    /*
    public Text nameText;
    public Image healthBar;

    public float yOffset = 1f;
    public float screenY;

    public TopDownItem item;
    public Transform itemNameUi;

    public Text itemNameText;
    public Camera mainCamera;

    public TopDownUIManager uiManager;

    public float yOffset = 25f;
    public float screenY;

    public bool mouseOver = false;

    private void Start() {
        item = GetComponent<TopDownItem>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        uiManager = TopDownUIManager.instance;

        if(uiManager != null) {
            itemNameUi = uiManager.itemWorldName.transform;
            itemNameText = itemNameUi.GetComponentInChildren<Text>();
        }
    }

    public void OnMouseOver() {
        mouseOver = true;
        itemNameText.text = item.item.itemName;
    }

    public void OnMouseExit() {
        mouseOver = false;
        itemNameText.text = string.Empty;
        itemNameUi.position = new Vector2(-100f, 0f);
    }

    public void Update() {
        if (uiManager != null) {
            if (uiManager.checkUi.IsPointerOverUIObject() == false && TopDownUIInventory.instance.holdingItem == null) {

                screenY = Screen.height;

                if (mouseOver == true) {
                    Vector2 tmp = mainCamera.WorldToScreenPoint(transform.position);
                    Vector2 namePos = new Vector3(tmp.x, tmp.y + (yOffset * screenY));
                    itemNameUi.position = namePos;
                }
            }
            else {
                mouseOver = false;
                itemNameText.text = string.Empty;
                itemNameUi.position = new Vector2(-100f, 0f);
            }
        }
    }
    */
}
