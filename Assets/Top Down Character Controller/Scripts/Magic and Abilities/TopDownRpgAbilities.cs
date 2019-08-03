using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownRpgAbilities : MonoBehaviour {

    public bool usingAbility;

    public TopDownRpgAbilityObject activeAbility;

    public List<TopDownRpgAbilityObject> listOfAllAbilities;

    public Transform target;

    public Animator animator;

    public float speed = 10f;

    private TopDownControllerInteract tdcInteract;
    private TopDownCharacterCard tdcCard;

    public Vector3 hitPoint;

    private void Start() {
        animator = GetComponent<Animator>();
        tdcInteract = GetComponent<TopDownControllerInteract>();
        tdcCard = GetComponent<TopDownCharacterCard>();
    }

    public void Update() {

        if (Input.GetKeyDown(tdcInteract.defInteractKey) && usingAbility) {

            Ray ray = tdcInteract.tdcc_CameraMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (tdcInteract.td_CheckUI != null) {
                if (tdcInteract.td_CheckUI.IsPointerOverUIObject() == false && TopDownUIInventory.instance.holdingItem == null && TopDownUIInventory.instance.clickedOutOfUi == false) {
                    if (Physics.Raycast(ray, out hit, 100)) {
                        hitPoint = hit.point;
                        if (hit.transform.tag == tdcInteract.enemyTag) {
                            target = hit.transform;
                        }
                    }
                }
            }
        }

        if (Input.GetKeyUp(tdcInteract.defInteractKey) && activeAbility != null && target != null) {

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= tdcInteract.enemyStopDistanceRanged) {
                if (tdcCard.energy >= activeAbility.energyCost) {
                    StartCoroutine(ExecuteAbility());
                }
                else {
                    usingAbility = false;
                    tdcInteract.RemoveFocus();
                    target = null;
                }
            }
        }
    }

    public IEnumerator ExecuteAbility() {

        //tdcInteract.RemoveFocus();

        animator.Play(activeAbility.abilityAnim);

        yield return new WaitForSeconds(activeAbility.animTime);

        if (activeAbility.abilityType == AbilityType.Projectile) {
            GameObject fx = Instantiate(activeAbility.abilityFx as GameObject);
            fx.transform.SetParent(transform);
            fx.transform.localPosition = new Vector3(0f, gameObject.GetComponent<CapsuleCollider>().center.y, 0f);
            fx.transform.SetParent(null);

            fx.GetComponent<Rigidbody>().velocity = (target.transform.position - transform.position).normalized * speed;
        }

        usingAbility = false;

        tdcInteract.RemoveFocus();

        target = null;

        tdcCard.energy -= activeAbility.energyCost;
    }
}
