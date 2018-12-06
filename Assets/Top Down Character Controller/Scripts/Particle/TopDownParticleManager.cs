using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownParticleManager : MonoBehaviour {

    public GameObject[] hitParticle;

    public static TopDownParticleManager instance;

    public void Start() {
        instance = this;
    }
}
