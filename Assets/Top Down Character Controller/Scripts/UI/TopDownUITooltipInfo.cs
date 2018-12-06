using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TopDownUITooltipInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [TextArea(10, 20)]
    public string tooltipInfo;

    public Vector2 tooltipOffset; //custom offset
    public Vector2 screenSize;

    public CanvasGroup canvasGroup;
    public Text tooltipTxt;
    public Camera mainCamera;
    public TopDownUIManager uiManager;

    private void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        uiManager = TopDownUIManager.instance;

        if(uiManager != null) {
            canvasGroup = uiManager.statsTooltip.GetComponent<CanvasGroup>();
            tooltipTxt = uiManager.statsTooltip.GetComponentInChildren<Text>();
        }

        canvasGroup.alpha = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (tooltipInfo != string.Empty) {
            canvasGroup.alpha = 1f;
            tooltipTxt.text = tooltipInfo;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        canvasGroup.alpha = 0f;
        tooltipTxt.text = null;
    }

    public void LateUpdate() {
        if (canvasGroup.alpha == 1f) {
            if (tooltipOffset == Vector2.zero) {
                Vector2 pos = Input.mousePosition;
                canvasGroup.transform.position = new Vector2(pos.x + (screenSize.x / uiManager.statsTooltipOffset.x), pos.y + (screenSize.y / uiManager.statsTooltipOffset.y));
            }
            else {
                Vector2 pos = Input.mousePosition;
                canvasGroup.transform.position = new Vector2(pos.x + (screenSize.x / tooltipOffset.x), pos.y + (screenSize.y / tooltipOffset.y));
            }
        }

        screenSize = new Vector2(Screen.width, Screen.height);
    }
}
