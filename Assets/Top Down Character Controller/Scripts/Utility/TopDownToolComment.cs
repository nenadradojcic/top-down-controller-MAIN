using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownToolComment : MonoBehaviour {

    [Tooltip("This component will be destroyed on play/start of the scene.")]
    [TextArea(4, 10)]
    public string comment;

    private void Start() {
        Destroy(this);
    }
}
