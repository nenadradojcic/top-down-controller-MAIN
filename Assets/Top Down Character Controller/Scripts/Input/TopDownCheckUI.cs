using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TopDownCheckUI : MonoBehaviour {

    public string uiNameDetected;

    public static TopDownCheckUI instance;

    private void Awake() {
        instance = this;
    }

    public bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current) {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (results.Count > 0) {
            uiNameDetected = results[0].gameObject.name;
        }
        else {
            uiNameDetected = string.Empty;
        }
        return results.Count > 0;
    }
}