using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownRpgSpellCollision : MonoBehaviour {

    public TopDownItemObject thisSpell;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy") {

            if (thisSpell.spellImpactSfx != null) {
                Instantiate(thisSpell.spellImpactSfx, Vector3.zero, Quaternion.identity);
            }

            if (thisSpell.spellModifierValue > 0) {
                other.gameObject.GetComponent<TopDownCharacterCard>().health -= thisSpell.spellModifierValue;
            }

            GameObject impactGo = Instantiate(thisSpell.onImpactFx, transform.position, Quaternion.identity);
            impactGo.AddComponent<TopDownToolDestroyAfterTime>().destroyAfter = 1.5f;

            Destroy(gameObject);
        }
    }
}
