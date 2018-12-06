using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownUIItemTooltip : MonoBehaviour {

    public Canvas canvas;

    public Text itemNameTxt;
    public Text itemStatsTxt;
    public Text itemDescrTxt;

    public Vector2 tooltipOffset;
    public Vector2 quickSlotOffset;

    private CanvasGroup canvasGroup;

    public Vector2 screenSize;

    public SlotType slotType;

    private void Start() {
        canvasGroup = GetComponent<CanvasGroup>();

        if(tooltipOffset == Vector2.zero) {
            tooltipOffset = new Vector2(1f, 1f);
        }

        if (quickSlotOffset == Vector2.zero) {
            quickSlotOffset = new Vector2(1f, 1f);
        }
    }

    private void LateUpdate() {
        if (canvasGroup.alpha == 1f) {
            if (slotType == SlotType.Quickslot) {
                Vector2 pos = Input.mousePosition;
                transform.position = new Vector2(pos.x + (screenSize.x / quickSlotOffset.x), pos.y + (screenSize.y / quickSlotOffset.y));
            }
            else {
                Vector2 pos = Input.mousePosition;
                transform.position = new Vector2(pos.x + (screenSize.x / tooltipOffset.x), pos.y + (screenSize.y / tooltipOffset.y));
            }
        }

        screenSize = new Vector2(Screen.width, Screen.height);
    }
}
