using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TopDownCharacterCard))]
[RequireComponent(typeof(TopDownControllerMain))]
public class TopDownAI : MonoBehaviour {

    public bool hostile;

    public new string name;

    public TopDownVoiceSet voiceSet;
    public bool detected = false;

    public float velocity;

    public Transform focus;

    public bool playerInSight = false;

    public float detectRadius;
    public float fieldOfViewAngle = 180f; //Used for 
    public SphereCollider visionCollider;

    public TopDownControllerMain tdc_Main;
    public TopDownEquipmentManager td_EquipmentManager;
    public TopDownCharacterCard tdc_CharacterCard;

    public float distToStartPos;
    public Vector3 startPos;

    private void Start() {
        tdc_Main = GetComponent<TopDownControllerMain>();
        td_EquipmentManager = GetComponent<TopDownEquipmentManager>();
        startPos = transform.position;
        tdc_CharacterCard = GetComponent<TopDownCharacterCard>();
        //tdc_CharacterCard.activePlayerAi = true;
    }

    public void Update() {

        if (tdc_Main != null) {

            velocity = tdc_Main.tdcm_animator.GetFloat("Forward");

            if (focus != null) {

                tdc_Main.tdcm_NavMeshAgent.SetDestination(focus.position);

                if (tdc_Main.tdcm_NavMeshAgent.remainingDistance > tdc_Main.tdcm_NavMeshAgent.stoppingDistance) {
                    tdc_Main.TDCC_MoveCharacter(tdc_Main.tdcm_NavMeshAgent.velocity);
                }
                else {
                    tdc_Main.TDCC_MoveCharacter(Vector3.zero);
                    if (tdc_Main.tdcm_NavMeshAgent.remainingDistance != 0f) {
                        if (td_EquipmentManager != null) {
                            int i = (int)td_EquipmentManager.weaponTypeUsed;
                            tdc_Main.tdcm_animator.SetFloat("WeaponType", (float)i);
                        }
                        else {
                            tdc_Main.tdcm_animator.SetFloat("WeaponType", 1);
                        }

                        //Add rotation
                        Quaternion targetRotation = Quaternion.LookRotation(focus.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 500f);

                        tdc_Main.tdcm_animator.SetBool("TargetInFront", true);
                        tdc_Main.tdcm_animator.SetBool("Attacking", true);
                    }
                    else {
                        tdc_Main.tdcm_animator.SetBool("TargetInFront", false);
                    }
                }
            }
            else {
                tdc_Main.tdcm_NavMeshAgent.SetDestination(startPos);

                if (tdc_Main.tdcm_NavMeshAgent.remainingDistance > tdc_Main.tdcm_NavMeshAgent.stoppingDistance) {
                    tdc_Main.TDCC_MoveCharacter(tdc_Main.tdcm_NavMeshAgent.velocity);
                }
                else {
                    tdc_Main.TDCC_MoveCharacter(Vector3.zero);
                }

                tdc_Main.tdcm_animator.SetBool("Attacking", false);
                tdc_Main.tdcm_animator.SetBool("TargetInFront", false);
            }

            distToStartPos = Vector3.Distance(transform.position, startPos);
        }
    }

    public void OnTriggerStay(Collider other) {
        if(hostile == true && other.gameObject.tag == "Player") {
            playerInSight = false;
            focus = null;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            float distance = Vector3.Distance(transform.position, other.transform.position);
            
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z) + transform.up, direction.normalized * visionCollider.radius);

            if (angle < fieldOfViewAngle * 0.5f) {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, visionCollider.radius)) {
                    if (hit.collider.gameObject.tag == "Player") {
                        playerInSight = true;
                        focus = other.transform;

                        if (detected == false && voiceSet != null) {
                            Instantiate(voiceSet.detectVoice, transform.position, Quaternion.identity);
                            detected = true;
                        }
                    }
                }
            }
            else if(distance < detectRadius * 0.5f) {
                if (other.gameObject.tag == "Player") {
                    playerInSight = true;
                    focus = other.transform;

                    if (detected == false && voiceSet != null) {
                        Instantiate(voiceSet.detectVoice, transform.position, Quaternion.identity);
                        detected = true;
                    }
                }
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player") {
            playerInSight = false;
            focus = null;
            tdc_Main.tdcm_animator.SetBool("Attacking", false);
            detected = false;
        }
    }
}
