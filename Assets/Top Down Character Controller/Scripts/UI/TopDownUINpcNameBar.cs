using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownUINpcNameBar : MonoBehaviour {

    public bool placeBarOverHead = true;
    public Text nameText;
    public Image healthBar;
    public Image energyBar;
    public Image portraitImage;

    public float yOffset = 0.15f;
    public float yOffsetOverHead = 0.05f;
    public float screenY;

    private void Update() {
        screenY = Screen.height;
    }
}
