using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownRpgSpellcaster : MonoBehaviour {

    public TopDownItemObject activeSpell;
    public TopDownUIItemSlot spellItemSlot;

    public bool castingSpell;

    public Transform target;
    public Transform previousTarget;

    public Animator animator;

    public float speed = 10f;

    private SphereCollider col;

    private TopDownControllerInteract tdcInteract;
    private TopDownCharacterCard tdcCard;

    public Vector3 hitPoint;

    private void Start() {
        animator = GetComponent<Animator>();
        tdcInteract = GetComponent<TopDownControllerInteract>();
        tdcCard = GetComponent<TopDownCharacterCard>();
    }

    public void Update() {

        if (Input.GetKeyDown(tdcInteract.defInteractKey) && castingSpell) {

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

        if (Input.GetKeyUp(tdcInteract.defInteractKey) && activeSpell != null && target != null) {

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= tdcInteract.enemyStopDistanceRanged) {
                if (tdcCard.energy >= activeSpell.castingCost) {
                    StartCoroutine(CastSpell());
                }
                else {
                    castingSpell = false;
                    tdcInteract.RemoveFocus();
                    target = null;
                }
            }
        }
    }

    public IEnumerator CastSpell() {

        if (spellItemSlot != null) {
            
            if(spellItemSlot.slottedInQuick != null) {
                spellItemSlot.slottedInQuick.ClearSlot(spellItemSlot.slottedInQuick);
                spellItemSlot.slottedInQuick = null;
            }

            spellItemSlot.ClearSlot(spellItemSlot);
        }

        animator.Play(activeSpell.castingSpellAnimation);

        yield return new WaitForSeconds(activeSpell.animationTriggerTime);

        if(activeSpell.spellCastSfx != null) {
            Instantiate(activeSpell.spellCastSfx, Vector3.zero, Quaternion.identity);
        }

        if (activeSpell.spellType == SpellType.CastOnEnemy) {
            GameObject fx = Instantiate(activeSpell.spellFx as GameObject);
            fx.transform.SetParent(transform);
            fx.transform.localPosition = new Vector3(0f, gameObject.GetComponent<CapsuleCollider>().center.y, 0f);
            fx.transform.SetParent(null);

            fx.GetComponent<Rigidbody>().velocity = (target.transform.position - transform.position).normalized * speed;

            if(fx.GetComponent<TopDownRpgSpellCollision>() == null) {
                TopDownRpgSpellCollision spellCol = fx.AddComponent<TopDownRpgSpellCollision>();
                spellCol.thisSpell = activeSpell;
            }
        }

        castingSpell = false;

        tdcInteract.RemoveFocus();

        previousTarget = target;
        target = null;

        tdcCard.energy -= activeSpell.castingCost;

        activeSpell = null;
        spellItemSlot = null;
    }
}
