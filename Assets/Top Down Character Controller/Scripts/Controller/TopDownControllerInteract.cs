using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("Top Down RPG/Controller/Third Person Interact")]
public class TopDownControllerInteract : MonoBehaviour {

    public bool tempDisable = false;

    public NavMeshAgent tdcc_NavMeshAgent;
    public TopDownControllerMain tdcc_Main;
    public TopDownInputManager tdcc_InputManager;
    public TopDownEquipmentManager tdcc_EquipmentManager;
    public Camera tdcc_CameraMain;
    public TopDownRpgSpellcaster tdr_Spellcaster;

    public string walkableTag = "Walkable";
    public string npcTag = "NPC";
    public string enemyTag = "Enemy";
    public string itemTag = "Item";
    public string chestTag = "Chest";
    public float enemyStopDistance = 2f;
    public float enemyStopDistanceRanged = 8f;
    public float itemStopDistance = 1f;
    public float chestStopDistance = 0.75f;
    public float defaultStopDistance = 0.2f;

    public float faceEnemyRotSpeed = 6f;

    public float sheathWeaponAfterSeconds = 12f;

    public GameObject noFocusWalkPoint;
    public bool hasWalkPoint = false;
    public bool followingCursor = false;
    public bool followedCursor = false;
    public Transform focusedTarget;
    public float distanceToFocus;

    public Vector3 hitPoint;

    public TopDownCheckUI td_CheckUI;

    public KeyCode defInteractKey = KeyCode.Mouse0;

