using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownRpgSpellCollision : MonoBehaviour {

    public TopDownItemObject thisSpell;
    public SpellType thisSpellType;

    private void OnTriggerEnter(Collider other) {
        if(thisSpellType == SpellType.CastOnEnemy && other.tag == TopDownCharacterManager.instance.controllingCharacter.GetComponent<TopDownControllerInteract>().enemyTag) {

            if (thisSpell.spellImpactSfx != null) {
                Instantiate(thisSpell.spellImpactSfx, Vector3.zero, Quaternion.identity);
            }

            if (thisSpell.onImpactFx != null) {
                GameObject impactGo = Instantiate(thisSpell.onImpactFx, transform.position, Quaternion.identity);
                impactGo.AddComponent<TopDownToolDestroyAfterTime>().destroyAfter = 1.5f;
            }

            thisSpell.spellOnImpactEvents.Invoke();

            Destroy(gameObject);
        }
        if (thisSpellType == SpellType.CastOnAlly && other.tag == "NPC" && other.gameObject != TopDownCharacterManager.instance.controllingCharacter) {

            if (thisSpell.spellImpactSfx != null) {
                Instantiate(thisSpell.spellImpactSfx, Vector3.zero, Quaternion.identity);
            }

            if (thisSpell.onImpactFx != null) {
                GameObject impactGo = Instantiate(thisSpell.onImpactFx, transform.position, Quaternion.identity);
                impactGo.AddComponent<TopDownToolDestroyAfterTime>().destroyAfter = 1.5f;
            }

            thisSpell.spellOnImpactEvents.Invoke();

            Destroy(gameObject);
        }
    }
}
