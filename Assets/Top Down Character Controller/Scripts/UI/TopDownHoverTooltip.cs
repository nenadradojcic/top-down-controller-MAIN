using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class TopDownHoverTooltip : MonoBehaviour {

    [TextArea(5, 25)]
    public string tooltip;

    public bool interacted = false;
    public bool mouseOver = false;

    public float distanceTillVisible = 8f;
    private float distToPlayer = 0f;

    public UnityEvent onMouseOverEvent;

    private TopDownUIGeneralWorldTooltip tooltipUi;

    private TopDownCharacterManager td_CharacterManager;
    private TopDownUIManager td_UiManager;

    private Camera mainCamera;

    private void Start() {
        td_CharacterManager = TopDownCharacterManager.instance;
        td_UiManager = TopDownUIManager.instance;

        if (td_UiManager != null) {
            tooltipUi = td_UiManager.genericWorldTooltip.GetComponent<TopDownUIGeneralWorldTooltip>();
        }

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void OnMouseOver() {
        mouseOver = true;
        if (td_UiManager != null && td_UiManager.checkUi.IsPointerOverUIObject() == false) {
            tooltipUi.tooltipText.text = tooltip;
            if (interacted == false) {
                onMouseOverEvent.Invoke();
                onMouseOverEvent.RemoveAllListeners();
                interacted = true;
            }
        }
    }

    public void OnMouseExit() {
        mouseOver = false;
        if (td_UiManager != null) {
            tooltipUi.tooltipText.text = string.Empty;

            tooltipUi.transform.position = new Vector2(-100f, 0f);
        }
    }

    public void LateUpdate() {

        if (td_UiManager != null) {
            tooltipUi.screenY = Screen.height;

            if (mouseOver == true) {
                if (distToPlayer <= distanceTillVisible) {
                    Vector2 tmp = mainCamera.WorldToScreenPoint(transform.position);
                    Vector2 namePos = new Vector3(tmp.x, tmp.y + (tooltipUi.yOffset * tooltipUi.screenY), 0f);
                    tooltipUi.transform.position = namePos;
                }
            }

            distToPlayer = Vector3.Distance(transform.position, td_CharacterManager.activeCharacter.transform.position);
        }
    }
}
