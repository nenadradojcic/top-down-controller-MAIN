using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownDecompose : MonoBehaviour {

    public bool decompose = false;

    public float decomposeSpeed = 0.00015f;

    public float startingY = 0f;
    public float currentY = 0f;
    public float difference;


    private void LateUpdate() {
        currentY = transform.position.y;

        if(decompose) {
            Decompose();
        }
    }

    private void Decompose() {

        if (startingY == 0f) {
            startingY = transform.position.y;
        }

        currentY = transform.position.y;

        difference = startingY - currentY;

        if (difference <= 0.2f) {
            transform.Translate(Vector3.down * decomposeSpeed);
        }
        else {
            Destroy(gameObject);
            decompose = false;
        }
    }
}
