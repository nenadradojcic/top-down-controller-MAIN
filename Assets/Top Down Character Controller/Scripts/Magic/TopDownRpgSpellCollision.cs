using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownRpgSpellCollision : MonoBehaviour {

    public TopDownItemObject thisSpell;
    public SpellType thisSpellType;

    private void OnTriggerEnter(Collider other) {
        if(thisSpellType == SpellType.CastOnEnemy && other.tag == TopDownCharacterManager.instance.activeCharacter.GetComponent<TopDownControllerInteract>().enemyTag) {

            if (thisSpell.spellImpactSfx != null) {
                Instantiate(thisSpell.spellImpactSfx, Vector3.zero, Quaternion.identity);
            }

            /*if (thisSpell.spellModifierValue > 0) {
                other.gameObject.GetComponent<TopDownCharacterCard>().health -= thisSpell.spellModifierValue;
            }*/

            GameObject impactGo = Instantiate(thisSpell.onImpactFx, transform.position, Quaternion.identity);
            impactGo.AddComponent<TopDownToolDestroyAfterTime>().destroyAfter = 1.5f;

            thisSpell.spellOnImpactEvents.Invoke();

            Destroy(gameObject);
        }
        if(thisSpellType == SpellType.CastOnAlly && other.tag == "NPC" && other.gameObject != TopDownCharacterManager.instance.activeCharacter) {

            if(thisSpell.spellImpactSfx != null) {
                Instantiate(thisSpell.spellImpactSfx, Vector3.zero, Quaternion.identity);
            }

            GameObject impactGo = Instantiate(thisSpell.onImpactFx, transform.position, Quaternion.identity);
            impactGo.AddComponent<TopDownToolDestroyAfterTime>().destroyAfter = 1.5f;

            thisSpell.spellOnImpactEvents.Invoke();

            Destroy(gameObject);
        }
    }
}