    private void Start() {
        td_CheckUI = TopDownCheckUI.instance;

        tdcc_Main = GetComponent<TopDownControllerMain>();
        tdcc_NavMeshAgent = GetComponent<NavMeshAgent>();
        tdcc_InputManager = TopDownInputManager.instance;
        tdcc_EquipmentManager = GetComponent<TopDownEquipmentManager>();
        tdr_Spellcaster = GetComponent<TopDownRpgSpellcaster>();
        if (GameObject.FindGameObjectWithTag("MainCamera")) {
            tdcc_CameraMain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        else if(GameObject.FindObjectOfType<Camera>()) {
            tdcc_CameraMain = GameObject.FindObjectOfType<Camera>();
        }
    }

    public void Update() {

        if (tempDisable == false) {
            if (TopDownUIManager.instance != null) {
                if (Input.GetKey(tdcc_InputManager.interactKey)) {
                    MoveCharacter();
                }
                else {
                    if (followingCursor == true) {
                        if (focusedTarget == null) {
                            //Vector3 pos = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
                            GameObject tmpPoint = Instantiate(noFocusWalkPoint, hitPoint, Quaternion.identity) as GameObject;
                            SetFocus(tmpPoint.transform);
                            hasWalkPoint = true;
                        }
                        followingCursor = false;
                    }
                }
            }
            else {
                if (Input.GetKey(defInteractKey)) {
                    MoveCharacter();
                }
                else {
                    if (followingCursor == true) {
                        if (focusedTarget == null) {
                            //Vector3 pos = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
                            GameObject tmpPoint = Instantiate(noFocusWalkPoint, hitPoint, Quaternion.identity) as GameObject;
                            SetFocus(tmpPoint.transform);
                            hasWalkPoint = true;
                        }
                        followingCursor = false;
                    }
                }
            }

            if (followingCursor == false) {
                if (focusedTarget != null) {

                    distanceToFocus = Vector3.Distance(transform.position, focusedTarget.position);

                    tdcc_NavMeshAgent.SetDestination(focusedTarget.position);

                    if (tdcc_NavMeshAgent.remainingDistance > tdcc_NavMeshAgent.stoppingDistance) {
                        tdcc_Main.TDCC_MoveCharacter(tdcc_NavMeshAgent.desiredVelocity);
                        if (tdcc_Main.vegetationMoveWindZone != null) {
                            tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(true);
                        }
                    }
                    else {
                        tdcc_Main.TDCC_MoveCharacter(Vector3.zero);
                        if (tdcc_Main.vegetationMoveWindZone != null) {
                            tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(false);
                        }

                        //We needed to put this as sometimes our agent wont rotate all the way toward target
                        Quaternion targetRotation = Quaternion.LookRotation(focusedTarget.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * faceEnemyRotSpeed);

                        //COMBAT
                        if (focusedTarget.tag == enemyTag && distanceToFocus < tdcc_Main.tdcm_NavMeshAgent.stoppingDistance) {
                            if (focusedTarget.GetComponent<TopDownCharacterCard>() && focusedTarget.GetComponent<TopDownCharacterCard>().IsDead() == false) {
                                if (tdr_Spellcaster != null) {
                                    if (tdr_Spellcaster.castingSpell == false) {
                                        //here we only attack with melee
                                        int i = (int)tdcc_EquipmentManager.weaponTypeUsed;
                                        int h = (int)tdcc_EquipmentManager.weaponHoldingType;

                                        tdcc_Main.tdcm_animator.SetBool("TargetInFront", true);
                                        tdcc_Main.tdcm_animator.SetFloat("WeaponType", (float)i);
                                        tdcc_Main.tdcm_animator.SetFloat("WeaponHoldingType", (float)h);
                                        tdcc_Main.tdcm_animator.SetBool("Attacking", true);
                                    }
                                }
                                else {
                                    //here we only attack with melee
                                    int i = (int)tdcc_EquipmentManager.weaponTypeUsed;
                                    int h = (int)tdcc_EquipmentManager.weaponHoldingType;

                                    tdcc_Main.tdcm_animator.SetBool("TargetInFront", true);
                                    tdcc_Main.tdcm_animator.SetFloat("WeaponType", (float)i);
                                    tdcc_Main.tdcm_animator.SetFloat("WeaponHoldingType", (float)h);
                                    tdcc_Main.tdcm_animator.SetBool("Attacking", true);
                                }
                            }
                            else {
                                RemoveFocus();
                            }
                        }
                        else {
                            tdcc_Main.tdcm_animator.SetBool("TargetInFront", false);
                        }
                    }
                }
                else {
                    RemoveFocus();
                }
            }
        }
        else {
            RemoveFocus();
        }
    }

    public void MoveCharacter() {

        Ray ray = tdcc_CameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (td_CheckUI != null) {
            if (td_CheckUI.IsPointerOverUIObject() == false && TopDownUIInventory.instance.holdingItem == null && TopDownUIInventory.instance.clickedOutOfUi == false) {
                if (Physics.Raycast(ray, out hit, 100)) {
                    hitPoint = hit.point;
                    if (tdr_Spellcaster != null) {
                        if (tdr_Spellcaster.castingSpell) {
                            if ((hit.transform.tag == enemyTag)) {
                                if (focusedTarget != null && focusedTarget != hit.transform) {
                                    RemoveFocus();
                                    SetFocus(hit.transform);
                                }
                                else if (focusedTarget == null) {
                                    SetFocus(hit.transform);
                                }

                                followingCursor = false;
                            }
                            else if (hit.transform.tag == walkableTag) {

                                tdr_Spellcaster.castingSpell = false;

                                tdcc_NavMeshAgent.SetDestination(hit.point);

                                if (tdcc_NavMeshAgent.remainingDistance > tdcc_NavMeshAgent.stoppingDistance) {
                                    tdcc_Main.TDCC_MoveCharacter(tdcc_NavMeshAgent.desiredVelocity);
                                    if (tdcc_Main.vegetationMoveWindZone != null) {
                                        tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(true);
                                    }
                                }
                                else {
                                    tdcc_Main.TDCC_MoveCharacter(Vector3.zero);
                                    if (tdcc_Main.vegetationMoveWindZone != null) {
                                        tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(false);
                                    }
                                }

                                if (focusedTarget != null) {
                                    RemoveFocus();
                                }

                                followingCursor = true;
                            }
                        }
                        else {
                            if ((hit.transform.tag == enemyTag || hit.transform.tag == itemTag || hit.transform.tag == chestTag || hit.transform.tag == npcTag)) {
                                if (focusedTarget != null && focusedTarget != hit.transform) {
                                    RemoveFocus();
                                    SetFocus(hit.transform);
                                }
                                else if (focusedTarget == null) {
                                    SetFocus(hit.transform);
                                }

                                followingCursor = false;
                            }
                            else if (hit.transform.tag == walkableTag) {
                                tdcc_NavMeshAgent.SetDestination(hit.point);

                                if (tdcc_NavMeshAgent.remainingDistance > tdcc_NavMeshAgent.stoppingDistance) {
                                    tdcc_Main.TDCC_MoveCharacter(tdcc_NavMeshAgent.desiredVelocity);
                                    if (tdcc_Main.vegetationMoveWindZone != null) {
                                        tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(true);
                                    }
                                }
                                else {
                                    tdcc_Main.TDCC_MoveCharacter(Vector3.zero);
                                    if (tdcc_Main.vegetationMoveWindZone != null) {
                                        tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(false);
                                    }
                                }

                                if (focusedTarget != null) {
                                    RemoveFocus();
                                }

                                followingCursor = true;
                            }
                        }
                    }
                    else {
                        if ((hit.transform.tag == enemyTag || hit.transform.tag == itemTag || hit.transform.tag == chestTag || hit.transform.tag == npcTag)) {
                            if (focusedTarget != null && focusedTarget != hit.transform) {
                                RemoveFocus();
                                SetFocus(hit.transform);
                            }
                            else if (focusedTarget == null) {
                                SetFocus(hit.transform);
                            }

                            followingCursor = false;
                        }
                        else if (hit.transform.tag == walkableTag) {
                            tdcc_NavMeshAgent.SetDestination(hit.point);

                            if (tdcc_NavMeshAgent.remainingDistance > tdcc_NavMeshAgent.stoppingDistance) {
                                tdcc_Main.TDCC_MoveCharacter(tdcc_NavMeshAgent.desiredVelocity);
                                if (tdcc_Main.vegetationMoveWindZone != null) {
                                    tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(true);
                                }
                            }
                            else {
                                tdcc_Main.TDCC_MoveCharacter(Vector3.zero);
                                if (tdcc_Main.vegetationMoveWindZone != null) {
                                    tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(false);
                                }
                            }

                            if (focusedTarget != null) {
                                RemoveFocus();
                            }

                            followingCursor = true;
                        }
                    }
                }
            }
            else {
                if (followingCursor == true) {
                    if (focusedTarget == null) {
                        //Vector3 pos = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
                        GameObject tmpPoint = Instantiate(noFocusWalkPoint, hitPoint, Quaternion.identity) as GameObject;
                        SetFocus(tmpPoint.transform);
                        hasWalkPoint = true;
                    }
                    followingCursor = false;
                }
            }
        }
        else {
            if (Physics.Raycast(ray, out hit, 100)) {
                hitPoint = hit.point;
                if (hit.transform.tag == enemyTag || hit.transform.tag == itemTag || hit.transform.tag == chestTag) {
                    if (focusedTarget != null && focusedTarget != hit.transform) {
                        RemoveFocus();
                        SetFocus(hit.transform);
                    }
                    else if (focusedTarget == null) {
                        SetFocus(hit.transform);
                    }

                    followingCursor = false;
                }
                else if (hit.transform.tag == walkableTag) {
                    tdcc_NavMeshAgent.SetDestination(hit.point);

                    if (tdcc_NavMeshAgent.remainingDistance > tdcc_NavMeshAgent.stoppingDistance) {
                        tdcc_Main.TDCC_MoveCharacter(tdcc_NavMeshAgent.desiredVelocity);
                        if (tdcc_Main.vegetationMoveWindZone != null) {
                            tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(true);
                        }
                    }
                    else {
                        tdcc_Main.TDCC_MoveCharacter(Vector3.zero);
                        if (tdcc_Main.vegetationMoveWindZone != null) {
                            tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(false);
                        }
                    }

                    if (focusedTarget != null) {
                        RemoveFocus();
                    }

                    followingCursor = true;
                }
                else {
                    //If there is no focused target under our mouse click
                }
            }
        }
    }

    public void SetFocus(Transform focusObject) {

        focusedTarget = focusObject;

        if (focusObject.tag == enemyTag) {
            if (tdcc_EquipmentManager.weaponTypeUsed == WeaponType.Ranged) {
                tdcc_NavMeshAgent.stoppingDistance = enemyStopDistanceRanged;
            }
            else if(tdr_Spellcaster != null && tdr_Spellcaster.castingSpell == true) {
                tdcc_NavMeshAgent.stoppingDistance = enemyStopDistanceRanged;
            }
            else {
                tdcc_NavMeshAgent.stoppingDistance = enemyStopDistance;
            }
        }
        else if (focusObject.tag == itemTag) {
            tdcc_NavMeshAgent.stoppingDistance = itemStopDistance;
            focusedTarget.GetComponent<TopDownInteractible>().OnFocused(transform);
        }
        else if (focusObject.tag == chestTag) {
            tdcc_NavMeshAgent.stoppingDistance = chestStopDistance;
            focusedTarget.GetComponent<TopDownInteractible>().OnFocused(transform);
        }
        else if (focusObject.tag == npcTag) {
            tdcc_NavMeshAgent.stoppingDistance = enemyStopDistance;
            if (focusedTarget.GetComponent<TopDownInteractible>()) {
                focusedTarget.GetComponent<TopDownInteractible>().OnFocused(transform);
            }
        }
        else {
            tdcc_NavMeshAgent.stoppingDistance = defaultStopDistance;
        }
    }

    public void RemoveFocus() {
        if (focusedTarget != null && focusedTarget.GetComponent<TopDownInteractible>()) {
            focusedTarget.GetComponent<TopDownInteractible>().OnDefocused();
        }

        if (hasWalkPoint == true && focusedTarget != null) {
            Destroy(focusedTarget.gameObject);
        }
        hasWalkPoint = false;
        focusedTarget = null;

        distanceToFocus = 0f;

        tdcc_Main.tdcm_animator.SetBool("TargetInFront", false);
        if (tdcc_Main.tdcm_animator.GetBool("Attacking") == true) {
            //StartCoroutine(WaitToSheatheWeapon());
            tdcc_Main.tdcm_animator.SetBool("Attacking", false);

        }

        if (tdcc_NavMeshAgent != null) {
            tdcc_NavMeshAgent.stoppingDistance = 0f;
            tdcc_NavMeshAgent.SetDestination(transform.position);
        }
        tdcc_Main.TDCC_MoveCharacter(Vector3.zero);
        if(tdcc_Main.vegetationMoveWindZone != null) {
            tdcc_Main.vegetationMoveWindZone.gameObject.SetActive(false);
        }
    }

    public IEnumerator WaitToSheatheWeapon() {
        yield return new WaitForSeconds(sheathWeaponAfterSeconds);

        tdcc_Main.tdcm_animator.SetBool("Attacking", false);
        print("Sheathe weapon.");
    }
}
