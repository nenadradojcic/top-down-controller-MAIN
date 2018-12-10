using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownToolMoveTargetRemove : MonoBehaviour {

    public void OnTriggerEnter(Collider col) {
        if(col.transform.tag == "Player") {
            gameObject.transform.DetachChildren();
            Destroy(gameObject);
        }
    }
}
