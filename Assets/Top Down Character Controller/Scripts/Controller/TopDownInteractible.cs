using UnityEngine;

public class TopDownInteractible : MonoBehaviour {

    public float interactDistance = 1f;

    public Transform playerTransform;
    public bool isFocus = false;

    public bool hasInteracted = false;

    private void Update() {

        if (isFocus == true && hasInteracted == false) {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= interactDistance) {
                Interact();
            }
        }
    }

    public void OnFocused(Transform pcT) {
        playerTransform = pcT;
        isFocus = true;
        hasInteracted = false;
    }

    public void OnDefocused() {
        playerTransform = null;
        isFocus = false;
        hasInteracted = false;
    }

    public virtual void Interact() {
        hasInteracted = true;
    }

    public void OnDrawGizmosSelect() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
