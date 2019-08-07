using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TopDownFootsteps : MonoBehaviour {

    public float stepRunInterval = 0.35f;
    
    public NavMeshAgent navMeshAgent;
    public TopDownControllerMain tdc_Main;
    private TopDownAudioManager tdc_AudioManager;

    public bool stepped = false;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if(GetComponent<TopDownControllerMain>()) {
            tdc_Main = GetComponent<TopDownControllerMain>();
        }

        tdc_AudioManager = TopDownAudioManager.instance;

        if (GameObject.FindObjectOfType<TopDownAudioManager>()) {
            tdc_AudioManager = GameObject.FindObjectOfType<TopDownAudioManager>();
        }
        else {
            GameObject audioManagerGo = Instantiate(Resources.Load("TD_AudioManager") as GameObject);
            tdc_AudioManager = audioManagerGo.GetComponent<TopDownAudioManager>();
        }
    }

    public void Update() {
        if (stepped == false) {
            if (tdc_Main == null) {
                if (navMeshAgent.velocity.normalized != Vector3.zero) {
                    StartCoroutine(Footstep(stepRunInterval));
                }
            }
            else {
                if (tdc_Main.tdcm_MoveAmount > 0f) {
                    float interval = tdc_Main.tdcm_MoveAmount * stepRunInterval;
                    StartCoroutine(Footstep(interval));
                }
            }
        }
    }

    public IEnumerator Footstep(float seconds) {
            stepped = true;
            Instantiate(tdc_AudioManager.footstepAudio, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(seconds);
            stepped = false;
    }
}
