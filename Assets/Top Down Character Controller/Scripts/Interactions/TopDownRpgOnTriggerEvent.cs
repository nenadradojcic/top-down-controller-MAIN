using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TopDownRpgOnTriggerEvent : MonoBehaviour {

    public string triggererTag;

    public bool destroyAfter = false;

    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public UnityEvent OnStay;

    public void OnTriggerEnter(Collider other) {
        if(other.tag == triggererTag) {
            OnEnter.Invoke();

            if(destroyAfter) {
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.tag == triggererTag) {
            OnExit.Invoke();

            if (destroyAfter) {
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerStay(Collider other) {
        if (other.tag == triggererTag) {
            OnStay.Invoke();

            if (destroyAfter) {
                Destroy(gameObject);
            }
        }
    }
}
