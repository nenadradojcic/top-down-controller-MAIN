using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownRoofManager : MonoBehaviour {

    public GameObject roofObject;
    public float dissapearSpeed = 0.5f;

    private void Start() {
        if(roofObject != null) {
            if (roofObject.activeSelf == false) {
                roofObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (roofObject != null && other.GetComponent<TopDownControllerMain>()) {
            StartCoroutine(RoofFadeInOrOur(roofObject, 0f, false, dissapearSpeed));
        }
    }

    private void OnTriggerExit(Collider other) {
        if (roofObject != null && other.GetComponent<TopDownControllerMain>()) {
            StartCoroutine(RoofFadeInOrOur(roofObject, 1f, false, dissapearSpeed));
        }
    }

    public IEnumerator RoofFadeInOrOur(GameObject go, float targetAlpha, bool isVanish, float duration) {

        Renderer sr = go.GetComponent<Renderer>();
        float diffAlpha = (targetAlpha - sr.material.color.a);

        float counter = 0;
        while (counter < duration) {
            float alphaAmount = sr.material.color.a + (Time.deltaTime * diffAlpha) / duration;
            sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, alphaAmount);

            counter += Time.deltaTime;
            yield return null;
        }

        sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, targetAlpha);

        if (isVanish) {
            sr.transform.gameObject.SetActive(false);
        }
    }
}
