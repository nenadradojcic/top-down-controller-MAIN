using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownToolDestroyAfterTime : MonoBehaviour {

    public float destroyAfter;

	void Update () {
        Destroy(gameObject, destroyAfter);
	}
}
